namespace Info360.Models;

public class Calendario
{
    public int idUsuario { get; private set; }
    public DateTime fecha { get; private set; }

    public Calendario(int idUsuario, DateTime fecha)
    {
        this.idUsuario = idUsuario;
        this.fecha = fecha;
    }
}