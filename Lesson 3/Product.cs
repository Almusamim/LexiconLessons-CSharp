using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace MiniProjectOne
{
    class Product : Category
    {
        public Product(string categoryName, string office, string brand, string model, decimal price, DateTime purchaseDate) : base(categoryName)
        {
            Office = office ?? throw new ArgumentNullException(nameof(office));
            Brand = brand ?? throw new ArgumentNullException(nameof(brand));
            Model = model ?? throw new ArgumentNullException(nameof(model));
            Price = price;
            PurchaseDate = purchaseDate;
        }

        public Product()
        {
        }

        [Required]
        [Display(Order = -2)]
        public string Office { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }

        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Price; Maximum Two Decimal Points.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "The Price must be from 0,1 and up.\nMake sure to use local settings format")]
        [Required]
        public decimal Price { get; set; }

        [Required]
        [Display(Name = "Purchase Date", Order = 3)]
        public DateTime PurchaseDate { get; set; }

        public (string price, DateTime latestUpdate) CurrencyConvert()
        {
            var (MYR, SEK, latestUpdate) = Client.Rates();

            string price;
            switch (Office)
            {
                case "Malmö": price = (Price * SEK).ToString("C", CultureInfo.GetCultureInfo("sv-SE"));
                    break;
                case "Kuala Lumpur": price = (Price * MYR).ToString("C", CultureInfo.GetCultureInfo("en-MY"));
                    break;
                default: price = Price.ToString("C", CultureInfo.GetCultureInfo("en-US"));
                    break;
            }

            return (price, latestUpdate);
        }

        public void SetColorForOld()
        {
            DateTime now = DateTime.Now;
            DateTime thirtyThreeMonthsBack = now.AddMonths(-33);
            DateTime thirtyMonthsBack = now.AddMonths(-30);

            if (thirtyMonthsBack > PurchaseDate)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            if (thirtyThreeMonthsBack > PurchaseDate)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
        }
    }
}
