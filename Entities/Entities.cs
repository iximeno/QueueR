using System;

namespace UnitX
{
    public class Product
    {
        public Guid Id { get; set; }

        public DateTime? SoldDate { get; set; }

        public Product(Guid id)
        {
            Id = id;
        }

        public bool Sold(DateTime soldDate)
        {
            if (SoldDate == null)
            {
                SoldDate = soldDate;
                return true;
            }
            else
                return false;
        }
    }
}
