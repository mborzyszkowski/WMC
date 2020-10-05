using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    class WarehouseProductsMock : IWarehouseProducts<Product>
    {
        readonly List<Product> products;

        public WarehouseProductsMock()
        {
            products = new List<Product>
            {
                new Product() { Id = 1, ManufacturerName = "Samsung", ModelName = "Galaxy S9", Price = 3499, Quantity = 2},
                new Product() { Id = 2, ManufacturerName = "Huawei", ModelName = "P9", Price = 1500, Quantity = 4}
            };
        }

        public async Task<bool> AddProduct(Product product)
        {
            products.Add(product);
            return await Task.FromResult(true);
        }

        public async Task<Product> GetProduct(long productId)
        {
            return await Task.FromResult(products.Where(p => p.Id == productId).FirstOrDefault());
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            //HttpClient client = new HttpClient();
            //string content = await client.GetStringAsync("http://apiIp:8080/products");

            return await Task.FromResult(products);
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();

            if (product != null)
            {
                products.Remove(product);
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            Product product = products.Where(p => p.Id == updateProduct.Id).FirstOrDefault();

            if (product != null)
            {
                product.ManufacturerName = updateProduct.ManufacturerName;
                product.ModelName = updateProduct.ModelName;
                product.Price = updateProduct.Price;
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> IncreaseProductQuantity(long productId, int intCount)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();
            
            if (product != null)
            {
                product.Quantity += intCount;
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }

        public async Task<bool> DecreaseProductQuantity(long productId, int decCount)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();
           
            if (product != null)
            {
                product.Quantity -= decCount;
                return await Task.FromResult(true);
            }

            return await Task.FromResult(false);
        }
    }
}
