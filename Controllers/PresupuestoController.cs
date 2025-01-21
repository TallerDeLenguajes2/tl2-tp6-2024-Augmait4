using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Augmait4.Models;

namespace tl2_tp6_2024_Augmait4.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestosRepository _presupuestoRepository;
    private readonly ProductoRepository _productoRepository;
    private readonly ILogger<PresupuestoController> _logger;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        _presupuestoRepository = new PresupuestosRepository("Data Source= BD/Tienda.db;Cache = Shared");
        _productoRepository = new ProductoRepository("Data Source = BD/Tienda.db;Cache = Shared");
    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> Presupuesto = _presupuestoRepository.GetAll();
        return View(Presupuesto);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Presupuestos());
    }

    [HttpPost]
    public IActionResult Create(Presupuestos presupuesto)
    {
        _presupuestoRepository.Created(presupuesto);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var presupuesto = _presupuestoRepository.GetById(id);
        if (presupuesto == null)
        {
            return NotFound("No se encontro el presupuesto.");
        }
        List<Productos> productos = _productoRepository.GetAll();
        ViewData["Productos"] = productos;
        return View(presupuesto);
    }

    [HttpPost]
    public IActionResult Update(int id, int productoId, int cantidad)
    {
        var presupuesto = _presupuestoRepository.GetById(id);
        var producto = _productoRepository.GetById(productoId);

        if (presupuesto == null || producto == null)
        {
            return NotFound();
        }

        // Agregar producto al presupuesto
        _presupuestoRepository.Update(id, producto, cantidad);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        _presupuestoRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
