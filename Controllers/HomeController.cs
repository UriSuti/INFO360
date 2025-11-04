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
        ViewBag.Recetas = new List<CalendariosxRecetas>()
        {
            new CalendariosxRecetas(
                new Receta("Tostadas integrales con palta", "Desayuno saludable y rápido"),
                new DateTime(2025, 11, 4),
                "desayuno"
            ),
            new CalendariosxRecetas(
                new Receta("Ensalada César", "Pollo, lechuga, crutones y aderezo ligero"),
                new DateTime(2025, 11, 4),
                "almuerzo"
            ),
            new CalendariosxRecetas(
                new Receta("Sopa de calabaza", "Ideal para una cena liviana y nutritiva"),
                new DateTime(2025, 11, 4),
                "cena"
            ),
            new CalendariosxRecetas(
                new Receta("Panqueques de avena", "Rápidos, ricos y con banana"),
                new DateTime(2025, 11, 5),
                "desayuno"
            ),
            new CalendariosxRecetas(
                new Receta("Wraps de pollo", "Tortilla integral con vegetales y pollo grillado"),
                new DateTime(2025, 11, 5),
                "almuerzo"
            ),
            new CalendariosxRecetas(
                new Receta("Salteado de verduras", "Liviano y lleno de sabor"),
                new DateTime(2025, 11, 6),
                "cena"
            )
        };
        return View("verCalendario");
    }

    public IActionResult Registrarse(string nombre, string apellido, string email, string contraseña, int edad)
    {
        Usuario usu = new Usuario(nombre, apellido, email, contraseña, edad);
        BD.registrarse(usu);
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usu));
        return View("");
    }

    public IActionResult Login(string email, string contraseña)
    {
        Usuario usuario = BD.buscarUsuario(email, contraseña);
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usuario));
        return View("");
    }

    public IActionResult AgregarCalendarioyReceta(DateTime fecha, string momento, string nombreReceta)
    {
        Usuario usuario = Objeto.StringToObject<Usuario>(HttpContext.Session.GetString("Usuario"));
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        Calendario calendario = new Calendario(idUsuario, fecha, momento);
        Receta receta = BD.buscarReceta(nombreReceta);
        BD.agregarCalendario(calendario);
        BD.agregarCalendarioReceta(BD.buscarIdCalendario(calendario), BD.buscarIdReceta(receta));
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usuario));
        List<CalendariosxRecetas> calendarios = BD.buscarCalendariosRecetas(idUsuario);
        ViewBag.Calendarios = calendarios;
        return View("");
    }
}
