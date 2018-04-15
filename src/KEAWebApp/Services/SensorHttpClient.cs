using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace KEAWebApp.Services
{
    public class SensorHttpClient : ISensorHttpClient
    {
        private readonly IHttpContextAccessor _contextAccesssor;
        private string ApiUrl = "http://sysint-rest.azurewebsites.net/";
        private readonly HttpClient client = new HttpClient();

        public SensorHttpClient(IHttpContextAccessor accessor)
        {
            _contextAccesssor = accessor;
        }

        public async Task<HttpClient> GetClient()
        {
            /*
            //getting the token variable to hold it
            string accessToken = string.Empty;
            //getting the context
            var currentContext = _accessor.HttpContext;
            //getting the token from the context
            accessToken = await currentContext.GetTokenAsync(OpenIdConnectParameterNames.AccessToken);
            //setting the token as a bearer token if found
            if (!string.IsNullOrWhiteSpace(accessToken))
            {
                client.SetBearerToken(accessToken);
            }
            */
            client.BaseAddress = new Uri(ApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return client;

        }
    }
}
