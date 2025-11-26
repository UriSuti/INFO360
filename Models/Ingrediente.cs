namespace Info360.Models;

public class Ingrediente
{

    public int id { get; private set; }
    public string nombre { get; private set; }
    public string medida { get; private set; }
    public double precio { get; private set; }
    public Ingrediente() { }

    public Ingrediente(string nombre, string medida, double precio)
    {
        this.nombre = nombre;
        this.medida = medida;
        this.precio = precio;
    }
}
