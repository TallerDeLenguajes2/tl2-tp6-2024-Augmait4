using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Augmait4.Models;

namespace tl2_tp6_2024_Augmait4.Controllers;

public class ProductoController : Controller
{
    private readonly ProductoRepository _productoRepository;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(ILogger<ProductoController> logger)
    {
        _logger = logger;
        _productoRepository = new ProductoRepository("Data Source= BD/Tienda.db;Cache = Shared");
    }
    [HttpGet]
    public IActionResult GetAll(){
        return View (_productoRepository.GetAll());
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
