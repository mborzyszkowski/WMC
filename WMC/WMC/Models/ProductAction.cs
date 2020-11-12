using System;

namespace WMC.Models
{
    public class ProductAction
    {
        public enum ActionType
        {
            Add, Update, Delete, ChangeQuantity
        }
        
        public ActionType Action { get; set; }
        // All existing on API, if id is negative then it is new item
        public long Id { get; set; }
        // On update/create
        public string ManufacturerName { get; set; }
        // On update/create
        public string ModelName { get; set; }
        // On update/create
        public double Price { get; set; }
        // On update/create
        public double? PriceUsd { get; set; }
        // On quantity change
        public long QuantityChange { get; set; }

        public static ProductAction AddProduct(Product product) =>
            new ProductAction
            {
                Action = ActionType.Add,
                Id = product.Id,
                ManufacturerName = product.ManufacturerName,
                ModelName = product.ModelName,
                Price = product.Price,
                PriceUsd = product.PriceUsd,
            };

        public static ProductAction UpdateProduct(Product product) =>
            new ProductAction
            {
                Action = ActionType.Update,
                Id = product.Id,
                ManufacturerName = product.ManufacturerName,
                ModelName = product.ModelName,
                Price = product.Price,
                PriceUsd = product.PriceUsd,
            };

        public static ProductAction DeleteProduct(long productId) =>
            new ProductAction
            {
                Action = ActionType.Delete,
                Id = productId,
            };

        public static ProductAction ChangeQuantityOfProduct(long productId, long quantityChange) =>
            new ProductAction
            {
                Action = ActionType.ChangeQuantity,
                Id = productId,
                QuantityChange = quantityChange,
            };
    }
}
