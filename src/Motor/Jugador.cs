using HundirLaFlota.Dominio;

namespace HundirLaFlota.Motor;

public class Jugador
{
    // Guardar nombre del jugador
    public string Nombre { get; set; }

    // Guardar tablero propio
    public Tablero Tablero { get; set; }

    // Guardar número total de disparos
    public int Disparos { get; set; }

    // Guardar número de aciertos
    public int Aciertos { get; set; }

    // Guardar número de fallos
    public int Fallos { get; set; }

    // Calcular precisión del jugador
    public double Precision
    {
        get
        {
            if (Disparos == 0)
            {
                return 0;
            }

            return (double)Aciertos / Disparos * 100;
        }
    }

    // Inicializar jugador
    public Jugador(string nombre)
    {
        Nombre = nombre;
        Tablero = new Tablero();
        Disparos = 0;
        Aciertos = 0;
        Fallos = 0;
    }

    // Registrar resultado de disparo
    public void RegistrarDisparo(ResultadoDisparo resultado)
    {
        // Incrementar disparos totales
        Disparos++;

        // Comprobar agua
        if (resultado == ResultadoDisparo.Agua)
        {
            Fallos++;
        }

        // Comprobar impacto o hundimiento
        if (resultado == ResultadoDisparo.Impacto || resultado == ResultadoDisparo.Hundido)
        {
            Aciertos++;
        }
    }
}