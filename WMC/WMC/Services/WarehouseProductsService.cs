using System.Collections.Generic;
using System.Threading.Tasks;
using WMC.Models;
using WMC.Repositories;

namespace WMC.Services
{
    public class WarehouseProductsService : IWarehouseProductsService<Product>
    {
        private readonly IProductRepository<Product> _repository = new ProductRepository();

        public List<string> GetSyncResultErrors()
        {
            return _repository.GetSyncResultErrors();
        }

        public void ClearCache()
        {
            _repository.ClearCache();
        }

        public async Task<IEnumerable<Product>> GetProductsList()
        {
            return await _repository.GetProductsList();
        }

        public async Task<Product> GetProduct(long productId)
        {
            return await _repository.GetProduct(productId);
        }

        public async Task<bool> AddProduct(Product product)
        {
            return await _repository.AddProduct(product);
        }

        public async Task<bool> UpdateProduct(Product updateProduct)
        {
            return await _repository.UpdateProduct(updateProduct);
        }

        public async Task<bool> RemoveProduct(long productId)
        {
            return await _repository.RemoveProduct(productId);
        }

        public async Task<bool> ChangeProductQuantity(long productId, long count)
        {
            return await _repository.ChangeProductQuantity(productId, count);
        }
    }
}
