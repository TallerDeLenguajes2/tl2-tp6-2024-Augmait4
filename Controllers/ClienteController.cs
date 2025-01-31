using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Augmait4.Models;

namespace tl2_tp6_2024_Augmait4.Controllers;

public class ClienteController : Controller
{
    private readonly ClienteRepository _clienteRepository;
    private readonly ILogger<PresupuestoController> _logger;

    public ClienteController(ILogger<PresupuestoController> logger)
    {
        _logger = logger;
        _clienteRepository = new ClienteRepository("Data Source = BD/Tienda.db;Cache = Shared");
    }
    [HttpGet]
    public IActionResult Index()
    {
        List<Cliente> cliente = _clienteRepository.GetAll();
        return View(cliente);
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Cliente());
    }

    [HttpPost]
    public IActionResult Create(Cliente cliente)
    {
        _clienteRepository.Created(cliente);
        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        _clienteRepository.Delete(id);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
