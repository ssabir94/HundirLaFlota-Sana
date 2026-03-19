using HundirLaFlota.Dominio;
using HundirLaFlota.Motor;

namespace HundirLaFlota.Presentacion;

public class Renderizador
{
    public void MostrarBienvenida() { }

    public void MostrarTablerosBatalla(Tablero propio, Tablero enemigo) { }

    public void MostrarTableroColocacion(Tablero tablero, Barco barco) { }

    public string PedirCoordenada()
    {
        return "";
    }

    public string PedirPosicion(Barco barco)
    {
        return "";
    }

    public void MostrarResultadoDisparo(ResultadoDisparo resultado, int fila, int columna) { }

    public void MostrarDisparoCpu(ResultadoDisparo resultado, int fila, int columna) { }

    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador) { }

    public void MostrarError(string mensaje) { }
}