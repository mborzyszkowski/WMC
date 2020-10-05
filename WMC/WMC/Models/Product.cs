namespace WMC.Models
{
    class Product
    {
        public long Id { get; set; }
        public string ManufacturerName { get; set; }
        public string ModelName { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }
}
