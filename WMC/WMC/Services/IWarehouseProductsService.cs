using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMC.Services
{
    public interface IWarehouseProductsService<T>
    {
        Task<bool> AddProduct(T product);
        Task<bool> RemoveProduct(long productId);
        Task<bool> UpdateProduct(T updateProduct);
        Task<T> GetProduct(long productId);
        Task<IEnumerable<T>> GetProductsList();
        Task<bool> ChangeProductQuantity(long productId, long count);
    }
}
