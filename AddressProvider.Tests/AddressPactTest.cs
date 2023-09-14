using Microsoft.Extensions.Hosting;
using PactNet.Verifier;

namespace AddressProvider.Tests
{
    [TestFixture]
    public class AddressPactTest
    {
        private const string PactServiceUri = "http://127.0.0.1:9876";

        [Test]
        public void VerifyThatAddressServiceHonoursPacts()
        {
            using var app = Startup.WebApp();
            app.Urls.Add(PactServiceUri);
            app.Start();
            
            using var verifier = new PactVerifier(new PactVerifierConfig());
            var pact = new FileInfo(Path.Join("..", "..", "..", "pacts", "customer_consumer-address_provider.json"));
            verifier.ServiceProvider("address_provider", new Uri(PactServiceUri))
                .WithFileSource(pact)
                .Verify();
        }
    }
}
