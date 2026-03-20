namespace HundirLaFlota.Dominio;

public class Flota
{
    public static List<Barco> CrearFlota()
    {
        return new List<Barco>
        {
            new Barco("Portaaviones", 5),
            new Barco("Acorazado", 4),
            new Barco("Destructor", 3),
            new Barco("Submarino", 3),
            new Barco("Patrullera", 2)
        };
    }
}