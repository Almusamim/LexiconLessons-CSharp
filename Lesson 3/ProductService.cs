using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
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
                string price;
                if (p.Office == "Malmö")
                {
                    price = (p.Price * 8.71m).ToString("C", CultureInfo.GetCultureInfo("sv-SE"));
                }
                else if (p.Office == "Kuala Lumpur")
                {
                    price = (p.Price * 4.16m).ToString("C", CultureInfo.GetCultureInfo("en-MY"));
                }
                else
                {
                    price = p.Price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
                }

                DateTime now = DateTime.Now;
                DateTime thirtyThreeMonthsBack = now.AddMonths(-33);
                DateTime thirtyMonthsBack = now.AddMonths(-30);

                if (thirtyMonthsBack > p.PurchaseDate)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                }
                if (thirtyThreeMonthsBack > p.PurchaseDate)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }

                Console.Write("\t" + p.ProductType().PadRight(padding));
                Console.Write(p.Brand.PadRight(padding));
                Console.Write(p.Model.PadRight(padding));
                Console.Write(p.PurchaseDate.ToString("dd MMM yyyy").PadRight(padding));
                Console.Write(p.Office.ToString().PadRight(padding));
                Console.Write(price);
                Console.WriteLine("\n");

                Console.ResetColor();
            }
        }

        public static List<object> New()
        {
            List<object> list = new List<object>();
            Type productType = categoryMenu();

            var properties = productType.GetProperties();
            var instance = Activator.CreateInstance(productType);

            foreach (var prop in properties)
            {
                while (true)
                {
                    try
                    {
                        Console.WriteLine();
                        string userInput;

                        // Skipping property
                        if (prop.Name == "Network")
                        {
                            break;
                        }

                        if (prop.Name != "Office")
                        {
                            Util.PrintTxt($"{prop.Name}: ", "Green");
                        }

                        userInput = prop.Name == "Office" ? locationMenu() : userInput = Console.ReadLine();

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

            static string locationMenu()
            {
                Util.PrintTxt($"Choose office location by picking corresponding number.\n\n", "DarkYellow");
                Util.PrintTxt($"1.) Malmö", "Blue");
                Util.PrintTxt($"2.) Kuala Lumpur", "Yellow");
                Util.PrintTxt($"3.) Dallas", "DarkBlue");
                ConsoleKeyInfo cki = Console.ReadKey(false);

                while (true)
                {
                    Console.WriteLine();
                    if (ConsoleKey.D1 == cki.Key)
                    {
                        return "Malmö";
                    }
                    else if (ConsoleKey.D2 == cki.Key)
                    {
                        return "Kuala Lumpur";
                    }
                    else if (ConsoleKey.D3 == cki.Key)
                    {
                        return "Dallas";
                    }
                    else
                    {
                        Util.PrintTxt("\n\n This field is required, please choose corresponding number ", "Red");
                        cki = Console.ReadKey(false);
                    }
                }
            } // end of locationMenu()

            static Type categoryMenu()
            {
                Util.PrintTxt($"Choose a category by picking corresponding number.\n\n", "DarkYellow");
                Util.PrintTxt($"1.) Laptop", "Blue");
                Util.PrintTxt($"2.) Mobile Phone", "Yellow");

                ConsoleKeyInfo cki = Console.ReadKey(false);
                while (true)
                {
                    Console.WriteLine();
                    if (ConsoleKey.D1 == cki.Key)
                    {
                        return typeof(Laptop);
                    }
                    else if (ConsoleKey.D2 == cki.Key)
                    {
                        return typeof(MobilePhone);
                    }
                    else
                    {
                        Util.PrintTxt("\n\n This field is required, please choose corresponding number ", "Red");
                        cki = Console.ReadKey(false);
                    }
                }
            } // end of categoryMenu()

        } // end of New()

        public static void DemoData(List<Product> list)
        {
            bool exist = !list.Any(p => p.Model == "Thinkpad P1" && p.Brand == "Lenovo");
            Console.WriteLine(exist);

            if (exist)
            {
                DateTime now = DateTime.Now;

                list.Add(new Laptop("Kuala Lumpur", "Dell", "XPS 13", 1699.00m, now.AddMonths(-31)));
                list.Add(new MobilePhone("Malmö", "Ericsson", "T28", 100.00m, now.AddMonths(-66).AddDays(5)));
                list.Add(new Laptop("Dallas", "Framework", "Gen 1", 2900.00m, new DateTime(2020, 01, 03)));
                list.Add(new Laptop("Dallas", "IBM", "Thinkpad A20p", 1900.00m, now.AddMonths(-34)));
                list.Add(new Laptop("Kuala Lumpur", "Lenovo", "Thinkpad P1", 3750.90m, now));
                list.Add(new MobilePhone("Malmö", "Nokia", "3200", 90.00m, now.AddMonths(-66).AddDays(19)));
                list.Add(new MobilePhone("Malmö", "Nokia ", "3210", 30.00m, now.AddMonths(-34)));
                list.Add(new Laptop("Dallas", "Lenovo", "ThinkPad X1 Nano", 2400.00m, now.AddMonths(-30)));

                Util.PrintTxt("\tDemo data added.\n", "Yellow");
            }
            else
            {
                Util.PrintTxt("\n\tDemo Data Alreay Added.\n", "Red");
            }
        }

    }
}
