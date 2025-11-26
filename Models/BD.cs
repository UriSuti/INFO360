//dotnet add package Microsoft.Data.SqlClient
//dotnet add package Dapper

using Microsoft.Data.SqlClient;
using Dapper;

namespace Info360.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=Info360;Integrated Security=True;TrustServerCertificate=True;";

    public static Usuario buscarUsuario(string email, string contraseña)
    {
        Usuario usu = null;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT nombre, apellido, email, contraseña, edad FROM Usuarios WHERE email = @pEmail AND contraseña = @pContraseña";
            usu = connection.QueryFirstOrDefault<Usuario>(query, new { pEmail = email, pContraseña = contraseña });
        }
        return usu;
    }

    public static int buscarIdUsuario(string email, string contraseña)
    {
        int id;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT id FROM Usuarios WHERE email = @pEmail AND contraseña = @pContraseña";
            id = connection.QueryFirstOrDefault<int>(query, new { pEmail = email, pContraseña = contraseña });
        }
        return id;
    }

    public static string buscarRestricciones(int idUsuario)
    {
        string restriccion;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string storedProcedure = "sp__BuscarRestricciones";
            restriccion = connection.Query<string>(storedProcedure, new { idUsuario = idUsuario }, commandType: System.Data.CommandType.StoredProcedure).ToString();
        }
        return restriccion;
    }

    public static void registrarse(Usuario usuario)
    {
        string query = "INSERT INTO Usuarios (nombre, apellido, email, contraseña, edad) VALUES (@pNombre, @pApellido, @pEmail, @pContraseña, @pEdad)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pNombre = usuario.nombre, pApellido = usuario.apellido, pEmail = usuario.email, pContraseña = usuario.contraseña, pEdad = usuario.edad });
        }
    }

    public static void agregarCalendario(Calendario calendario)
    {
        string query = "INSERT INTO Calendarios (fecha, momento, idUsuario) VALUES (@pFecha, @pMomento, @pIdUsuario)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pFecha = calendario.fecha, pMomento = calendario.momento, pIdUsuario = calendario.idUsuario });
        }
    }

    public static List<CalendariosxRecetas> buscarCalendariosRecetas(int idUsuario)
    {
        List<CalendariosxRecetas> lista = new List<CalendariosxRecetas>();
        List<Calendario> calendarios = new List<Calendario>();
        Receta receta = null;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT idUsuario, fecha, momento FROM Calendarios WHERE idUsuario = @pIdUsuario";
            calendarios = connection.Query<Calendario>(query, new { pIdUsuario = idUsuario }).ToList();
            foreach (Calendario calendario in calendarios)
            {
                int idCalendario = buscarIdCalendario(calendario);
                receta = buscarRecetaDesdeCalendarios(idCalendario);
                lista.Add(new CalendariosxRecetas(receta, calendario.fecha, calendario.momento));
            }
        }
        return lista;
    }

    public static List<Receta> buscarRecetas()
    {
        List<Receta> recetas = new List<Receta>();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT nombre, descripcion, urlFoto FROM Recetas";
            recetas = connection.Query<Receta>(query).ToList();
        }
        return recetas;
    }

    public static int agregarReceta(string nombre, string descripcion, string urlFoto)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Recetas (nombre, descripcion, urlFoto) VALUES (@pNombre, @pDescripcion, @pUrlFoto); SELECT CAST(SCOPE_IDENTITY() as int);";
            int id = connection.QuerySingle<int>(query, new { pNombre = nombre, pDescripcion = descripcion, pUrlFoto = urlFoto });
            return id;
        }
    }

    public static List<Ingrediente> buscarIngredientes()
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT * FROM Ingredientes";
            return connection.Query<Ingrediente>(query).ToList();
        }
    }

    public static void agregarRecetaIngrediente(int idReceta, int idIngrediente)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO RecetasIngredientes (idReceta, idIngrediente) VALUES (@pIdReceta, @pIdIngrediente);";
            connection.Execute(query, new { pIdReceta = idReceta, pIdIngrediente = idIngrediente });
        }
    }

    public static Receta buscarRecetaDesdeCalendarios(int idCalendario)
    {
        Receta receta;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Recetas.nombre, Recetas.descripcion, Recetas.urlFoto FROM Recetas INNER JOIN CalendariosRecetas ON Recetas.id = CalendariosRecetas.idReceta INNER JOIN Calendarios ON CalendariosRecetas.idCalendario = @pIdCalendario";
            receta = connection.QueryFirstOrDefault<Receta>(query, new { pIdCalendario = idCalendario });
        }
        return receta;
    }

    public static int buscarIdReceta(Receta receta)
    {
        int id;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT id FROM Recetas WHERE nombre = @pNombre";
            id = connection.QueryFirstOrDefault<int>(query, new { pNombre = receta.nombre });
        }
        return id;
    }

    public static int buscarIdCalendario(Calendario calendario)
    {
        int id;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT id FROM Calendarios WHERE idUsuario = @pIdUsuario AND fecha = @pFecha AND momento = @pMomento";
            id = connection.QueryFirstOrDefault<int>(query, new { pIdUsuario = calendario.idUsuario, pFecha = calendario.fecha, pMomento = calendario.momento });
        }
        return id;
    }

    public static void agregarCalendarioReceta(int idCalendario, int idReceta)
    {
        string query = "INSERT INTO CalendariosRecetas (idCalendario, idReceta) VALUES (@pIdCalendario, @pIdReceta)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pIdCalendario = idCalendario, pIdReceta = idReceta });
        }
    }

    public static Receta buscarReceta(string nombre)
    {
        Receta receta;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT nombre, descripcion, urlFoto FROM Recetas WHERE nombre = @pNombre";
            receta = connection.QueryFirstOrDefault<Receta>(query, new { @pNombre = nombre });
        }
        return receta;
    }

    public static List<Ingrediente> buscarHeladera(int idUsuario)
    {
        List<Ingrediente> ingredientes = new List<Ingrediente>();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = @"SELECT *
                                FROM Ingredientes i
                                INNER JOIN Heladeras h ON i.id = h.idIngrediente
                                WHERE h.idUsuario = @pIdUsuario";
            ingredientes = connection.Query<Ingrediente>(query, new { pIdUsuario = idUsuario }).ToList();
        }
        return ingredientes;
    }

    public static int agregarIngredienteReturnId(string medida, double precio, string nombre)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Ingredientes (nombre, medida, precio) VALUES (@pNombre, @pMedida, @pPrecio); SELECT CAST(SCOPE_IDENTITY() as int);";
            int id = connection.QuerySingle<int>(query, new {pMedida = medida, pPrecio = precio, pNombre = nombre });
            return id;
        }
    }

    public static void agregarIngredienteHeladera(int idUsuario, int idIngrediente)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "INSERT INTO Heladeras (idUsuario, idIngrediente) VALUES (@pIdUsuario, @pIdIngrediente)";
            connection.Execute(query, new { pIdUsuario = idUsuario, pIdIngrediente = idIngrediente });
        }
    }

    public static void quitarIngredienteHeladera(int idUsuario, int idIngrediente)
    {
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "DELETE FROM Heladeras WHERE idUsuario = @pIdUsuario AND idIngrediente = @pIdIngrediente";
            connection.Execute(query, new { pIdUsuario = idUsuario, pIdIngrediente = idIngrediente });
        }
    }
}