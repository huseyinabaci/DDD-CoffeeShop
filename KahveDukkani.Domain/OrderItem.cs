using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace KahveDukkani.Domain
{
    public class OrderItem
    {
        public Guid Id { get; private set; }
        public string ProductName { get; private set; }
        public decimal Price { get; private set; }

        internal OrderItem(string productName, decimal price)
        {
            Id = Guid.NewGuid();
            ProductName = productName;
            Price = price;
        }
    }
}
