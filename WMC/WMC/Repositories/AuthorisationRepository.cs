using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Services;
using Xamarin.Forms;

namespace WMC.Repositories
{
    public class AuthorisationRepository : IAuthorisationRepository
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl =
            (Device.RuntimePlatform == Device.Android ? Constants.ApiEndpointForAndroid : Constants.ApiEndpointForIos)
            + "/access";

        public AuthorisationRepository()
        {
            _httpClient = new HttpClient();
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
                return WmcToken.CreateToken(token.Token, token.RefreshToken, token.ExpirationDate);
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
                return WmcToken.CreateToken(token.Token, token.RefreshToken, token.ExpirationDate);
            }

            return null;
        }

        public async Task<UserInfo> GetUserInfo(WmcToken token)
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);

            var url = new Uri($"{_baseUrl}/userInfo");

            var response = await _httpClient.GetAsync(url);

            _httpClient.DefaultRequestHeaders.Authorization = null;

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserInfo>(content);
            }

            return null;
        }

        public async Task<WmcToken> RefreshToken(WmcToken previousToken)
        {
            var url = new Uri($"{_baseUrl}/token/refresh");

            var body = JsonConvert.SerializeObject(new RefreshTokenForm { Token = previousToken.RefreshToken });

            var contentBody = new StringContent(body, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, contentBody);

            if (response.IsSuccessStatusCode)
            {
                var tokenContent = await response.Content.ReadAsStringAsync();
                var token = JsonConvert.DeserializeObject<WmcTokenUnsafe>(tokenContent);
                return WmcToken.CreateToken(token.Token, token.RefreshToken, token.ExpirationDate);
            }

            return null;
        }
    }
}
