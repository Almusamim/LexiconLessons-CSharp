using System;

namespace MiniProjectOne
{
    class MobilePhone : Product
    {
        public string Network { get; set; }

        public MobilePhone(string office, string brand, string model, decimal price, DateTime purchaseDate): base(office, brand, model, price, purchaseDate)
        {
        }

        public MobilePhone()
        {
        }

        public override string ProductType()
        {
            return "Mobile Phone";
        }
    }
}
