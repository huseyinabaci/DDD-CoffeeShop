namespace KahveDukkani.Application.DTOs;

public class CreateOrderRequest
{
    public string Street { get; set; }
    public string City { get; set; }
    public string ZipCode { get; set; }
    public string CustomerName { get; set; }
    public List<OrderItemRequest> Items { get; set; } 
}

public class OrderItemRequest
{
    public string ProductName { get; set; }
    public int Quantity { get; set; } 
}