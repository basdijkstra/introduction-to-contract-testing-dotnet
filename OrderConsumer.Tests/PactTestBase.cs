using Newtonsoft.Json;
using NUnit.Framework;
using PactNet;
using PactNet.Matchers;

namespace OrderConsumer.Tests
{
    public class PactTestBase
    {
        protected IPactBuilderV3 pact;
        protected AddressClient client;
        protected object address;
        protected readonly string addressId = "93edc1a1-5093-4d30-a9c1-da04765553b7";

        private string pactDir = Path.Join("..", "..", "..", "pacts");
        private readonly int port = 9876;

        [SetUp]
        public void SetUp()
        {
            var Config = new PactConfig
            {
                PactDir = pactDir,
                DefaultJsonSettings = new JsonSerializerSettings()
            };

            pact = Pact.V3("order_consumer", "address_provider", Config).WithHttpInteractions(port);
            client = new AddressClient(new Uri($"http://localhost:{port}"));

            address = new
            {
                Id = Match.Regex(addressId, "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}"),
                AddressType = Match.Type("billing"),
                Street = Match.Type("Main street"),
                Number = Match.Integer(123),
                City = Match.Type("Los Angeles"),
                ZipCode = Match.Integer(12345),
                State = Match.Type("California"),
                Country = Match.Type("United States")
            };
        }
    }
}
