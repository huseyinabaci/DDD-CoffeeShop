using KahveDukkani.Application;
using KahveDukkani.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace KahveDukkani.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _service;

    public OrdersController(OrderService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderRequest request)
    {
        try
        {
            var orderId = _service.CreateOrder(request);
            return Ok(new { Message = "Sipariþ alýndý!", OrderId = orderId });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetOrders());
    }

    [HttpPost("{id}/pay")]
    public IActionResult Pay(Guid id)
    {
        try
        {
            _service.PayOrder(id);
            return Ok("Ödeme alýndý, sipariþ kilitlendi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/add-item")]
    public IActionResult AddItem(Guid id, string productName, decimal price)
    {
        try
        {
            _service.AddExtraItem(id, productName, price);
            return Ok("Ürün eklendi.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}