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
            List<CalendariosxRecetas> lista = new List<CalendariosxRecetas>()
            {
                new CalendariosxRecetas(new Receta("Ensalada de quinoa", "10cal | 20g proteína | 5g grasas"), new DateTime(2025, 10, 30), "desayuno"),
                new CalendariosxRecetas(new Receta("Tostadas con queso", "7cal | 9g proteína | 12g grasas"), new DateTime(2025, 10, 31), "merienda"),
                new CalendariosxRecetas(new Receta("Pollo con papas", "7cal | 9g proteína | 12g grasas"), new DateTime(2025, 10, 31), "almuerzo"),
                new CalendariosxRecetas(new Receta("Pizza napolitana", "7cal | 9g proteína | 12g grasas"), new DateTime(2025, 10, 31), "cena"),
                new CalendariosxRecetas(new Receta("Papas fritas", "7cal | 9g proteína | 12g grasas"), new DateTime(2025, 10, 31), "cena")
            };            
            
            ViewBag.recetas = lista;
            return View("verCalendario");
        }
        return View("Index2");
    }

    public IActionResult verCalendario()
    {
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("Usuario"));
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        // List<CalendariosxRecetas> lista = BD.buscarCalendariosRecetas(idUsuario);
        // ViewBag.lista = lista;
        return View("");
    }
}
