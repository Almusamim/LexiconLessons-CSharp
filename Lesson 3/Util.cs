using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MiniProjectOne
{
    class Util
    {
        public static bool ValidaterProperty<T>(T value, string propertyName, object obj)
        {
            var context = new ValidationContext(obj) { MemberName = propertyName };
            var results = new List<ValidationResult>();
            bool valid = Validator.TryValidateProperty(value, context, results);

            if (!valid)
            {
                foreach (var error in results)
                {
                    throw new ValidationException(error.ErrorMessage);
                }
                return false;
            }
            return true;
        }
        
        public static void PrintTxt(string txt, string color = "White")
        {
            Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color, true);
            Console.Write($" {txt} ");
            Console.ResetColor();
        }

        public static void Menu()
        {
            Console.WriteLine();

            PrintTxt("Press");
            PrintTxt("'L' to open product List", "Cyan");
            PrintTxt("'S' to Search", "Yellow");
            PrintTxt("'D' to add sample data\n", "DarkGray");
            Console.WriteLine();

            PrintTxt("'Enter' to add product", "Green");
            PrintTxt("||");
            PrintTxt("'Esc' to exit:", "Red");
            Console.WriteLine("\n");
        }

        public static string NumberMenu(string[] array, string name = "field")
        {
            Util.PrintTxt($"\n\nChoose {name} by picking corresponding number.\n\n", "DarkYellow");

            // Generate a random color.  
            Random rand = new Random();
            string[] colors = { "Red", "DarkRed", "Blue", "DarkYellow", "Yellow", "Green", "DarkGreen", "DarkCyan", "Cyan", "Magenta" };

            for (int i = 0; i < array.Length; i++)
            {
                int indexColor = rand.Next(colors.Length);

                Util.PrintTxt($"{i + 1}.) {array[i]}", colors[indexColor]);
            }

            ConsoleKeyInfo cki = Console.ReadKey(false);
            int key;

            while (true)
            {
                if (!int.TryParse(cki.KeyChar.ToString(), out key) || key > array.Length || key <= 0)
                {
                    Util.PrintTxt($"\n\n This field is required, please choose corresponding number between 1 - {array.Length}", "Red");
                    cki = Console.ReadKey(false);
                }

                for (int i = 0; i < array.Length; i++)
                {
                    if (key == i + 1)
                    {
                        return array[i];
                    }
                }
            }
        }

        public static void ClearConsole()
        {
            Console.Clear();
            Menu();
        }

        public static void ExitApp()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n\n Press any key to exit...");
            Console.ReadKey(true);
            Environment.Exit(0);
        }

        public string ToUpper(string value)
        {
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        //JUST FOR FUN, LESSON 2
        public static void LexiconHeader(string color = "Red", string secondColor = "Gray")
        {
            Console.Title = "Lexicon :: Lesson #3";
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
    |L|e|s|s|o|n| |#|3|
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