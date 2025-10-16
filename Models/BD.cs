//dotnet add package Microsoft.Data.SqlClient
//dotnet add package Dapper

using Microsoft.Data.SqlClient;
using Dapper;

namespace Info360.Models;

public static class BD
{
    private static string _connectionString = @"Server=localhost;DataBase=Info360;Integrated Security=True;TrustServerCertificate=True;";
}