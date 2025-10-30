using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Info360.Models;

namespace Info360.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("Usuario"));
        if (usuario is null)
        {
            return View("Index")
        }
        return View("Index2");
    }

    public IActionResult verCalendario()
    {
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("Usuario"));
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        List<CalendariosxRecetas> lista = BD.buscarCalendariosRecetas(idUsuario);
        ViewBag.lista = lista;
        return View("");
    }
}
