using System;
using System.Collections.Generic;
using System.Linq;

namespace CheckpointTwo
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Product> productList = new List<Product>();

            Util.LexiconHeader();
            Util.Menu();

            ConsoleKeyInfo cki;

            while (true)
            {
                var productsOrderByPrice = productList.OrderBy(product => product.Price).ToList();

                cki = Console.ReadKey(false);
               
                // Dump demo/sample data to the product list
                if (cki.Key == ConsoleKey.D)
                {
                    //Refresh the Console and keep the 'Keys' Menu on the top
                    Util.ClearConsole();

                    Util.DemoData(productList);
                    // Redirect to table list
                    //cki = new ConsoleKeyInfo((char)ConsoleKey.L, ConsoleKey.L, false, false, false);
                }

                // User input to add product to the list with validations
                if (cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.N)
                {
                    Util.ClearConsole();
                    productList.AddRange(Util.AddProduct());
                }
                

                // Show Product Table 
                if (cki.Key == ConsoleKey.L)
                {
                    Util.ClearConsole(); 
                    Util.ProductTable(productsOrderByPrice);
                }

                // Search
                if (cki.Key == ConsoleKey.S)
                {
                    Util.ClearConsole();

                    Util.PrintTxt("Search:", "Blue");
                    string searchValue = Console.ReadLine();
                    Util.ProductTable(productsOrderByPrice, searchValue);
                }

                if (cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.Q)
                {
                    ExitApp();
                    break;
                }
            }
        }

        static private void ExitApp()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\n Press any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }
    }
}
