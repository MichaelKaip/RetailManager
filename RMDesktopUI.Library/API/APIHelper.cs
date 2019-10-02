using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RMDesktopUI.Library.Models;
using RMDesktopUI.Models;

namespace RMDesktopUI.Library.API
{
    /*
     * The aim of this class is to handle all API call interactions between the different
     * views and the API.
     */
    public class APIHelper : IAPIHelper
    { 
        private HttpClient _apiClient;
        private readonly ILoggedInUserModel _loggedInUser;

        public HttpClient ApiClient => _apiClient;

        public APIHelper(ILoggedInUserModel loggedInUser)
        {
            InitializeClient();
            _loggedInUser = loggedInUser;
        }

        private void InitializeClient()
        {
            var api = ConfigurationManager.AppSettings["api"];

            _apiClient = new HttpClient {BaseAddress = new Uri(api)};
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            using (HttpResponseMessage response = await _apiClient.PostAsync("/Token", data)) 
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


        public void LogOffUser()
        {
            _apiClient.DefaultRequestHeaders.Clear();
        }

        public async Task<LoggedInUserModel> GetLoggedInUserInfo(string token)
        {
            _apiClient.DefaultRequestHeaders.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Clear();
            _apiClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            // Adding the "Authorization Bearer Token" to the DefaultRequestHeader for every call.
            _apiClient.DefaultRequestHeaders.Add("Authorization", $"Bearer { token }");

            using (HttpResponseMessage response = await _apiClient.GetAsync("/api/User"))
            {
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<LoggedInUserModel>();

                    // data mapping
                    _loggedInUser.CreatedDate = result.CreatedDate;
                    _loggedInUser.EmailAdress = result.EmailAdress;
                    _loggedInUser.FirstName = result.FirstName;
                    _loggedInUser.Id = result.Id;
                    _loggedInUser.LastName = result.LastName;
                    _loggedInUser.Token = token;

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
