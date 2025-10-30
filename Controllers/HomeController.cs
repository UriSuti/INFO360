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
                new CalendariosxRecetas(new Receta("Ensalada de quinoa", "Rica en proteínas, con un sabor dulce a nuez."), new DateTime(2025, 10, 30), "desayuno"),
                new CalendariosxRecetas(new Receta("Tostadas con queso", "Snacks de trigo horneados que tienen una textura crujiente."), new DateTime(2025, 10, 31), "merienda"),
                new CalendariosxRecetas(new Receta("Pollo con papas", "Pollo al horno con papas fritas, puré u horno, pudiendo ser pata y muslo o pechuga."), new DateTime(2025, 10, 31), "almuerzo"),
                new CalendariosxRecetas(new Receta("Pizza napolitana", "Masa tierna y delgada pero bordes altos"), new DateTime(2025, 10, 31), "cena"),
                new CalendariosxRecetas(new Receta("Papas fritas", "Se preparan cortándose en rodajas o en forma de palitos y friéndose en aceite caliente hasta que queden doradas, retirándolas del aceite."), new DateTime(2025, 10, 31), "cena")
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
