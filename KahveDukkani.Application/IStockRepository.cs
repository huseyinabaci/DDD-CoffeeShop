using KahveDukkani.Domain;

namespace KahveDukkani.Application;

public interface IStockRepository
{
    ProductStock? GetByName(string productName);
    void Add(ProductStock stock);
    void SaveChanges();
}