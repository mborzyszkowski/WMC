using System.Collections.Generic;
using System.Threading.Tasks;

namespace WMC.Services
{
    interface IWarehouseProducts<T>
    {
        Task<bool> AddProduct(T product);
        Task<bool> RemoveProduct(long productId);
        Task<bool> UpdateProduct(T updateProduct);
        Task<T> GetProduct(long productId);
        Task<IEnumerable<T>> GetProductsList();
        Task<bool> IncreaseProductQuantity(long productId, int intCount);
        Task<bool> DecreaseProductQuantity(long productId, int decCount);
    }
}
