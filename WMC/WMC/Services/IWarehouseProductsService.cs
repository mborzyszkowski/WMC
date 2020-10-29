using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMC.Services
{
    public interface IWarehouseProductsService<T>
    {
        List<string> GetSyncResultErrors();
        void ClearCache();
        Task<IEnumerable<T>> GetProductsList();
        Task<T> GetProduct(long productId);
        Task<bool> AddProduct(T product);
        Task<bool> UpdateProduct(T updateProduct);
        Task<bool> RemoveProduct(long productId);
        Task<bool> ChangeProductQuantity(long productId, long count);
    }
}
