namespace ConsoleSalesApp
{
    public class Product : LineItem
    {
        public int Sku { get; set; }
        public TaxStatus Taxable { get; set; }
    }
}