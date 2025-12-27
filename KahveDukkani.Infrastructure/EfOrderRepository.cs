using KahveDukkani.Application;
using KahveDukkani.Domain;
using KahveDukkani.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveDukkani.Infrastructure
{
    public class EfOrderRepository : IOrderRepository
    {
        private readonly KahveDbContext _context;

        public EfOrderRepository(KahveDbContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.Items)
                .ToList();
        }

        public Order? GetById(Guid id)
        {
            return _context.Orders
                .Include(o => o.Items)
                .FirstOrDefault(o => o.Id == id);
        }
        public void Update(Order order)
        {
            _context.SaveChanges();
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
