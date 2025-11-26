using System.Diagnostics;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Info360.Models;

namespace Info360.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;

    public HomeController(ILogger<HomeController> logger, Microsoft.AspNetCore.Hosting.IWebHostEnvironment env)
    {
        _logger = logger;
        _env = env;
    }

    private Usuario? GetUsuarioFromSession()
    {
        var s = HttpContext.Session.GetString("Usuario");
        if (string.IsNullOrEmpty(s)) return null;
        return Objeto.StringToObject<Usuario>(s);
    }

    public IActionResult Index()
    {
        return View("Login");
    }

    public IActionResult agregarIngrediente()
    {
        List<Receta> recetas = BD.buscarRecetas();
        ViewBag.recetas = recetas;
        return View("agregarIngrediente");
    }

    [HttpGet]
    public IActionResult SubirReceta()
    {
        ViewBag.ingredientes = BD.buscarIngredientes();
        return View("subirReceta");
    }

    [HttpPost]
    public IActionResult verCalendario(DateTime fecha)
    {
        ViewBag.Fecha = fecha;
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        ViewBag.Recetas = BD.buscarCalendariosRecetas(idUsuario);

        return View("verCalendario");    
    }

    public IActionResult verCalendario()
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        ViewBag.Recetas = BD.buscarCalendariosRecetas(idUsuario);
        return View("verCalendario");
    }

    [HttpPost]
    public IActionResult Registrarse(string nombre, string apellido, string email, string contraseña, int edad)
    {
        Usuario usu = new Usuario(nombre, apellido, email, contraseña, edad);
        BD.registrarse(usu);
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usu));
        ViewBag.Usuario = usu;
        return View("Index");
    }

    public IActionResult Registrarse()
    {
        return View("Signin");
    }
    
    [HttpPost]
    public IActionResult Login(string email, string contraseña)
    {
        Usuario? usuario = BD.buscarUsuario(email, contraseña);
        if (usuario == null)
        {
            return View("Login");
        }
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usuario));
        ViewBag.Usuario = usuario;
        return View("Index");
    }

    public IActionResult HeladeraVirtual()
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");

        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        List<Ingrediente> items = BD.buscarHeladera(idUsuario);
        ViewBag.ingredientes = BD.buscarIngredientes();
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usuario));
        ViewBag.Heladera = items;
        return View("Heladeravirtual", items);
    }

    [HttpPost]
    public IActionResult AgregarHeladera(string nombre, string medida, double precio)
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");

        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        int idIngredienteFinal = BD.agregarIngredienteReturnId(medida, precio, nombre);

        BD.agregarIngredienteHeladera(idUsuario, idIngredienteFinal);
        return RedirectToAction("HeladeraVirtual");
    }

    [HttpPost]
    public IActionResult EliminarHeladera(int idIngrediente)
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");

        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        BD.quitarIngredienteHeladera(idUsuario, idIngrediente);
        return RedirectToAction("HeladeraVirtual");
    }

    public IActionResult Recetas()
    {
        ViewBag.ingredientes = BD.buscarIngredientes();
        ViewBag.recetas = BD.buscarRecetas();
        return View("recetas");
    }  

    [HttpPost]
    public IActionResult Recetas(int[] ingredientes)
    {
        ViewBag.ingredientes = BD.buscarIngredientes();
        ViewBag.recetas = BD.buscarRecetas();
        return View("verCalendario");
    }  
    
    public IActionResult RecetasFav()
    {
        ViewBag.recetas = BD.buscarRecetas();
        return View("recetas");
    }

    public IActionResult Indexx()
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return View("Index");
        ViewBag.Usuario = usuario;
        return View("Index");
    }


    public IActionResult AgregarCalendarioyReceta(DateTime fecha, string momento, string nombreReceta)
    {
        Usuario? usuario = GetUsuarioFromSession();
        if (usuario == null) return RedirectToAction("Index");
        int idUsuario = BD.buscarIdUsuario(usuario.email, usuario.contraseña);
        Calendario calendario = new Calendario(idUsuario, fecha, momento);
        Receta receta = BD.buscarReceta(nombreReceta);
        BD.agregarCalendario(calendario);
        BD.agregarCalendarioReceta(BD.buscarIdCalendario(calendario), BD.buscarIdReceta(receta));
        HttpContext.Session.SetString("Usuario", Objeto.ObjectToString<Usuario>(usuario));
        return RedirectToAction("verCalendario");
    }

    public IActionResult carrito()
    {
        return View("Carrito");
    }

    
    [HttpPost]
    public IActionResult SubirReceta(string titulo, string resumen, IFormFile? imagen, string? ingredientes)
    {
        string urlFoto = string.Empty;
        if (imagen != null && imagen.Length > 0)
        {
            var uploads = Path.Combine(_env.WebRootPath ?? "wwwroot", "assets", "uploads");
            if (!Directory.Exists(uploads)) Directory.CreateDirectory(uploads);
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imagen.FileName);
            var filePath = Path.Combine(uploads, fileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                imagen.CopyTo(stream);
            }
            urlFoto = "/assets/uploads/" + fileName;
        }

        int idReceta = BD.agregarReceta(titulo, resumen, urlFoto);

        if (!string.IsNullOrEmpty(ingredientes))
        {
            try
            {
                var ids = JsonSerializer.Deserialize<List<int>>(ingredientes!);
                if (ids != null)
                {
                    foreach (var id in ids) BD.agregarRecetaIngrediente(idReceta, id);
                }
            }
            catch { /* ignore malformed JSON */ }
        }

        return RedirectToAction("Recetas");
    }

}
