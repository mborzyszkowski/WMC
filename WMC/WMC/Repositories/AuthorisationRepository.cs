using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Services;
using Xamarin.Forms;

namespace WMC.Repositories
{
    class AuthorisationRepository : IAuthorisationRepository
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl =
            (Device.RuntimePlatform == Device.Android ? Constants.ApiEndpointForAndroid : Constants.ApiEndpointForIos)
            + "/access";

        public AuthorisationRepository()
        {
            _httpClient = new HttpClient(DependencyService.Get<IHttpClientHandlerService>().GetInsecureHandler());
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<WmcToken> AuthenticateWithFacebook(string facebookToken)
        {
            var url = new Uri($"{_baseUrl}/facebook");

            var body = JsonConvert.SerializeObject(new FacebookAuthForm {Token = facebookToken});
            
            var contentBody = new StringContent(body, Encoding.UTF8, "application/json");

            var response =  await _httpClient.PostAsync(url, contentBody);

            if (response.IsSuccessStatusCode)
            {
                var tokenContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<WmcTokenUnsafe>(tokenContent);
                return WmcToken.CreateToken(token.Token, token.ExpirationDate);
            }

            return null;
        }

        public async Task<WmcToken> AuthenticateWithWmc(string name, string password)
        {
            var url = new Uri($"{_baseUrl}/wmc");

            var body = JsonConvert.SerializeObject(new WmcAuthForm { UserName = name, Password = password});

            var contentBody = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, contentBody);

            if (response.IsSuccessStatusCode)
            {
                var tokenContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<WmcTokenUnsafe>(tokenContent);
                return WmcToken.CreateToken(token.Token, token.ExpirationDate);
            }

            return null;
        }
    }
}
