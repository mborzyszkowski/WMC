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
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly HttpClient _httpClient;

        private readonly string _baseUrl = 
            (Device.RuntimePlatform == Device.Android ? Constants.ApiEndpointForAndroid : Constants.ApiEndpointForIos)
            + "/products";

        public ProductRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        private async Task SetupToken()
        {
            var authenticationService = DependencyService.Get<IAuthenticationService>();

            var token = await authenticationService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Bearer", token.Token);
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            await SetupToken();
            
            var url = new Uri($"{_baseUrl}");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<Product>>(content);
            }

            return new List<Product>();
        }

        public async Task<Product> GetProduct(long productId)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(content);
            }

            return null;
        }

        public async Task<bool> AddProduct(Product product)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}");

            var jsonProduct = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{updateProduct.Id}");

            var jsonProduct = JsonConvert.SerializeObject(updateProduct);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}");

            var response = await _httpClient.DeleteAsync(url);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeProductQuantity(long productId, long count)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}/{count}");

            var response = await _httpClient.PutAsync(url, null);

            return response.IsSuccessStatusCode;
        }
    }
}
