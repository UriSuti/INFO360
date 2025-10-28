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
            string query = "SELECT nombre, apellido, email, contraseña, edad, idCalendario FROM Usuarios WHERE email = @pEmail AND contraseña = @pContraseña";
            usu = connection.QueryFirstOrDefault<Usuario>(query, new { pEmail = email, pContraseña = contraseña });
        }
        return usu;
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
}