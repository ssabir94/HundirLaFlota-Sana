using HundirLaFlota.Dominio;

namespace HundirLaFlota.Motor;

public class Jugador
{
    public string Nombre { get; set; }
    public Tablero Tablero { get; set; }

    public int Disparos { get; set; }
    public int Aciertos { get; set; }
    public int Fallos { get; set; }

    public double Precision => 0;

    public Jugador(string nombre)
    {
        Nombre = nombre;
        Tablero = new Tablero();
    }

    public void RegistrarDisparo(ResultadoDisparo resultado)
    {
    }
}