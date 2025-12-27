using KahveDukkani.Application;
using KahveDukkani.Domain;
using KahveDukkani.Infrastructure.Persistence;

namespace KahveDukkani.Infrastructure;

public class EfStockRepository : IStockRepository
{
    private readonly KahveDbContext _context;

    public EfStockRepository(KahveDbContext context)
    {
        _context = context;
    }

    public ProductStock? GetByName(string productName)
    {
        return _context.ProductStocks.FirstOrDefault(x => x.ProductName == productName);
    }

    public void Add(ProductStock stock)
    {
        _context.ProductStocks.Add(stock);
        _context.SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}