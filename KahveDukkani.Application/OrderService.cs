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
        // 1. Adresi oluştur
        var address = new Address(request.Street, request.City, request.ZipCode);

        // 2. Siparişi oluştur
        var order = new Order(address);

        // 3. İstekteki ürünleri dön ve ekle
        foreach (var itemRequest in request.Items)
        {
            // Fiyatı veritabanından bulmamız lazım (Basitlik olsun diye şimdilik sabit veriyorum)
            // Normalde: _productRepository.GetPrice(itemRequest.ProductName) yapılır.
            decimal productPrice = 50; // Şimdilik her kahve 50 TL olsun :)

            // Adet kadar ekle (Döngüyle)
            for (int i = 0; i < itemRequest.Quantity; i++)
            {
                // STOK KONTROLÜNÜ BURADA YAPABİLİRSİN (Önceki dersteki mantık)
                var stock = _stockRepository.GetByName(itemRequest.ProductName);
                if (stock != null)
                {
                    stock.DecreaseStock(1);
                }

                order.AddItem(itemRequest.ProductName, productPrice);
            }
        }

        order.ApplyCampaigns();

        // 4. Kaydet
        _repository.Add(order);
        _repository.SaveChanges();
        _stockRepository.SaveChanges(); // Stok düştüyse onu da kaydet

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
        // 1. Repository'den tüm siparişleri, içindeki ürünlerle (Include) beraber çekiyoruz.
        var orders = _repository.GetAll();

        // 2. Domain Nesnesini (Order) -> Transfer Nesnesine (DTO) çeviriyoruz.
        var orderDtos = orders.Select(order => new OrderDto
        {
            Id = order.Id,
            Status = order.Status.ToString(), // Enum'ı string'e çevir

            // Veritabanında olmayan ama DTO'da göstermek istediğimiz hesaplama:
            OriginalPrice = order.Items.Sum(x => x.Price),

            // Domain'den gelen veriler:
            DiscountAmount = order.DiscountAmount,
            TotalPrice = order.TotalPrice, // Bu zaten (Original - Discount) olarak Domain'de hesaplı geliyor

            OrderDate = order.OrderDate,

            // Value Object (Address) içindeki alanları birleştirip tek string yapıyoruz:
            FullAddress = $"{order.ShippingAddress.Street}, {order.ShippingAddress.City} ({order.ShippingAddress.ZipCode})",

            // İç içe listeyi (Items) de çevirmemiz lazım:
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