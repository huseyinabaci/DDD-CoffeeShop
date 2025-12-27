using KahveDukkani.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace KahveDukkani.Infrastructure.Persistence
{
    public class KahveDbContext : DbContext
    {
        public KahveDbContext(DbContextOptions<KahveDbContext> options) : base(options)
        {
        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<ProductStock> ProductStocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(b =>
            {
                b.HasKey(o => o.Id);

                b.OwnsOne(o => o.ShippingAddress, a =>
                {
                    a.Property(p => p.Street).HasColumnName("Street");
                    a.Property(p => p.City).HasColumnName("City");
                    a.Property(p => p.ZipCode).HasColumnName("ZipCode");
                });

            });

            modelBuilder.Entity<OrderItem>(b =>
            {
                b.HasKey(i => i.Id);
                b.Property(i => i.Price).HasColumnType("decimal(18,2)");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
