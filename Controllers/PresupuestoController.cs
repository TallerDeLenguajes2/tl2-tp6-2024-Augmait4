using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Augmait4.Models;

namespace tl2_tp6_2024_Augmait4.Controllers;

public class PresupuestoController : Controller
{
    private readonly PresupuestosRepository _presupuestoRepository;
    private readonly ILogger<PresupuestoController> _logger;

    public PresupuestoController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        _presupuestoRepository = new PresupuestosRepository("Data Source= BD/Tienda.db;Cache = Shared");
    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Presupuestos> Presupuesto = _presupuestoRepository.GetAll();
        return View(Presupuesto);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
