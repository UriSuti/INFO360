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
        return View("perfil");
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
