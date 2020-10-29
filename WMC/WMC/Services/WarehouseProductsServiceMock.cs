using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WMC.Models;

namespace WMC.Services
{
    public class WarehouseProductsServiceMock : IWarehouseProductsService<Product>
    {
        readonly List<Product> _products;

        public WarehouseProductsServiceMock()
        {
            _products = new List<Product>
            {
                new Product { Id = 1, ManufacturerName = "Samsung", ModelName = "Galaxy S9", Price = 3499, Quantity = 2},
                new Product { Id = 2, ManufacturerName = "Huawei", ModelName = "P9", Price = 1500, Quantity = 4},
                new Product { Id = 3, ManufacturerName = "Samsung", ModelName = "Galaxy S9", Price = 3499, Quantity = 2},
                new Product { Id = 4, ManufacturerName = "Huawei", ModelName = "P9", Price = 1500, Quantity = 4},
                new Product { Id = 5, ManufacturerName = "Samsung", ModelName = "Galaxy S9", Price = 3499, Quantity = 2},
                new Product { Id = 6, ManufacturerName = "Huawei", ModelName = "P9", Price = 1500, Quantity = 4},
            };
        }

        public List<string> GetSyncResultErrors() => new List<string>();

        public void ClearCache()
        {
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            return await Task.FromResult(_products);
        }

        public async Task<Product> GetProduct(long productId)
        {
            return await Task.FromResult(_products.FirstOrDefault(p => p.Id == productId));
        }

        public async Task<bool> AddProduct(Product product)
        {
            _products.Add(product);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            var product = _products.FirstOrDefault(p => p.Id == updateProduct.Id);

            if (product == null)
                return await Task.FromResult(false);

            product.ManufacturerName = updateProduct.ManufacturerName;
            product.ModelName = updateProduct.ModelName;
            product.Price = updateProduct.Price;
            return await Task.FromResult(true);
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);

            if (product == null) 
                return await Task.FromResult(false);
            
            _products.Remove(product);
            return await Task.FromResult(true);
        }

        public async Task<bool> ChangeProductQuantity(long productId, long quantityChange)
        {
            var product = _products.FirstOrDefault(p => p.Id == productId);

            if (product == null) 
                return await Task.FromResult(false);
            
            product.Quantity += quantityChange;
            return await Task.FromResult(true);
        }
    }
}
