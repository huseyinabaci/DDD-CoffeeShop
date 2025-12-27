using KahveDukkani.Domain.Events;

namespace KahveDukkani.Domain;

public class Order
{
    public Guid Id { get; private set; }
    public Address ShippingAddress { get; private set; }
    public DateTime OrderDate { get; private set; }
    public OrderStatus Status { get; private set; }
    public decimal DiscountAmount { get; private set; }

    private readonly List<OrderItem> _items = new();
    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();
    public decimal TotalPrice => _items.Sum(item => item.Price) - DiscountAmount;

    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Order(Address address)
    {

        Id = Guid.NewGuid();
        OrderDate = DateTime.Now;
        ShippingAddress = address;
        Status = OrderStatus.Olusturuldu;

    }

    private Order()
    {
    }

    public void AddItem(string productName, decimal price)
    {
        if (Status != OrderStatus.Olusturuldu)
        {
            throw new Exception("Ödemesi alınmış veya kargolanmış siparişe ürün ekleyemezsiniz!");
        }

        if (price <= 0)
            throw new Exception("Bedava ürün veremeyiz veya fiyat eksi olamaz!");

        if (_items.Count >= 10)
            throw new Exception("Bir siparişte en fazla 10 ürün olabilir! (İş Kuralı)");

        var newItem = new OrderItem(productName, price);
        _items.Add(newItem);

    }

    public void Pay() {

        if (Status == OrderStatus.Odendi) return;

        Status = OrderStatus.Odendi;

        _domainEvents.Add(new OrderPaidEvent(this.Id, this.TotalPrice));
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }

    public void ApplyCampaigns()
    {
        if (Status != OrderStatus.Olusturuldu) return;

        var subTotal = _items.Sum(i => i.Price);

        if(subTotal >= 500)
        {
            DiscountAmount = subTotal * 0.10m;

        }
        else {
            DiscountAmount = 0;
        }
    }
}
