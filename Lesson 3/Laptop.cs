using System;

namespace MiniProjectOne
{
    class Laptop : Product
    {
        public Laptop(string office, string brand, string model, decimal price, DateTime purchaseDate) : base(office, brand, model, price, purchaseDate)
        {

        }

        public Laptop()
        {
        }

        public override string ProductType()
        {
            return "Laptop";
        }

    }
}
