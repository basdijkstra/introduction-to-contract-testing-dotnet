using NUnit.Framework;
using System.Net;

namespace CustomerConsumer.Tests
{
    [TestFixture]
    public class AddressClientGetTests : PactTestBase
    {
        [Test]
        public async Task GetAddress_AddressIdExists()
        {
            pact.UponReceiving("A request to retrieve an address")
                    .Given($"address with ID {addressId} exists")
                    .WithRequest(HttpMethod.Get, $"/address/{addressId}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(address);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress(addressId);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public async Task GetAddress_AddressIdDoesNotExist()
        {
            pact.UponReceiving("A request to retrieve an address")
                    .Given($"address with ID {addressId} does not exist")
                    .WithRequest(HttpMethod.Get, $"/address/{addressId}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.NotFound);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress(addressId);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public async Task GetAddress_AddressIdIsInvalid()
        {
            pact.UponReceiving("A request to retrieve an address")
                    .Given($"no specific state required")
                    .WithRequest(HttpMethod.Get, "/address/invalid_address_id")
                .WillRespond()
                    .WithStatus(HttpStatusCode.BadRequest);

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress("invalid_address_id");
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
    }
}
