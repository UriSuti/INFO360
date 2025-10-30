namespace Info360.Models;

public class CalendariosxRecetas
{
    public string receta { get; private set; }
    public DateTime fecha { get; private set; }
    public CalendariosxRecetas(string receta, DateTime fecha)
    {
        this.receta = receta;
        this.fecha = fecha;
    }
}