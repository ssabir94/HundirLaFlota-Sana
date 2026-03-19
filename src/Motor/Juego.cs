using HundirLaFlota.Dominio;
using HundirLaFlota.Presentacion;
using HundirLaFlota.Datos;

namespace HundirLaFlota.Motor;

public class Juego
{
    private Jugador jugador;
    private Cpu cpu;
    private Renderizador renderizador;
    private GestorGuardado gestorGuardado;
    private Marcador marcador;
    private ConfigJuego config;

    public Juego()
    {
    }

    public void Iniciar()
    {
    }

    public void ColocacionInteractiva()
    {
    }

    public bool HayGanador()
    {
        return false;
    }
}