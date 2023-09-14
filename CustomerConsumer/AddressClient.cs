namespace CustomerConsumer
{
    public class AddressClient
    {
        private readonly Uri baseUri;

        public AddressClient(Uri baseUri)
        {
            this.baseUri = baseUri;
        }

        public async Task<HttpResponseMessage> GetAddress(string id)
        {
            using (var client = new HttpClient { BaseAddress = baseUri })
            {
                try
                {
                    var response = await client.GetAsync($"/address/{id}");
                    return response;
                }
                catch (Exception ex)
                {
                    throw new Exception("There was a problem connecting to the AddressProvider API.", ex);
                }
            }
        }
    }
}