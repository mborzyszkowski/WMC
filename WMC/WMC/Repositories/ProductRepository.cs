using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WMC.Models;
using WMC.Services;
using Xamarin.Forms;
using System.Reactive.Linq;
using Akavache;
using Xamarin.Essentials;
using System.Reactive.Linq;
using WMC.Exceptions;

namespace WMC.Repositories
{
    public class ProductRepository : IProductRepository<Product>
    {
        private readonly string _baseUrl = 
            (Device.RuntimePlatform == Device.Android ? Constants.ApiEndpointForAndroid : Constants.ApiEndpointForIos)
            + "/products";

        private readonly HttpClient _httpClient;
        private readonly IBlobCache _cache;
        private List<Product> _products;
        private List<ProductAction> _productActions;
        private List<string> _syncResultErrors = new List<string>();

        public ProductRepository()
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            _cache = BlobCache.LocalMachine;

            var productsCache = GetFromCache<List<Product>>(Constants.StorageProducts);
            _products = productsCache ?? new List<Product>();

            var productActionCache = GetFromCache<List<ProductAction>>(Constants.StorageProductActions);
            _productActions = productActionCache ?? new List<ProductAction>();
        }

        public List<string> GetSyncResultErrors() => new List<string>(_syncResultErrors);

        public void ClearCache() => ClearCache(true);

        public void ClearCache(bool clearProductsCache)
        {
            if (clearProductsCache)
            {
                _products = new List<Product>();
                var productsClear = _cache.InvalidateObject<List<Product>>(Constants.StorageProducts);
                productsClear.Wait();
            }

            _productActions = new List<ProductAction>();
            var productsActionsClear = _cache.InvalidateObject<List<ProductAction>>(Constants.StorageProductActions);
            productsActionsClear.Wait();
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return _products;
            }

            if (_productActions.Count == 0)
            {
                return await GetProductsListUsingApi();
            }

            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");
        }

        public async Task<IEnumerable<Product>> GetProductsListUsingApi()
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(content);

                if (products != null)
                {
                    await SaveProductsInCache(products);
                    _products = new List<Product>(products);
                    return products;
                }
            }

            return new List<Product>();
        }

        public async Task<Product> GetProduct(long productId)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            { 
                return _products.First(p => p.Id == productId);
            }

            if (_productActions.Count == 0)
            {
                return await GetProductUsingApi(productId);
            }

            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");

        }

        public async Task<Product> GetProductUsingApi(long productId)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}");

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Product>(content);
            }

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProductNotFoundException("Selected product no longer exists");
            }

            return null;
        }

        public async Task<bool> AddProduct(Product product)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return await AddProductUsingCache(product);
            }

            if (_productActions.Count == 0)
            {
                return await AddProductUsingApi(product);
            }

            await AddProductUsingCache(product);
            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");
        }

        private async Task<bool> AddProductUsingCache(Product product)
        {
            product.Id = GetNextIdForNewItem();
            _products.Add(product);
            await SaveProductsInCache(_products);

            var productAddAction = ProductAction.AddProduct(product);
            _productActions.Add(productAddAction);
            await SaveProductsActionsInCache(_productActions);

            return true;
        }

        private async Task<bool> AddProductUsingApi(Product product)
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
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return await UpdateProductUsingCache(updateProduct);
            }

            if (_productActions.Count == 0)
            {
                return await UpdateProductUsingApi(updateProduct);
            }

            await UpdateProductUsingCache(updateProduct);
            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");
        }

        public async Task<bool> UpdateProductUsingCache(Product updateProduct)
        {
            var product = _products.First(p => p.Id == updateProduct.Id);

            if (product == null)
            {
                return false;
            }

            product.ManufacturerName = updateProduct.ManufacturerName;
            product.ModelName = updateProduct.ModelName;
            product.Price = updateProduct.Price;
            await SaveProductsInCache(_products);

            var productUpdateAction = ProductAction.UpdateProduct(product);
            _productActions.Add(productUpdateAction);
            await SaveProductsActionsInCache(_productActions);

            return true;
        }

        public async Task<bool> UpdateProductUsingApi(Product updateProduct)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{updateProduct.Id}");

            var jsonProduct = JsonConvert.SerializeObject(updateProduct);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProductNotFoundException("Selected product no longer exists");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return await RemoveProductUsingCache(productId);
            }

            if (_productActions.Count <= 0)
            {
                return await RemoveProductUsingApi(productId);
            }

            await RemoveProductUsingCache(productId);
            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");
        }

        public async Task<bool> RemoveProductUsingCache(long productId)
        {
            var product = _products.First(p => p.Id == productId);

            if (product == null)
            {
                return false;
            }

            _products.Remove(product);
            await SaveProductsInCache(_products);

            var productRemoveAction = ProductAction.DeleteProduct(productId);
            _productActions.Add(productRemoveAction);
            await SaveProductsActionsInCache(_productActions);

            return true;
        }

        public async Task<bool> RemoveProductUsingApi(long productId)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}");

            var response = await _httpClient.DeleteAsync(url);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProductNotFoundException("Selected product no longer exists");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangeProductQuantity(long productId, long quantityChange)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                return await ChangeProductQuantityUsingCache(productId, quantityChange);
            }

            if (_productActions.Count <= 0)
            {
                return await ChangeProductQuantityUsingApi(productId, quantityChange);
            }

            await ChangeProductQuantityUsingCache(productId, quantityChange);
            await SyncProducts();
            throw new SyncRedirectException("Products have been synchronized");

        }

        public async Task<bool> ChangeProductQuantityUsingCache(long productId, long quantityChange)
        {
            var product = _products.First(p => p.Id == productId);

            if (product == null)
            {
                return false;
            }

            product.Quantity += quantityChange;
            await SaveProductsInCache(_products);

            var changeQuantityOfProductAction = ProductAction.ChangeQuantityOfProduct(productId, quantityChange);
            _productActions.Add(changeQuantityOfProductAction);
            await SaveProductsActionsInCache(_productActions);

            return true;
        }

        public async Task<bool> ChangeProductQuantityUsingApi(long productId, long quantityChange)
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/{productId}/{quantityChange}");

            var response = await _httpClient.PutAsync(url, null);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ProductNotFoundException("Selected product no longer exists");
            }

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> SyncProducts()
        {
            await SetupToken();

            var url = new Uri($"{_baseUrl}/syncProducts");

            var jsonProduct = JsonConvert.SerializeObject(_productActions);
            var content = new StringContent(jsonProduct, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var syncErrorsResponse = await response.Content.ReadAsStringAsync();
                _syncResultErrors = JsonConvert.DeserializeObject<List<string>>(syncErrorsResponse);
                ClearCache(false);
                return true;
            }

            return false;
        }

        /*
         * PRIVATE METHODS
         */

        private async Task SetupToken()
        {
            var authenticationService = DependencyService.Get<IAuthenticationService>();

            var token = await authenticationService.GetToken();

            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token.Token);
        }

        private TItem GetFromCache<TItem>(string cacheName)
        {
            try
            {
                var item = _cache.GetObject<TItem>(cacheName);
                return item.Wait();
            }
            catch (KeyNotFoundException)
            {
                return default;
            }
        }

        private long GetNextIdForNewItem()
        {
            if (_products.Count == 0)
            {
                return -1;
            }

            var minProductId = _products.Select(p => p.Id).Min();
            return minProductId > 0 ? -1 : minProductId - 1;
        }

        private async Task SaveProductsInCache(List<Product> products)
        {
            await _cache.InsertObject(Constants.StorageProducts, products);
        }

        private async Task SaveProductsActionsInCache(List<ProductAction> productActions)
        {
            await _cache.InsertObject(Constants.StorageProductActions, productActions);
        }
    }
}
