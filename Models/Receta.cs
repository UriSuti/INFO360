namespace Info360.Models;

public class Receta
{
    public string nombre { get; private set; }
    public string descripcion { get; private set; }

    public Receta(string nombre, string descripcion)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
    }
}