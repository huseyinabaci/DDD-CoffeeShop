using KahveDukkani.Application.DTOs;
using KahveDukkani.Domain;
using KahveDukkani.Domain.Events;

namespace KahveDukkani.Application;

public class OrderService
{
    private readonly IOrderRepository _repository;
    private readonly IStockRepository _stockRepository;

    public OrderService(IOrderRepository repository, IStockRepository stockRepository)
    {
        _repository = repository;
        _stockRepository = stockRepository;
    }

    public Guid CreateOrder(CreateOrderRequest request)
    {
        var address = new Address(request.Street, request.City, request.ZipCode);

        var order = new Order(address);

        foreach (var itemRequest in request.Items)
        {
            decimal productPrice = 50;

            for (int i = 0; i < itemRequest.Quantity; i++)
            {
                var stock = _stockRepository.GetByName(itemRequest.ProductName);
                if (stock != null)
                {
                    stock.DecreaseStock(1);
                }

                order.AddItem(itemRequest.ProductName, productPrice);
            }
        }

        order.ApplyCampaigns();

        _repository.Add(order);
        _repository.SaveChanges();
        _stockRepository.SaveChanges();

        return order.Id;
    }

    public Guid CreateOrderWithCoffee(string street, string city, string coffeeName, decimal price)
    {
        var address = new Address(street, city, "34000");

        var order = new Order(address);

        order.AddItem(coffeeName, price);

        order.AddItem("Cookie", 2.50m);

        _repository.Add(order);

        return order.Id;
    }

    public void PayOrder(Guid orderId) {

        var order = _repository.GetById(orderId);

        if (order == null)
            throw new Exception("Sipariş bulunamadı!");

        foreach (var item in order.Items)
        {
            var stock = _stockRepository.GetByName(item.ProductName);

            if (stock == null)
                throw new Exception($"Stokta '{item.ProductName}' diye bir ürün tanımlı değil!");

            stock.DecreaseStock(1);
        }

        order.Pay();

        _stockRepository.SaveChanges();
        _repository.SaveChanges();

        foreach (var domainEvents in order.DomainEvents)
        {
            if(domainEvents is OrderPaidEvent paidEvent)
            {
                SendEmailMock(paidEvent);
            }
        }

    }

    public void AddExtraItem(Guid orderId, string productName, decimal price)
    {
        var order = _repository.GetById(orderId);
        if (order == null) throw new Exception("Sipariş bulunamadı!");

        order.AddItem(productName, price);

        _repository.SaveChanges();
    }

    public List<OrderDto> GetOrders()
    {
        var orders = _repository.GetAll();

        var orderDtos = orders.Select(order => new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(),
            OriginalPrice = order.Items.Sum(x => x.Price),

            DiscountAmount = order.DiscountAmount,
            TotalPrice = order.TotalPrice,

            OrderDate = order.OrderDate,

            FullAddress = $"{order.ShippingAddress.Street}, {order.ShippingAddress.City} ({order.ShippingAddress.ZipCode})",

            Items = order.Items.Select(item => new OrderItemDto
            {
                ProductName = item.ProductName,
                Price = item.Price
            }).ToList()

        }).ToList();

        return orderDtos;
    }

    private void SendEmailMock(KahveDukkani.Domain.Events.OrderPaidEvent e)
    {
        Console.WriteLine($"[MAIL GÖNDERİLDİ] Sipariş No: {e.OrderId} - Tutar: {e.Amount} TL tahsil edildi. Fatura oluşturuluyor...");
    }
}