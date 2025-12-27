using KahveDukkani.Domain;

namespace KahveDukkani.Application;
public interface IOrderRepository
{
    void Add(Order order);
    List<Order> GetAll();
    Order? GetById(Guid id);
    void SaveChanges();
}