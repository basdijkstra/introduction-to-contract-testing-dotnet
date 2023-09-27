using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace AddressProvider.Tests.Middleware
{
    public class ProviderStateMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IDictionary<string, Action> _providerStates;

        private readonly string addressId = "93edc1a1-5093-4d30-a9c1-da04765553b7";

        public ProviderStateMiddleware(RequestDelegate next)
        {
            _next = next;
            _providerStates = new Dictionary<string, Action>
            {
                {
                    $"address with ID {addressId} exists",
                    CreateAddress
                },
                {
                    $"address with ID {addressId} does not exist",
                    DeleteAddress
                },
                {
                    "no specific state required",
                    DoNothing
                }
            };
        }

        private void DoNothing()
        {
            Console.WriteLine("DO NOTHING");
        }

        private void CreateAddress()
        {
            Console.WriteLine("CREATE ADDRESS");
        }

        private void DeleteAddress()
        {
            Console.WriteLine("DELETE ADDRESS");
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/provider-states"))
            {
                await this.HandleProviderStatesRequest(context);
                await context.Response.WriteAsync(string.Empty);
            }
            else
            {
                await this._next(context);
            }
        }

        private async Task HandleProviderStatesRequest(HttpContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.OK;

            if (context.Request.Method.ToUpper() == HttpMethod.Post.ToString().ToUpper() &&
                context.Request.Body != null)
            {
                string jsonRequestBody = string.Empty;
                using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8))
                {
                    jsonRequestBody = await reader.ReadToEndAsync();
                }

                var providerState = JsonConvert.DeserializeObject<ProviderState>(jsonRequestBody);

                //A null or empty provider state key must be handled
                if (providerState != null && !string.IsNullOrEmpty(providerState.State))
                {
                    _providerStates[providerState.State].Invoke();
                }
            }
        }
    }
}
