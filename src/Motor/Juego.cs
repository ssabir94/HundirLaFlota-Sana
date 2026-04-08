using HundirLaFlota.Datos;
using HundirLaFlota.Dominio;
using HundirLaFlota.Presentacion;

namespace HundirLaFlota.Motor;

public class Juego
{
    private enum Fase
    {
        Colocacion,
        Batalla,
        Terminado,
    }

    private Jugador jugador;
    private Cpu cpu;
    private Renderizador renderizador;
    private GestorGuardado gestorGuardado;
    private Fase faseActual;

    public Juego()
    {
        jugador = new Jugador("Jugador");
        cpu = new Cpu("CPU");
        renderizador = new Renderizador();
        gestorGuardado = new GestorGuardado();
        faseActual = Fase.Colocacion;
    }

    public void Iniciar()
    {
        renderizador.MostrarBienvenida();

        if (gestorGuardado.ExistePartidaGuardada)
        {
            string respuesta = renderizador.PedirConfirmacionPartidaGuardada();

            if (respuesta == "S")
            {
                CargarPartida();
            }
        }

        if (faseActual == Fase.Colocacion)
        {
            List<Barco> flotaJugador = Flota.CrearFlota();
            List<Barco> flotaCpu = Flota.CrearFlota();

            ColocarFlotaJugador(flotaJugador);
            cpu.ColocarFlotaAleatoria(flotaCpu);

            faseActual = Fase.Batalla;
            GuardarPartida("Jugador");
        }

        while (!HayGanador())
        {
            TurnoJugador();

            if (HayGanador())
            {
                break;
            }

            TurnoCpu();
        }

        faseActual = Fase.Terminado;
        MostrarResultadoFinal();
    }

    private void ColocarFlotaJugador(List<Barco> flota)
    {
        foreach (Barco barco in flota)
        {
            bool colocado = false;

            while (!colocado)
            {
                renderizador.MostrarTableroColocacion(jugador.Tablero, barco, -1, -1, true);

                int fila = renderizador.PedirFilaBarco(barco);
                int columna = renderizador.PedirColumnaBarco(barco);
                bool esHorizontal = renderizador.PedirOrientacion();

                renderizador.MostrarTableroColocacion(
                    jugador.Tablero,
                    barco,
                    fila,
                    columna,
                    esHorizontal
                );

                bool confirmar = renderizador.PedirConfirmacionPosicion();

                if (!confirmar)
                {
                    continue;
                }

                if (!jugador.Tablero.PuedeColocar(barco, fila, columna, esHorizontal))
                {
                    renderizador.MostrarError("No se puede colocar el barco en esa posición.");
                    continue;
                }

                jugador.Tablero.ColocarBarco(barco, fila, columna, esHorizontal);
                colocado = true;
            }
        }
    }

    private void TurnoJugador()
    {
        while (true)
        {
            renderizador.MostrarTablerosBatalla(jugador.Tablero, cpu.Tablero, jugador);

            string coordenada = renderizador.PedirCoordenada();

            bool conversionCorrecta = ConvertirCoordenada(
                coordenada,
                out int fila,
                out int columna
            );

            if (!conversionCorrecta)
            {
                renderizador.MostrarError("Coordenada no válida.");
                continue;
            }

            ResultadoDisparo resultado = cpu.Tablero.Disparar(fila, columna);

            if (resultado == ResultadoDisparo.YaDisparado)
            {
                renderizador.MostrarError("Esa casilla ya ha sido atacada.");
                continue;
            }

            jugador.RegistrarDisparo(resultado);
            renderizador.MostrarResultadoDisparo(resultado, fila, columna);

            GuardarPartida("Cpu");
            break;
        }
    }

    private void TurnoCpu()
    {
        (int fila, int columna) objetivo = cpu.ElegirObjetivo();

        ResultadoDisparo resultado = jugador.Tablero.Disparar(objetivo.fila, objetivo.columna);

        cpu.RegistrarDisparo(resultado);
        renderizador.MostrarDisparoCpu(resultado, objetivo.fila, objetivo.columna);

        GuardarPartida("Jugador");
    }

    public bool HayGanador()
    {
        if (jugador.Tablero.TodosHundidos)
        {
            return true;
        }

        if (cpu.Tablero.TodosHundidos)
        {
            return true;
        }

        return false;
    }

    private void MostrarResultadoFinal()
    {
        bool ganaJugador = cpu.Tablero.TodosHundidos;
        renderizador.MostrarResultadoFinal(ganaJugador, jugador);
        gestorGuardado.EliminarGuardado();
    }

    private void GuardarPartida(string turnoActual)
    {
        EstadoPartida estado = new EstadoPartida();

        estado.NombreJugador = jugador.Nombre;
        estado.FaseActual = faseActual.ToString();
        estado.TurnoActual = turnoActual;

        estado.DisparosJugador = jugador.Disparos;
        estado.AciertosJugador = jugador.Aciertos;
        estado.FallosJugador = jugador.Fallos;

        estado.DisparosCpu = cpu.Disparos;
        estado.AciertosCpu = cpu.Aciertos;
        estado.FallosCpu = cpu.Fallos;

        estado.BarcosJugador = ObtenerBarcosGuardados(jugador.Tablero);
        estado.BarcosCpu = ObtenerBarcosGuardados(cpu.Tablero);

        estado.DisparosTableroJugador = ObtenerDisparosGuardados(jugador.Tablero);
        estado.DisparosTableroCpu = ObtenerDisparosGuardados(cpu.Tablero);

        gestorGuardado.Guardar(estado);
    }

    private void CargarPartida()
    {
        EstadoPartida? estado = gestorGuardado.Cargar();

        if (estado == null)
        {
            renderizador.MostrarError("No se pudo cargar la partida.");
            return;
        }

        jugador = new Jugador(estado.NombreJugador);
        cpu = new Cpu("CPU");

        jugador.Disparos = estado.DisparosJugador;
        jugador.Aciertos = estado.AciertosJugador;
        jugador.Fallos = estado.FallosJugador;

        cpu.Disparos = estado.DisparosCpu;
        cpu.Aciertos = estado.AciertosCpu;
        cpu.Fallos = estado.FallosCpu;

        if (estado.FaseActual == "Batalla")
        {
            faseActual = Fase.Batalla;
        }
        else if (estado.FaseActual == "Terminado")
        {
            faseActual = Fase.Terminado;
        }
        else
        {
            faseActual = Fase.Colocacion;
        }

        CargarBarcosEnTablero(jugador.Tablero, estado.BarcosJugador);
        CargarBarcosEnTablero(cpu.Tablero, estado.BarcosCpu);

        CargarDisparosEnTablero(jugador.Tablero, estado.DisparosTableroJugador);
        CargarDisparosEnTablero(cpu.Tablero, estado.DisparosTableroCpu);

        foreach (CasillaDisparadaGuardada disparo in estado.DisparosTableroJugador)
        {
            cpu.EliminarObjetivoUsado(disparo.Fila, disparo.Columna);
        }
    }

    private List<BarcoGuardado> ObtenerBarcosGuardados(Tablero tablero)
    {
        List<BarcoGuardado> resultado = new List<BarcoGuardado>();
        List<Barco> barcosYaGuardados = new List<Barco>();

        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                Casilla casilla = tablero.ObtenerCasilla(fila, columna);

                if (casilla.Barco != null && !barcosYaGuardados.Contains(casilla.Barco))
                {
                    Barco barco = casilla.Barco;
                    barcosYaGuardados.Add(barco);

                    BarcoGuardado barcoGuardado = new BarcoGuardado();
                    barcoGuardado.Nombre = barco.Nombre;
                    barcoGuardado.Tamanio = barco.Tamanio;
                    barcoGuardado.Impactos = barco.Impactos;

                    foreach (Casilla c in barco.Casillas)
                    {
                        CoordenadaGuardada coordenada = new CoordenadaGuardada();
                        coordenada.Fila = c.Fila;
                        coordenada.Columna = c.Columna;
                        barcoGuardado.Coordenadas.Add(coordenada);
                    }

                    resultado.Add(barcoGuardado);
                }
            }
        }

        return resultado;
    }

    private List<CasillaDisparadaGuardada> ObtenerDisparosGuardados(Tablero tablero)
    {
        List<CasillaDisparadaGuardada> resultado = new List<CasillaDisparadaGuardada>();

        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                Casilla casilla = tablero.ObtenerCasilla(fila, columna);

                if (casilla.Disparada)
                {
                    CasillaDisparadaGuardada disparo = new CasillaDisparadaGuardada();
                    disparo.Fila = fila;
                    disparo.Columna = columna;
                    disparo.TeniaBarco = casilla.Barco != null;
                    resultado.Add(disparo);
                }
            }
        }

        return resultado;
    }

    private void CargarBarcosEnTablero(Tablero tablero, List<BarcoGuardado> barcosGuardados)
    {
        foreach (BarcoGuardado barcoGuardado in barcosGuardados)
        {
            Barco barco = new Barco(barcoGuardado.Nombre, barcoGuardado.Tamanio);

            foreach (CoordenadaGuardada coordenada in barcoGuardado.Coordenadas)
            {
                tablero.AsignarBarcoEnCasilla(coordenada.Fila, coordenada.Columna, barco);
            }

            for (int i = 0; i < barcoGuardado.Impactos; i++)
            {
                barco.RecibirImpacto();
            }
        }
    }

    private void CargarDisparosEnTablero(
        Tablero tablero,
        List<CasillaDisparadaGuardada> disparosGuardados
    )
    {
        foreach (CasillaDisparadaGuardada disparo in disparosGuardados)
        {
            tablero.MarcarCasillaDisparada(disparo.Fila, disparo.Columna);
        }
    }

    private bool ConvertirCoordenada(string coordenada, out int fila, out int columna)
    {
        fila = -1;
        columna = -1;

        if (string.IsNullOrWhiteSpace(coordenada))
        {
            return false;
        }

        coordenada = coordenada.Trim().ToUpper();

        if (coordenada.Length < 2 || coordenada.Length > 3)
        {
            return false;
        }

        fila = coordenada[0] - 'A';

        if (fila < 0 || fila > 9)
        {
            return false;
        }

        string parteNumero = coordenada.Substring(1);
        bool numeroValido = int.TryParse(parteNumero, out int columnaHumana);

        if (!numeroValido)
        {
            return false;
        }

        columna = columnaHumana - 1;

        if (columna < 0 || columna > 9)
        {
            return false;
        }

        return true;
    }
}