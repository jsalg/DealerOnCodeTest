using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleSalesApp

{/// <summary>
///  Assumption of requirements is that you will enter each item one at a time, and that the string pattern 
///  is exactly the way you laid it out in the requirements "1 ProductName at Price" and it will accept and read that string. this Program for 
///  ease will allow you to enter the next scenario/ Transaction after you Check out! more notes through out the program
/// </summary>
    public class Program
    {
        public static List<Product> Inventory { get; }

        //Using a Constructer to instantiate the Inventory of the store.
        static Program()
        {
            Inventory = new List<Product>
            {
                new Product { Name = "Chocolate bar", Sku = 1234 },
                new Product { Name = "Book", Sku = 1345 },
                new Product { Name = "Music CD", Sku = 2222 },
                new Product { Name = "Bottle of perfume", Sku = 3333},
                new Product { Name = "Packet of headache pills", Sku = 1444},
                new Product { Name = "Imported box of chocolates", Sku = 6000},
                new Product { Name = "Imported bottle of perfume", Sku = 6500}
            };
        }

        static void Main(string[] args)
        {
            bool userWantsToExit = false;
            while (!userWantsToExit)
            {
                //barcode list of sku # for eaiser for identifying Products and thier tax rate later on
                var itemBarcode = new List<LineItem>();

                Console.WriteLine("Welcome to Jason's MiniMart!");
                Console.WriteLine("****************************");

                Console.WriteLine("Please enter item to be added to Shoping basket one item at time");

                var input = Console.ReadLine();
                ReadString(input, itemBarcode);
                Console.WriteLine("" +
                    "");

                while (input.ToUpper().Trim() != "CHECKOUT")
                {
                    Console.WriteLine("Add another Item or enter Checkout to Print Reciept ");
                    try
                    {
                        input = Console.ReadLine();
                    }
                    catch
                    {
                        Console.WriteLine("Add another Item or enter Checkout to Print Reciept ");
                    }

                    //Not case Sensitive.
                    if (input.ToUpper().Trim() != "CHECKOUT")
                    {
                        ReadString(input, itemBarcode);
                        Console.WriteLine("" +
                        "");
                    }
                }

                Console.WriteLine("**********************");

                // Calculates and Prints Reciept after typing Checkout,
                PointofService.CreateLineItems(itemBarcode, Inventory);

                Console.WriteLine("**********************");

                // Leave Program or Continue with new shopping basket
                try
                {
                    Console.WriteLine("Would you like to do? Another transaction? Y or press enter to continue or N to Exit");
                    var contd = Console.ReadLine();

                    if (contd.ToUpper() == "N")
                    {
                        userWantsToExit = true;
                    }

                    if (contd.ToUpper() == "Y")
                    {
                        userWantsToExit = false;
                    }
                }
                catch
                {
                    Console.WriteLine("Sorry, please try again and restart the app");
                }
            }
        }

        private static void ReadString(string input, List<LineItem> itemBarcode)
        {
            try
            {
                var qty = Convert.ToInt32(Regex.Match(input.Trim(), @"^\w+").ToString());
                var name = Regex.Match(input.Trim(), @"(?<= )(.*)(?= at)").ToString();
                var price = Convert.ToDecimal(Regex.Match(input.Trim(), @"\d+(\.\d+)?$").ToString());

                itemBarcode.Add(new LineItem { Name = name, Price = price, Quantity = qty });
                Console.Beep();
            }
            catch
            {
                Console.WriteLine("opps thats not a valid item");
            }
        }
    }
}