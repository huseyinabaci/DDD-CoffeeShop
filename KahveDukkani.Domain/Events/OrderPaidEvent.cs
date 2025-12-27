using System;
using System.Collections.Generic;
using System.Text;

namespace KahveDukkani.Domain.Events
{
    public class OrderPaidEvent: IDomainEvent
    {
        public Guid OrderId { get; }
        public decimal Amount { get; }

        public OrderPaidEvent(Guid orderId, decimal amount)
        {
            OrderId = orderId;
            Amount = amount;
        }
    }
}