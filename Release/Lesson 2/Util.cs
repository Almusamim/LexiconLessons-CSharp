using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CheckpointTwo
{
    class Util
    {
        public static void Menu()
        {
            Console.WriteLine();

            PrintTxt("Press");
            PrintTxt("'L' to open the product List", "Cyan");
            PrintTxt("'S' to Search", "Yellow");
            PrintTxt("'D' to add demo data\n", "DarkGray");
            Console.WriteLine();

            PrintTxt("'Enter' to add a new product", "Green");
            PrintTxt("||");
            PrintTxt("'Esc' to exit:", "Red");
            //PrintTxt("to exit:");

            Console.WriteLine("\n");
            Console.ResetColor();

        }

        public static void ClearConsole()
        {
            Console.Clear();
            Menu();
        }

        public static void PrintTxt(string txt, string color = "White")
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);        
            Console.Write($" {txt} ");
            Console.ResetColor();
        }

        // Notes: Improvements for cleaner code is to use Nuget package: Console Table
        public static void ProductTable(List<Product> productList, string searchWord = "")
        {
            bool isSearch = !string.IsNullOrEmpty(searchWord) && !string.IsNullOrWhiteSpace(searchWord);
            if (isSearch)
            {
                searchWord = searchWord.ToLower().Trim();
                productList = productList.Where(p => p.Name.ToLower().Contains(searchWord) || p.Category.ToLower().Contains(searchWord)).ToList();
            }

            decimal sumPrice = productList.Sum(product => product.Price);
            int count = productList.Count;

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine("\tNAME".PadRight(25) + "CATEGORY".PadRight(25) + "PRICE");
            Console.WriteLine("\t" + new string('-', 75) + "\n");
            Console.ResetColor();

            if (count == 0)
            {
                PrintTxt("\t\tNO PRODUCT FOUND!\n\n\n", "Red");
                PrintTxt("\t\tPress 'Enter' to add new product\n\n", "Yellow");
                PrintTxt("\t\tOr press 'D' to add demo data.\n\n", "Yellow");
            }

            foreach (Product product in productList)
            {
                // Hightlight searched word
                if (isSearch && product.Name.ToLower().Trim().Contains(searchWord))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                Console.Write("\t" + product.Name.PadRight(25));
                Console.ResetColor();

                if (isSearch && product.Category.ToLower().Trim().Contains(searchWord))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write(product.Category.PadRight(25));
                Console.ResetColor();

                Console.Write(product.Price + "\n\n");
            }

            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine();
            Console.WriteLine("\t" + new string('-', 75));
            Console.WriteLine($"\tPrice Sum: {sumPrice}");
            Console.WriteLine($"\tTotal Products: {count}");
          
            if (isSearch)
            {
                Console.WriteLine($"\tSearch Keyword: {searchWord}");
            }
            Console.ResetColor();
        }

        public static List<Product> AddProduct(List<Product> productList)
        {
            Product productObj = new();

            string[] properties = { "Name", "Category", "Price" };
            foreach (string prop in properties)
            {
                // Quit the loop when user input for each property is validated
                while (true)
                {
                    try
                    {
                        PrintTxt($"Add {prop}: ");
                        string userInput = Console.ReadLine();

                        // Parse price into decimal
                        decimal decimalValue;
                        bool isDecimal = decimal.TryParse(userInput, out decimalValue);
                        decimalValue = isDecimal ? decimalValue : decimalValue;

                        // Validating property
                        bool isValid = ValidaterProperty(userInput, prop, productObj, decimalValue);
                        if (isValid)
                        {
                            // Set value for each property, Note: some improvements needed here to make it more dynamic
                            switch (prop)
                            {
                                case "Name": productObj.Name = userInput; break;
                                case "Category": productObj.Category = userInput; break;
                                case "Price": productObj.Price = decimalValue; break;
                            }

                            // If succesful break the while loop
                            break;
                        }
                    }
                    catch (ValidationException ex)
                    {
                        PrintTxt($"\n - {ex.Message}\n\n", "Red");
                    }
                }
            }
            productList.Add(new Product(productObj.Name, productObj.Category, productObj.Price));
            PrintTxt($"\n  - Product '{productObj.Name}' successfully added.\n\n", "Green");
            PrintTxt($"- Press the 'L' key to view the product list.\n\n", "Yellow");
            PrintTxt($"- Or", "Yellow"); PrintTxt($"'Enter'", "Green"); PrintTxt($"to add another product.", "Yellow");

            return productList;
        }

        // Validating DataAnnotations by each property.
        // Note: Some improvement to make the 'value' param work with different data types to clean the code further more from all conditions and params.
        public static bool ValidaterProperty(string value, string propertyName, object obj, decimal decimalValue)
        {
            var context = new ValidationContext(obj) { MemberName = propertyName };
            var results = new List<ValidationResult>();

            bool valid = (propertyName != "Price") ?
                Validator.TryValidateProperty(value, context, results) :
                Validator.TryValidateProperty(decimalValue, context, results);

            if (!valid)
            {
                // Throw all errors
                foreach (var error in results)
                {
                    throw new ValidationException($"{error}");
                }
                return false;
            }
            return true;
        }

        public static void DemoData(List<Product> productList)
        {
            // In real world app, we would have used an unique ID :)
            bool exist = !productList.Any(p => p.Name == "Thinkpad P1" && p.Category == "Laptop");
            if (exist)
            {
                productList.Add(new Product("Thinkpad P1", "Laptop", 3750.90m));
                productList.Add(new Product("Bose QC35", "Headphones", 245.00m));
                productList.Add(new Product("Framework", "Laptop", 2340.40m));
                productList.Add(new Product("ThinkPad X1 Nano", "Laptop", 2400.00m));
                productList.Add(new Product("Raspberry Pi 4", "IoT", 99.00m));

                PrintTxt("\tDemo data added.\n", "Yellow");
            }
            else
            {
                PrintTxt("\n\tDemo Data Alreay Added.\n", "Red");
            }
            PrintTxt("\n\tPress 'L' to view Product Table.\n", "Green");
        }

        public static string FirstLetterToUpper(string str)
        {
            if (str == null || str.Length < 1)
                return null;

            return (char.ToUpper(str[0]) + str.Substring(1)).ToString();
        }

        //JUST FOR FUN, LESSON 2
        public static void LexiconHeader(string color = "Red", string secondColor = "Gray")
        {
            Console.Title = "Lexicon :: Lesson #2";
            string title = @"

    ██╗     ███████╗██╗  ██╗██╗ ██████╗ ██████╗ ███╗   ██╗
    ██║     ██╔════╝╚██╗██╔╝██║██╔════╝██╔═══██╗████╗  ██║
    ██║     █████╗   ╚███╔╝ ██║██║     ██║   ██║██╔██╗ ██║
    ██║     ██╔══╝   ██╔██╗ ██║██║     ██║   ██║██║╚██╗██║
    ███████╗███████╗██╔╝ ██╗██║╚██████╗╚██████╔╝██║ ╚████║
    ╚══════╝╚══════╝╚═╝  ╚═╝╚═╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝
    Application Created By Hadi.zakzouk@gmail.com
";
            string subTitle = @"
    +-+-+-+-+-+-+ +-+-+
    |L|e|s|s|o|n| |#|2|
    +-+-+-+-+-+-+ +-+-+
";

            //Console.ForegroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            Console.Write(title);
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), secondColor, true);
            Console.WriteLine(subTitle);
            Console.ResetColor();
        }
    }
}
