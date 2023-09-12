using AddressProvider.Models;
using Microsoft.AspNetCore.Mvc.Testing;

using static RestAssured.Dsl;

namespace AddressProvider.Tests
{
    [TestFixture]
    public class AddressIntegrationTests
    {
        [Test]
        public void SupplyNonExistentAddressId_CheckStatusCode_ShouldBe404()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            Guid uuid = Guid.NewGuid();

            Given(httpClient)
                .When()
                .Get($"http://localhost:5005/address/{uuid}")
                .Then()
                .StatusCode(404);
        }

        [Test]
        public void CreateNewAddress_RetrieveDetails_ShouldReturn200()
        {
            var webAppFactory = new WebApplicationFactory<Program>();
            var httpClient = webAppFactory.CreateDefaultClient();

            Guid uuid = Guid.NewGuid();

            Address address = new Address
            {
                Id = uuid,
                AddressType = "billing",
                Street = "Main street",
                Number = 123,
                City = "Los Angeles",
                ZipCode = 12345,
                State = "California",
                Country = "United States"
            };

            Given(httpClient)
                .Body(address)
                .When()
                .Post("http://localhost:5005/address")
                .Then()
                .StatusCode(201);

            Given(httpClient)
                .When()
                .Get($"http://localhost:5005/address/{uuid}")
                .Then()
                .StatusCode(200)
                .Body("$.state", NHamcrest.Is.EqualTo("California"));
        }
    }
}
