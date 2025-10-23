namespace Info360.Models;

public class Usuario
{
    public string nombre { get; private set; }
    public string apellido { get; private set; }
    public string email { get; private set; }
    public string contraseña { get; private set; }
    public int edad { get; private set; }
    public int idCalendario { get; private set; }

    public Usuario(string nombre, string apellido, string email, string contraseña, int edad, int idCalendario)
    {
        this.nombre = nombre;
        this.apellido = apellido;
        this.email = email;
        this.contraseña = contraseña;
        this.edad = edad;
        this.idCalendario = idCalendario;
    }
}