using System.Diagnostics;
using System.Runtime.CompilerServices;
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
    public IActionResult Index()
    {
        List<Productos> productos = _productoRepository.GetAll();
        return View(productos);
    }

    public IActionResult Delete(int Id)
    {
        _productoRepository.Delete(Id);
        return RedirectToAction("Index"); // Redirige al listado de productos.
    }
    [HttpGet]
    public IActionResult Update(int Id)
    {
        var producto = _productoRepository.GetById(Id);
        return View(producto);
    }
    [HttpPost]
    public IActionResult Update(int id, Productos producto)
    {
        if (ModelState.IsValid)
        {
            _productoRepository.Update(id, producto);
            return RedirectToAction("Index");
        }
        return View(producto);
    }
    [HttpGet]
    public IActionResult Create(int Id)
    {
        var producto = _productoRepository.GetById(Id);
        return View(producto);
    }
    [HttpPost]
    public IActionResult Create(int Id, Productos producto)
    {
        if (ModelState.IsValid)
        {
            _productoRepository.Created(producto);
            return RedirectToAction("index");
        }
        return View(producto);
    }
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
