using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniProjectOne
{
    class Program
    {
        static void Main(string[] args)
        {
            Util.LexiconHeader();

            ConsoleKeyInfo cki;
            Util.Menu();

            List<Product> productList = new List<Product>();
            while (true)
            {
                cki = Console.ReadKey(false);
                Util.ClearConsole();

                // add product to the list with validations
                if (cki.Key == ConsoleKey.Enter || cki.Key == ConsoleKey.N)
                {
                    productList.AddRange(ProductService.New().Cast<Product>().ToList());

                    // Show data table after adding new product
                    Util.ClearConsole();
                    cki = new ConsoleKeyInfo((char)ConsoleKey.L, ConsoleKey.L, false, false, false);
                }

                // List all products in data table
                if (cki.Key == ConsoleKey.L)
                {
                    ProductService.Table(productList);
                    Util.PrintTxt($"\tPress 'Enter' to add new product.", "DarkGray");
                }

                // Search
                if (cki.Key == ConsoleKey.S)
                {
                    Util.PrintTxt("Search:", "Blue");
                    string searchValue = Console.ReadLine();
                    ProductService.Table(productList, searchValue);
                }

                // Dump demo/sample data to the product list
                if (cki.Key == ConsoleKey.D)
                {
                    ProductService.DemoData(productList);
                }

                if (cki.Key == ConsoleKey.Escape || cki.Key == ConsoleKey.Q)
                {
                    Util.ExitApp();
                    break;
                }
            }
        }
    }
}
