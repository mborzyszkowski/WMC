using System;

namespace WMC.Models
{
    public class Product
    {
        public long? Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public long Quantity { get; set; }
    }
}
