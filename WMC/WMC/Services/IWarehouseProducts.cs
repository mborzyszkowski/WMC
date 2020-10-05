using System.Collections.Generic;

namespace WMC.Services
{
    interface IWarehouseProducts<T>
    {
        bool AddProduct(T product);
        bool RemoveProduct(long productId);
        bool UpdateProduct(T updateProduct);
        T GetProduct(long productId);
        IEnumerable<T> GetProductsList();
        bool IncreaseProductQuantity(long productId, int intCount);
        bool DecreaseProductQuantity(long productId, int decCount);
    }
}
