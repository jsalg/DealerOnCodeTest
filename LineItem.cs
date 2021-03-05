using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleSalesApp
{
    public class LineItem
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal SumoflineItem { get; set; }
        public int Quantity { get; set; }
        public decimal SalesTax { get; set; }
    }
}
