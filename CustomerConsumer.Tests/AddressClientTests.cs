using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;
using System.Net;
using CustomerConsumer.Models;

namespace CustomerConsumer.Tests
{
    [TestFixture]
    public class AddressClientTests
    {
        private IPactBuilderV3 pact;
        private AddressClient client;
        private Address address;

        private readonly int port = 9876;

        private readonly string addressId = "93edc1a1-5093-4d30-a9c1-da04765553b7";

        [SetUp]
        public void SetUp()
        {
            var Config = new PactConfig
            {
                PactDir = Path.Join("..", "..", "..", "pacts"),
                DefaultJsonSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                }
            };

            pact = Pact.V3("customer_consumer", "address_provider", Config).WithHttpInteractions(port);
            client = new AddressClient(new Uri($"http://localhost:{port}"));

            address = new Address
            {
                Id = Guid.Parse(addressId),
                AddressType = "billing",
                Street = "Main street",
                Number = 123,
                City = "Los Angeles",
                ZipCode = 12345,
                State = "California",
                Country = "United States"
            };
        }

        [Test]
        public async Task GetAddress()
        {
            pact.UponReceiving("A valid request for an address")
                    .Given($"address with ID {addressId} exists")
                    .WithRequest(HttpMethod.Get, $"/address/{addressId}")
                .WillRespond()
                    .WithStatus(HttpStatusCode.OK)
                    .WithHeader("Content-Type", "application/json; charset=utf-8")
                    .WithJsonBody(new TypeMatcher(address));

            await pact.VerifyAsync(async ctx => {
                var response = await client.GetAddress(addressId);
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
    }
}
