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
        // All existing on API, if empty then product created but not sync with api
        public long? Id { get; set; }
        // All products
        public Guid Guid { get; set; } = Guid.NewGuid();
        // On update/create
        public string ManufacturerName { get; set; }
        // On update/create
        public string ModelName { get; set; }
        // On update/create
        public double Price { get; set; }
        // On quantity change
        public long QuantityChange { get; set; }
    }
}
