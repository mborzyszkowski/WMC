using System;
using Newtonsoft.Json;

namespace WMC.Models
{
    public class Product
    {
        public long Id { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public double? PriceUsd { get; set; }
        [JsonIgnore] public string PriceUsdString => PriceUsd.HasValue ? PriceUsd.Value.ToString() : "No price in";
        public long Quantity { get; set; }
    }
}
