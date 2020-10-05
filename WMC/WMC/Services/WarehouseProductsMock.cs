using System;
using System.Collections.Generic;
using System.Linq;
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
                new Product() { Id = 1, ManufacturerName = " Samsung", ModelName = "Galaxy S9", Price = 3499, Quantity = 2}
            };
        }

        public bool AddProduct(Product product)
        {
            products.Add(product);
            return true;
        }

        public Product GetProduct(long productId)
        {
            return products.Where(p => p.Id == productId).FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsList()
        {
            return products;
        }

        public bool RemoveProduct(long productId)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();

            if (product != null)
            {
                products.Remove(product);
                return true;
            }

            return false;
        }

        public bool UpdateProduct(Product updateProduct)
        {
            Product product = products.Where(p => p.Id == updateProduct.Id).FirstOrDefault();

            if (product != null)
            {
                product.ManufacturerName = updateProduct.ManufacturerName;
                product.ModelName = updateProduct.ModelName;
                product.Price = updateProduct.Price;
                return true;
            }

            return false;
        }

        public bool IncreaseProductQuantity(long productId, int intCount)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();
            
            if (product != null)
            {
                product.Quantity += intCount;
                return true;
            }

            return false;
        }

        public bool DecreaseProductQuantity(long productId, int decCount)
        {
            Product product = products.Where(p => p.Id == productId).FirstOrDefault();
           
            if (product != null)
            {
                product.Quantity -= decCount;
                return true;
            }

            return false;
        }
    }
}
