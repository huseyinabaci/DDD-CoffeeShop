using System;
using System.Collections.Generic;
using System.Text;

namespace KahveDukkani.Domain
{
    public class ProductStock
    {
        public Guid Id { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }

        public ProductStock(Guid id, string productName, int quantity)
        {
            Id = id;
            ProductName = productName;
            Quantity = quantity;
        }

        private ProductStock(){}

        public void AddStock(int amount) { 
        
            Quantity += amount;

        }

        public void DecreaseStock(int amount) {

            if (amount <= 0)
                throw new Exception("Düşülecek stok miktarı 0'dan büyük olmalı.");

            if (Quantity < amount)
                throw new Exception($"Stok yetersiz! '{ProductName}' ürününden sadece {Quantity} adet kaldı.");

            Quantity -= amount;

        }
    }
}
