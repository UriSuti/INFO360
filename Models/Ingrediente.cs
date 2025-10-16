namespace Info360.Models;

public class Ingrediente
{
    public string medida { get; private set; }
    public double precio { get; private set; }

    public Ingrediente(string medida, double precio)
    {
        this.medida = medida;
        this.precio = precio;
    }
}