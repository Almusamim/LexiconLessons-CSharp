using System;
using System.Linq;

namespace Produktlista
{
    class Program
    {
        static void Main(string[] args)
        {
            LexiconHeader();
            Console.WriteLine("Skriv in produkter. Avsluta med att skriva 'exist'");
            Console.WriteLine();

            // loop through the porduct list
            ProductList();

            Console.WriteLine("Good bye =(");
            Console.Write("Tryck på enter för att stänga detta fönster. . .");
            Console.Read();
        }

        private static void ProductList()
        {
            string[] productList = new string[0];
            int index = 0;

            while (true)
            {
                Console.Write("Ange product: ");
                string product = Console.ReadLine().Trim();

                if (product.ToLower() == "exit")
                    break;

                bool isValid = ValidateName(product);
                if (isValid)
                {
                    Array.Resize(ref productList, index + 1);
                    productList[index] = product;
                    index++;
                }
            }

            DisplayList(productList, "produkter");
        }

        private static void DisplayList(string[] list, string name)
        {
            if(list.Length > 0)
            {
                Array.Sort(list);
                Console.WriteLine();
                Console.WriteLine($"Du angav följande {name}");
                Console.WriteLine("---------------------");
                Console.WriteLine();
            
                foreach (string item in list)
                {
                    Console.WriteLine($"\t* {item}");
                }

                Console.WriteLine();
                Console.WriteLine("---------------------");
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Du angav inga {name}");
                Console.ResetColor();
            }
        }
       
        private static bool ValidateName(string name)
        {
            //The whole validation could probably be validated with regex
            // but for practice purpose, we are not going to do it with regex.

            if (string.IsNullOrEmpty(name))
                return ErrorMsg("Du får inte ange ett tomt värde");

            else if (!name.Contains("-"))
                return ErrorMsg("Det måste finnas '-' i produkt namnet.\nSom till exempel 'produktNamn-12345'.");

            else if (name.Contains("-"))
            {
                string[] splitName = name.Split("-");
                bool isLetter = splitName[0].All(char.IsLetter);
                bool isInt = int.TryParse(splitName[1], out int numberValue);

                if (!isLetter || string.IsNullOrEmpty(splitName[0]))
                    return ErrorMsg("Felaktigt format på vänstra delen av produkten.");

                if (isInt && !(numberValue >= 200 && numberValue <= 500))
                    return ErrorMsg("Den numeriska delen måste vara mellan 200 och 500.");

                if (!isInt)
                    return ErrorMsg("Felaktigt format på högra delen av produkten.");
            }

            return true;
        }

        private static bool ErrorMsg(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
            return false;
        }

        // Just playing arround with console decoration, using ASCII Art
        private static void LexiconHeader()
        {
            Console.Title = "Lexicon :: Lesson #1";
            string title = @"

    ██╗     ███████╗██╗  ██╗██╗ ██████╗ ██████╗ ███╗   ██╗
    ██║     ██╔════╝╚██╗██╔╝██║██╔════╝██╔═══██╗████╗  ██║
    ██║     █████╗   ╚███╔╝ ██║██║     ██║   ██║██╔██╗ ██║
    ██║     ██╔══╝   ██╔██╗ ██║██║     ██║   ██║██║╚██╗██║
    ███████╗███████╗██╔╝ ██╗██║╚██████╗╚██████╔╝██║ ╚████║
    ╚══════╝╚══════╝╚═╝  ╚═╝╚═╝ ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝

    +-+-+-+-+-+-+ +-+-+
    |L|e|s|s|o|n| |#|1|
    +-+-+-+-+-+-+ +-+-+

";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(title);
            Console.ResetColor();
        }
    }
}
