using HundirLaFlota.Dominio;

namespace HundirLaFlota.Motor;

public class Cpu : Jugador
{
    private List<(int, int)> objetivos;
    private Random random;

    public Cpu(string nombre) : base(nombre)
    {
        objetivos = new List<(int, int)>();
        random = new Random();
    }

    public void ColocarFlotaAleatoria(List<Barco> barcos)
    {
    }

    public (int, int) ElegirObjetivo()
    {
        return (0, 0);
    }
}