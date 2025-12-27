using Microsoft.AspNetCore.Mvc;
using KahveDukkani.Application;
using KahveDukkani.Domain;

namespace KahveDukkani.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StocksController : ControllerBase
{
    private readonly IStockRepository _stockRepository;

    public StocksController(IStockRepository stockRepository)
    {
        _stockRepository = stockRepository;
    }

    [HttpPost]
    public IActionResult CreateStock(string productName, int quantity)
    {
        var stock = new ProductStock(Guid.NewGuid(), productName, quantity);
        _stockRepository.Add(stock);
        return Ok("Stok girişi yapıldı.");
    }
}