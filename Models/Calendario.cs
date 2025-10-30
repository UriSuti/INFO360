namespace Info360.Models;

public class Calendario
{
    public int idUsuario { get; private set; }
    public DateTime fecha { get; private set; }
    public string momento { get; private set; }

    public Calendario(int idUsuario, DateTime fecha, string momento)
    {
        this.idUsuario = idUsuario;
        this.fecha = fecha;
        this.momento = momento;
    }
}