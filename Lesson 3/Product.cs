using System;
using System.ComponentModel.DataAnnotations;

namespace MiniProjectOne
{
    class Product
    {
        public Product(string office, string brand, string model, decimal price, DateTime purchaseDate)
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
        public DateTime PurchaseDate { get; set; }

        public virtual string ProductType()
        {
            return "Product";
        }
    }
}
