namespace Info360.Models;

public class CalendariosxRecetas
{
    public Receta receta { get; private set; }
    public DateTime fecha { get; private set; }
    public string momento { get; private set; }
    public CalendariosxRecetas(Receta receta, DateTime fecha, string momento)
    {
        this.receta = receta;
        this.fecha = fecha;
        this.momento = momento;
    }
}