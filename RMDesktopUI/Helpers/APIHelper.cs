using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RMDesktopUI.Models;

namespace RMDesktopUI.Helpers
{
    /*
     * The aim of this class is to handle all API call interactions between the different views and the API.
     */
    public class APIHelper : IAPIHelper
    {
        private HttpClient ApiClient { get; set; }

        public APIHelper()
        {
            InitializeClient();
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            ApiClient = new HttpClient {BaseAddress = new Uri(api)};
            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<AuthenticatedUser> Authenticate(string username, string password)
        {
            var data = new FormUrlEncodedContent(new []
            {
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password), 
            });

            // Getting the token
            using (HttpResponseMessage response = await ApiClient.PostAsync("/Token", data)) 
            {
                if (response.IsSuccessStatusCode)
                {
                    // ReadAsAsync needs Microsoft.AspNet.WebApi.Client (NuGet-Package)
                    var result = await response.Content.ReadAsAsync<AuthenticatedUser>(); 
                    return result;
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
        }
    }
}
