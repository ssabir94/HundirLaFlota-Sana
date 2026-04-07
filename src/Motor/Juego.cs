using HundirLaFlota.Dominio;
using HundirLaFlota.Presentacion;
using HundirLaFlota.Datos;

namespace HundirLaFlota.Motor;

public class Juego
{
    // Definir fases internas del juego
    private enum Fase
    {
        Colocacion,
        Batalla,
        Terminado
    }

    // Guardar jugador humano
    private Jugador jugador;

    // Guardar CPU
    private Cpu cpu;

    // Guardar renderizador
    private Renderizador renderizador;

    // Guardar gestor de guardado
    private GestorGuardado gestorGuardado;

    // Guardar fase actual
    private Fase faseActual;

    // Inicializar juego
    public Juego()
    {
        // Crear jugador
        jugador = new Jugador("Jugador");

        // Crear CPU
        cpu = new Cpu("CPU");

        // Crear renderizador
        renderizador = new Renderizador();

        // Crear gestor de guardado
        gestorGuardado = new GestorGuardado();

        // Inicializar fase
        faseActual = Fase.Colocacion;
    }

    // Iniciar partida
    public void Iniciar()
    {
        // Mostrar bienvenida
        renderizador.MostrarBienvenida();

        // Preparar flota del jugador
        List<Barco> flotaJugador = Flota.CrearFlota();

        // Preparar flota de la CPU
        List<Barco> flotaCpu = Flota.CrearFlota();

        // Colocar flota del jugador
        ColocarFlotaJugador(flotaJugador);

        // Colocar flota de la CPU
        cpu.ColocarFlotaAleatoria(flotaCpu);

        // Cambiar a fase de batalla
        faseActual = Fase.Batalla;

        // Ejecutar batalla
        while (!HayGanador())
        {
            TurnoJugador();

            if (HayGanador())
            {
                break;
            }

            TurnoCpu();
        }

        // Cambiar a fase terminada
        faseActual = Fase.Terminado;

        // Mostrar resultado final
        MostrarResultadoFinal();
    }

    // Colocar flota del jugador
    private void ColocarFlotaJugador(List<Barco> flota)
    {
        // Recorrer barcos de la flota
        foreach (Barco barco in flota)
        {
            bool colocado = false;

            // Repetir hasta colocar correctamente
            while (!colocado)
            {
                // Pedir fila
                Console.Write("Introduce fila para " + barco.Nombre + " (0-9): ");
                string? textoFila = Console.ReadLine();

                // Pedir columna
                Console.Write("Introduce columna para " + barco.Nombre + " (0-9): ");
                string? textoColumna = Console.ReadLine();

                // Pedir orientación
                Console.Write("Introduce orientación (H/V): ");
                string? textoOrientacion = Console.ReadLine();

                // Validar fila
                bool filaValida = int.TryParse(textoFila, out int fila);

                // Validar columna
                bool columnaValida = int.TryParse(textoColumna, out int columna);

                if (!filaValida || !columnaValida)
                {
                    renderizador.MostrarError("Posición no válida.");
                    continue;
                }

                // Validar orientación
                bool esHorizontal = true;

                if (textoOrientacion == null)
                {
                    renderizador.MostrarError("Orientación no válida.");
                    continue;
                }

                textoOrientacion = textoOrientacion.ToUpper();

                if (textoOrientacion == "H")
                {
                    esHorizontal = true;
                }
                else if (textoOrientacion == "V")
                {
                    esHorizontal = false;
                }
                else
                {
                    renderizador.MostrarError("Orientación no válida.");
                    continue;
                }

                // Comprobar colocación
                if (jugador.Tablero.PuedeColocar(barco, fila, columna, esHorizontal))
                {
                    jugador.Tablero.ColocarBarco(barco, fila, columna, esHorizontal);
                    colocado = true;
                }
                else
                {
                    renderizador.MostrarError("No se puede colocar el barco en esa posición.");
                }
            }
        }
    }

    // Ejecutar turno del jugador
    private void TurnoJugador()
    {
        bool turnoValido = false;

        // Repetir hasta hacer disparo válido
        while (!turnoValido)
        {
            // Mostrar tableros de batalla
            renderizador.MostrarTablerosBatalla(jugador.Tablero, cpu.Tablero);

            // Pedir coordenada
            string coordenada = renderizador.PedirCoordenada();

            // Convertir coordenada a fila y columna
            bool conversionCorrecta = ConvertirCoordenada(coordenada, out int fila, out int columna);

            if (!conversionCorrecta)
            {
                renderizador.MostrarError("Coordenada no válida.");
                continue;
            }

            // Registrar disparo sobre tablero enemigo
            ResultadoDisparo resultado = cpu.Tablero.Disparar(fila, columna);

            // Comprobar disparo repetido
            if (resultado == ResultadoDisparo.YaDisparado)
            {
                renderizador.MostrarError("Esa casilla ya ha sido atacada.");
                continue;
            }

            // Registrar estadísticas del jugador
            jugador.RegistrarDisparo(resultado);

            // Mostrar resultado del disparo
            renderizador.MostrarResultadoDisparo(resultado, fila, columna);

            turnoValido = true;
        }
    }

    // Ejecutar turno de la CPU
    private void TurnoCpu()
    {
        // Elegir objetivo automático
        (int fila, int columna) objetivo = cpu.ElegirObjetivo();

        // Disparar sobre tablero del jugador
        ResultadoDisparo resultado = jugador.Tablero.Disparar(objetivo.fila, objetivo.columna);

        // Registrar estadísticas de la CPU
        cpu.RegistrarDisparo(resultado);

        // Mostrar disparo de la CPU
        renderizador.MostrarDisparoCpu(resultado, objetivo.fila, objetivo.columna);
    }

    // Comprobar si hay ganador
    public bool HayGanador()
    {
        // Comprobar derrota del jugador
        if (jugador.Tablero.TodosHundidos)
        {
            return true;
        }

        // Comprobar derrota de la CPU
        if (cpu.Tablero.TodosHundidos)
        {
            return true;
        }

        return false;
    }

    // Mostrar resultado final
    private void MostrarResultadoFinal()
    {
        // Comprobar victoria del jugador
        bool ganaJugador = cpu.Tablero.TodosHundidos;

        renderizador.MostrarResultadoFinal(ganaJugador, jugador);
    }

    // Convertir coordenada tipo B7 a fila y columna
    private bool ConvertirCoordenada(string coordenada, out int fila, out int columna)
    {
        fila = -1;
        columna = -1;

        // Comprobar nulo o vacío
        if (string.IsNullOrWhiteSpace(coordenada))
        {
            return false;
        }

        coordenada = coordenada.Trim().ToUpper();

        // Comprobar longitud
        if (coordenada.Length < 2 || coordenada.Length > 3)
        {
            return false;
        }

        // Convertir letra a fila
        fila = coordenada[0] - 'A';

        // Validar fila
        if (fila < 0 || fila > 9)
        {
            return false;
        }

        // Convertir parte numérica a columna
        string parteNumero = coordenada.Substring(1);

        bool numeroValido = int.TryParse(parteNumero, out int columnaHumana);

        if (!numeroValido)
        {
            return false;
        }

        columna = columnaHumana - 1;

        // Validar columna
        if (columna < 0 || columna > 9)
        {
            return false;
        }

        return true;
    }
}