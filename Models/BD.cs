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

    public static int eliminarUsuario(string email)
    {
        string query = "DELETE FROM Usuarios WHERE email = @pEmail";
        int registrosAfectados = 0;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            registrosAfectados = connection.Execute(query, new { email });
        }
        return registrosAfectados;
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

    // public static void registrarse(Usuario usuario)
    // {
    //     string query = "INSERT INTO Usuarios (nombre, apellido, email, contraseña, edad, idCalendario) VALUES (@pNombre, @pApellido, @pEmail, @pContraseña, @pEdad, @pIdCalendario)";
    //     using(SqlConnection connection = new SqlConnection(_connectionString))
    //     {
    //         connection.Execute(query, new { pNombre = usuario.nombre, pApellido = usuario.apellido, pEmail = usuario.email, pContraseña = usuario.contraseña, pEdad = usuario.edad, pIdCalendario = usuario.idCalendario });
    //     }
    // }

    public static void agregarCalendario(Calendario calendario)
    {
        string query = "INSERT INTO Calendarios (fecha, idUsuario) VALUES (@pFecha, @pIdUsuario)";
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            connection.Execute(query, new { pFecha = calendario.fecha, pIdUsuario = calendario.idUsuario });
        }
    }

    public static List<CalendariosxRecetas> buscarCalendariosRecetas(int idUsuario)
    {
        List<CalendariosxRecetas> lista = new List<CalendariosxRecetas>();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT idUsuario, fecha FROM Calendarios WHERE idUsuario = @pIdUsuario";
            List<Calendario> calendarios = connection.Query<Calendario>(query, new { pIdUsuario = idUsuario }).ToList();
            foreach (Calendario calendario in calendarios)
            {
                int idCalendario = buscarIdCalendario(calendario);
                Receta receta = buscarRecetaDesdeCalendarios(idCalendario);
                lista.Add(new CalendariosxRecetas(receta, calendario.fecha, "almuerzo"));
            }
        }
        return lista;
    }

    public static List<Receta> buscarRecetas()
    {
        List<Receta> recetas = new List<Receta>();
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT nombre, descripcion FROM Recetas";
            recetas = connection.Query<Receta>(query).ToList();
        }
        return recetas;
    }

    public static Receta buscarRecetaDesdeCalendarios(int idCalendario)
    {
        Receta receta;
        using(SqlConnection connection = new SqlConnection(_connectionString))
        {
            string query = "SELECT Recetas.nombre, Recetas.descripcion FROM Recetas INNER JOIN CalendariosRecetas ON Recetas.id = CalendariosRecetas.idReceta INNER JOIN Calendarios ON CalendariosRecetas.idCalendario = @pIdCalendario";
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
            string query = "SELECT id FROM Calendarios WHERE idUsuario = @pIdUsuario AND fecha = @pFecha";
            id = connection.QueryFirstOrDefault<int>(query, new { pIdUsuario = calendario.idUsuario, pFecha = calendario.fecha });
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
}