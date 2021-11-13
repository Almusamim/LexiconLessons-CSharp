using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace MiniProjectOne
{
    class ProductService
    {
        public static void Table(List<Product> productList, string searchWord = "")
        {
            productList = productList.OrderBy(p => p.Office).ThenBy(p => p.PurchaseDate).ToList();

            bool isSearch = !string.IsNullOrEmpty(searchWord) && !string.IsNullOrWhiteSpace(searchWord);
            if (isSearch)
            {
                searchWord = searchWord.ToLower().Trim();
                productList = productList.Where(p => p.Brand.ToLower().Contains(searchWord) || p.Model.ToLower().Contains(searchWord) || p.Office.ToLower().Contains(searchWord)).ToList();
            }

            int padding = 18;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n\t" + "Category".PadRight(padding) + "Brand".PadRight(padding) + "Model".PadRight(padding) + "Purchase Date".PadRight(padding) + "Office".ToString().PadRight(padding) + "Price".ToString());
            Console.ResetColor();
            Console.WriteLine();
            foreach (Product p in productList)
            {
                p.SetColorForOld();
                Console.Write("\t" + p.Name.PadRight(padding));
                Console.Write(p.Brand.PadRight(padding));
                Console.Write(p.Model.PadRight(padding));
                Console.Write(p.PurchaseDate.ToString("dd MMM yyyy").PadRight(padding));
                Console.Write(p.Office.ToString().PadRight(padding));
                Console.Write(p.CurrencyConvert().price);
                Console.WriteLine("\n");

                Console.ResetColor();
            }

            if(productList.Count > 0)
            {
                Util.PrintTxt("\t-------------------------------------------------------------------------------------------------------\n", "DarkGray");
                Util.PrintTxt($"\tExchange Rates Updated: {productList[0].CurrencyConvert().latestUpdate} | Total Products: {productList.Count} \n\n", "DarkGray");
            } else
            {
                Util.PrintTxt("\t\tNO PRODUCT FOUND!\n\n", "Red");
                Util.PrintTxt("\t\tPress 'D' to add demo data.\n\n", "Yellow");
            }
        }

        public static List<object> New()
        {
            List<object> list = new List<object>();
            //Type productType = categoryMenu();
            Type productType = typeof(Product);

            var instance = Activator.CreateInstance(productType);
            var properties = productType.GetProperties()
                .OrderBy(p => p.GetCustomAttributes(typeof(DisplayAttribute), true)
                .Cast<DisplayAttribute>()
                .Select(a => a.Order)
                .FirstOrDefault());

            foreach (PropertyInfo prop in properties)
            {
                while (true)
                {
                    try
                    {
                        var attribute = productType.GetProperty(prop.Name).GetCustomAttribute<DisplayAttribute>();

                        if(attribute!= null)
                        {
                            if (!string.IsNullOrEmpty(attribute.Name)) { Util.PrintTxt($"{attribute.Name}: ", "Green"); }
                        }
                        else
                        {
                            Util.PrintTxt($"{prop.Name}: ", "Green");
                        }

                        string[] offices = { "Malmö", "Kuala Lumpur", "Dallas" };
                        string[] Categories = { "Laptop", "Mobile Phone", "IoT" };
                        string userInput = prop.Name == "Office" ? Util.NumberMenu(offices, "Office Location")
                            : prop.Name == "Name" ? Util.NumberMenu(Categories, "Category")
                            : userInput = Console.ReadLine();

                        Console.WriteLine();

                        PropertyInfo propertyInfo = productType.GetProperty(prop.Name);
                        dynamic dynamicValue = Convert.ChangeType(userInput, propertyInfo.PropertyType);

                        bool isValid = Util.ValidaterProperty(dynamicValue, prop.Name, instance);
                        if (isValid)
                        {
                            prop.SetValue(instance, dynamicValue);
                            // Break and go to next prop
                            break;
                        }
                    }
                    catch (ValidationException ex)
                    {
                        Util.PrintTxt($"\n - {ex.Message}\n\n", "Red");
                    }
                    catch (Exception e)
                    {
                        Util.PrintTxt($"\n - {e.Message}\n\n", "DarkRed");
                    }
                }
            }

            list.Add(instance);
            Console.WriteLine();
            return list;
        }

        public static void DemoData(List<Product> list)
        {
            bool exist = !list.Any(p => p.Model == "Thinkpad P1" && p.Brand == "Lenovo");
            if (exist)
            {
                DateTime now = DateTime.Now;

                list.Add(new Product("Laptop", "Malmö", "Lenovo", "ThinkPad X1 Nano", 2400.00m, now.AddMonths(-30)));
                list.Add(new Product("Mobile Phone", "Malmö", "Ericsson", "T28", 100.00m, now.AddMonths(-66).AddDays(5)));
                list.Add(new Product("Mobile Phone", "Malmö", "Nokia", "3200", 90.00m, now.AddMonths(-66).AddDays(19)));
                list.Add(new Product("Mobile Phone", "Dallas", "Nokia ", "3210", 30.00m, now.AddMonths(-34)));
                list.Add(new Product("Laptop", "Dallas", "Framework", "Gen 1", 2900.00m, new DateTime(2020, 01, 03)));
                list.Add(new Product("Laptop", "Dallas", "IBM", "Thinkpad A20p", 1900.00m, now.AddMonths(-34).AddDays(4)));
                list.Add(new Product("Laptop", "Kuala Lumpur", "Dell", "XPS 13", 1699.00m, now.AddMonths(-31).AddDays(7)));
                list.Add(new Product("Laptop", "Kuala Lumpur", "Lenovo", "Thinkpad P1", 3750.90m, now.AddDays(-4)));
                list.Add(new Product("IoT", "Kuala Lumpur", "Raspberry", "Pi 4", 65.00m, now.AddMonths(-12).AddDays(4)));

                Util.PrintTxt("\tDemo data added.\n", "Yellow");
            }
            else
            {
                Util.PrintTxt("\n\tDemo Data Alreay Added.\n", "Red");
            }
        }
    }
}
