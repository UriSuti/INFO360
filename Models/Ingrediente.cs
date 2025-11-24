namespace Info360.Models;

public class Ingrediente
{
    // Id para mapear desde la BD
    public int id { get; set; }
    public string medida { get; set; } = string.Empty;
    public double precio { get; set; } = 0d;

    // Parameterless constructor required for Dapper mapping
    public Ingrediente() { }

    public Ingrediente(string medida, double precio)
    {
        this.medida = medida;
        this.precio = precio;
    }
}