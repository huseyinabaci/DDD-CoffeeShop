namespace KahveDukkani.Application.DTOs;

public class OrderDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } // "Odendi", "Olusturuldu" vs.

    // --- Fiyat Bilgileri ---
    public decimal OriginalPrice { get; set; }  // İndirimsiz ham tutar
    public decimal DiscountAmount { get; set; } // Yapılan indirim
    public decimal TotalPrice { get; set; }     // Müşterinin ödediği son tutar

    public DateTime OrderDate { get; set; }
    public string FullAddress { get; set; }     // Formatlanmış adres

    public List<OrderItemDto> Items { get; set; } // İçindeki ürünler
}

public class OrderItemDto
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
}