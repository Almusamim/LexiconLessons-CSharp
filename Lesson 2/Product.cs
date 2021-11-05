using System;
using System.ComponentModel.DataAnnotations;

namespace CheckpointTwo
{
    class Product
    {
        //private string category;
        //private string Name;
        //private decimal price;

        [Required]
        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$", ErrorMessage ="Numbers and special characters are not allowed in the name.")]
        public string Category { get; set; }

        [StringLength(maximumLength: 50, MinimumLength = 3)]
        [Required(ErrorMessage = "Product Name Required")]
        public string Name { get; set; }

        [RegularExpression(@"^\d+.?\d{0,2}$", ErrorMessage = "Invalid Price; Maximum Two Decimal Points.")]
        [Range(0.1, double.MaxValue, ErrorMessage = "The Price must be from 0,1 and up.\nMake sure to use local settings format")]
        [Required]
        public decimal Price { get; set; }

        public Product(string name, string category, decimal price)
        {
            Name = name;
            Category = category;
            Price = price;
        }

        public Product()
        {
        }
    }
}
