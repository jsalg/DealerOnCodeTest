using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSalesApp
{
    /// <summary>
    ///  Class responsible for proccessing items in Shopping Cart and Print them
    ///  in format required
    /// </summary>

    public class PointofService
    {
        public static void CreateLineItems(List<LineItem> bc, List<Product> inventory)
        {
            const decimal TaxRate = .10M;
            const decimal ImportRate = .05M;
            const decimal FullImportRate = .15M;

            // Used Linq to find a match on name and return inventory Sku, that will be helpful for find tax rate method
            var searchInventorySku = from b in bc
                                     join i in inventory
                                     on b.Name.ToUpper() equals i.Name.ToUpper()
                                     select new
                                     {
                                         i.Name,
                                         b.Price,
                                         i.Sku
                                     };

            var final = new List<Product>();

            foreach (var line in searchInventorySku)
            {
                var taxRate = FindTaxRate(line.Sku);
                final.Add(new Product { Name = line.Name, Price = line.Price, Sku = line.Sku, Taxable = taxRate });
            }

            // Group each product
            var groupItems = final
           .GroupBy(l => l.Sku)
           .Select(cl => new Product
           {
               Name = cl.First().Name,
               Taxable = cl.First().Taxable,
               Quantity = cl.Count(),
               Price = cl.First().Price,
               SalesTax = cl.First().Taxable == TaxStatus.Tax ? Math.Ceiling(cl.First().Price * ((TaxRate) /.05M))*.05M :
                 cl.First().Taxable == TaxStatus.PartialImport ? Math.Ceiling(cl.First().Price * ((ImportRate) / .05M)) *.05M :
                 cl.First().Taxable == TaxStatus.FullImport ? Math.Ceiling(cl.First().Price * ((FullImportRate) / .05M)) * .05M :
                 cl.First().Taxable == TaxStatus.NoTax ? 0M : 0M
           }).ToList();

            //write the reciept lines
            foreach (var item in groupItems)
            {
                if (item.Quantity != 1)
                {
                    Console.WriteLine($"{item.Name}: {(item.Price + item.SalesTax)*(item.Quantity)} ({item.Quantity} @ {item.Price + item.SalesTax})");
                }
                else
                {
                    Console.WriteLine($"{item.Name}: {item.Price + item.SalesTax}");
                }
            }

            //Calcuate sales tax and Grand Total.
            var salesTax = groupItems.Sum(x => (x.SalesTax) * (x.Quantity));
            var grandTotal = groupItems.Sum(x => (x.Price + x.SalesTax) * (x.Quantity));

            Console.WriteLine($"Sales Taxes: {salesTax}");
            Console.WriteLine($"Total: {grandTotal}");

        }

        /// <summary>
        /// Determine Items(s) tax rate
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        private static TaxStatus FindTaxRate(int sku)
        {
            if (sku < 2000)
            {
                return TaxStatus.NoTax;
            }
            else if (sku >= 2000 && sku < 6000)
            {
                return TaxStatus.Tax;
            }
            else if (sku >= 6000 && sku < 6499)
            {
                return TaxStatus.PartialImport;
            }
            else if(sku >= 6500)
            {
                return TaxStatus.FullImport;
            }
            else
            {
                return TaxStatus.NoTax;
            }
        }
    }
}
