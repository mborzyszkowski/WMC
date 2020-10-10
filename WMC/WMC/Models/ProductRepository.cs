using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace WMC.Models
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly HttpClient _httpClient;

        // TODO: later https://10.0.2.2/products and add 
        private readonly string _baseUrl = 
            Device.RuntimePlatform == Device.Android ? "http://192.168.2.122:8080/products" : "http://localhost:8080/products";

        public ProductRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
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
            var url = new Uri($"{_baseUrl}");

            var jsonProduct = JsonConvert.SerializeObject(product);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            var url = new Uri($"{_baseUrl}/{updateProduct.Id}");

            var jsonProduct = JsonConvert.SerializeObject(updateProduct);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            var url = new Uri($"{_baseUrl}/{productId}");

            var response = await _httpClient.GetAsync(url);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeProductQuantity(long productId, long count)
        {
            var url = new Uri($"{_baseUrl}/{productId}/{count}");

            var response = await _httpClient.GetAsync(url);

            return response.IsSuccessStatusCode;
        }
    }
}
