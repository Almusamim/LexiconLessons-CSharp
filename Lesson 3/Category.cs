using System.ComponentModel.DataAnnotations;

namespace MiniProjectOne
{
    class Category
    {
        public Category(string name = "Products")
        {
            Name = name;
        }

        [Required]
        [Display(Name = "Categpry Name", Order = -2,
        Prompt = "", Description = "")]
        public string Name { get; set; }

    }
}
