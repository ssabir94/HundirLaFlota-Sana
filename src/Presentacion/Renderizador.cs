using HundirLaFlota.Dominio;
using HundirLaFlota.Motor;

namespace HundirLaFlota.Presentacion;

public class Renderizador
{
    public void MostrarBienvenida()
    {
        EscribirColor(ArteAscii.LogoPrincipal, Colores.Titulo);
        Console.WriteLine();
    }

    public int MostrarMenuPrincipal(bool hayGuardado)
    {
        while (true)
        {
            EscribirColor(ArteAscii.LogoPrincipal, Colores.Titulo);

            Console.WriteLine("1. Nueva partida");

            if (hayGuardado)
            {
                Console.WriteLine("2. Continuar");
            }
            else
            {
                Console.WriteLine("2. Continuar (no disponible)");
            }

            Console.WriteLine("3. Récords");
            Console.WriteLine("4. Salir");
            Console.WriteLine();

            Console.Write("Selecciona una opción: ");
            string? entrada = Console.ReadLine();

            if (int.TryParse(entrada, out int opcion))
            {
                if (opcion >= 1 && opcion <= 4)
                {
                    if (opcion == 2 && !hayGuardado)
                    {
                        MostrarError("No hay partida guardada.");
                        continue;
                    }

                    return opcion;
                }
            }

            MostrarError("Opción no válida.");
        }
    }

    public string PedirConfirmacionPartidaGuardada()
    {
        Console.Write("¿Quieres continuar la partida guardada? (S/N): ");
        string? respuesta = Console.ReadLine();

        if (respuesta == null)
        {
            return "";
        }

        return respuesta.Trim().ToUpper();
    }

    public int PedirFilaBarco(Barco barco)
    {
        while (true)
        {
            Console.Write("Introduce fila para " + barco.Nombre + " (A-J): ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarError("Fila no válida.");
                continue;
            }

            entrada = entrada.Trim().ToUpper();

            if (entrada.Length != 1)
            {
                MostrarError("Fila no válida.");
                continue;
            }

            char letra = entrada[0];

            if (letra < 'A' || letra > 'J')
            {
                MostrarError("Fila no válida.");
                continue;
            }

            return letra - 'A';
        }
    }

    public int PedirColumnaBarco(Barco barco)
    {
        while (true)
        {
            Console.Write("Introduce columna para " + barco.Nombre + " (1-10): ");
            string? entrada = Console.ReadLine();

            if (!int.TryParse(entrada, out int columnaHumana))
            {
                MostrarError("Columna no válida.");
                continue;
            }

            if (columnaHumana < 1 || columnaHumana > 10)
            {
                MostrarError("Columna no válida.");
                continue;
            }

            return columnaHumana - 1;
        }
    }

    public bool PedirOrientacion()
    {
        while (true)
        {
            Console.Write("Introduce orientación (H/V): ");
            string? entrada = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(entrada))
            {
                MostrarError("Orientación no válida.");
                continue;
            }

            entrada = entrada.Trim().ToUpper();

            if (entrada == "H")
            {
                return true;
            }

            if (entrada == "V")
            {
                return false;
            }

            MostrarError("Orientación no válida.");
        }
    }

    public bool PedirConfirmacionPosicion()
    {
        Console.Write("¿Confirmar posición? (S/N): ");
        string? entrada = Console.ReadLine();

        if (entrada == null)
        {
            return false;
        }

        return entrada.Trim().ToUpper() == "S";
    }

    public void MostrarTablerosBatalla(Tablero propio, Tablero enemigo, Jugador jugador)
    {
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };
        int barcosHundidos = 5 - enemigo.BarcosRestantes;

        EscribirColor(ArteAscii.MarcoBatallaSuperior, Colores.Titulo);
        Console.WriteLine(
            "║      TU TABLERO                           MAR ENEMIGO                      ║"
        );
        Console.WriteLine(
            "║        1  2  3  4  5  6  7  8  9 10      1  2  3  4  5  6  7  8  9 10      ║"
        );

        for (int f = 0; f < 10; f++)
        {
            Console.Write("║   " + letras[f] + "   ");

            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(propio.ObtenerCasilla(f, c), true);
            }

            Console.Write("  " + letras[f] + "   ");

            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(enemigo.ObtenerCasilla(f, c), false);
            }

            Console.WriteLine("   ║");
        }

        EscribirColor(ArteAscii.MarcoBatallaEstadisticas, Colores.Titulo);

        string linea1 =
            "║  Disparos: "
            + jugador.Disparos
            + "   Aciertos: "
            + jugador.Aciertos
            + "   Fallos: "
            + jugador.Fallos
            + "   Precisión: "
            + jugador.Precision.ToString("F1")
            + " %";

        string linea2 =
            "║  Barcos hundidos: "
            + barcosHundidos
            + " / 5"
            + "       Barcos enemigos restantes: "
            + enemigo.BarcosRestantes;

        Console.WriteLine(AjustarLineaMarco(linea1));
        Console.WriteLine(AjustarLineaMarco(linea2));

        EscribirColor(ArteAscii.MarcoBatallaInferior, Colores.Titulo);
        Console.Write("  [ ");
        EscribirColorInline("S", Colores.BarcoPropio);
        Console.Write(" = barco propio   ");
        EscribirColorInline("X", Colores.Impacto);
        Console.Write(" = impacto   ");
        EscribirColorInline("~", Colores.Agua);
        Console.Write(" = agua   ");
        EscribirColorInline(".", Colores.CasillaVacia);
        Console.Write(" = vacío   ");
        EscribirColorInline("#", Colores.Hundido);
        Console.WriteLine(" = hundido ]");
        Console.WriteLine();
    }

    public void MostrarTableroColocacion(
        Tablero tablero,
        Barco barco,
        int filaPreview,
        int columnaPreview,
        bool horizontal
    )
    {
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        bool posicionValida = false;

        if (filaPreview >= 0 && columnaPreview >= 0)
        {
            posicionValida = tablero.PuedeColocar(barco, filaPreview, columnaPreview, horizontal);
        }

        EscribirColor(ArteAscii.MarcoColocacionSuperior, Colores.Titulo);
        Console.WriteLine(
            "║  Coloca tu "
                + AjustarTextoColocacion(barco.Nombre + " (" + barco.Tamanio + " casillas)", 29)
                + "     ║"
        );
        Console.WriteLine("║  H = horizontal   V = vertical               ║");
        EscribirColor(ArteAscii.MarcoColocacionMedio, Colores.Titulo);
        Console.WriteLine("║      1  2  3  4  5  6  7  8  9 10            ║");

        for (int f = 0; f < 10; f++)
        {
            Console.Write("║   " + letras[f] + "   ");

            for (int c = 0; c < 10; c++)
            {
                bool esPreview = EsCasillaPreview(
                    f,
                    c,
                    filaPreview,
                    columnaPreview,
                    barco.Tamanio,
                    horizontal
                );

                if (esPreview)
                {
                    if (posicionValida)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }

                    Console.Write($"{'?', 2} ");
                    Console.ResetColor();
                }
                else
                {
                    ImprimirCasilla(tablero.ObtenerCasilla(f, c), true);
                }
            }

            Console.WriteLine("         ║");
        }

        EscribirColor(ArteAscii.MarcoColocacionInferior, Colores.Titulo);
        Console.WriteLine();
        Console.Write("? = posición provisional   ");

        if (filaPreview >= 0 && columnaPreview >= 0)
        {
            if (posicionValida)
            {
                EscribirColor("Válida", ConsoleColor.Green);
            }
            else
            {
                EscribirColor("No válida", ConsoleColor.Red);
            }
        }
        else
        {
            Console.WriteLine();
        }

        Console.WriteLine();
    }

    public string PedirCoordenada()
    {
        while (true)
        {
            Console.Write("Coordenada (ej. B7): ");
            string? entrada = Console.ReadLine();

            if (entrada == null)
            {
                MostrarError("Entrada no válida.");
                continue;
            }

            entrada = entrada.Trim().ToUpper();

            if (EsCoordenadaValida(entrada))
            {
                return entrada;
            }

            MostrarError("Coordenada no válida. Usa formato A1-J10.");
        }
    }

    private bool EsCoordenadaValida(string texto)
    {
        if (texto.Length < 2 || texto.Length > 3)
        {
            return false;
        }

        char letra = texto[0];

        if (letra < 'A' || letra > 'J')
        {
            return false;
        }

        string numeroTexto = texto.Substring(1);

        if (!int.TryParse(numeroTexto, out int numero))
        {
            return false;
        }

        return numero >= 1 && numero <= 10;
    }

    public void MostrarResultadoDisparo(ResultadoDisparo resultado, int fila, int columna)
    {
        string coordenada = ConvertirACoordenadaHumana(fila, columna);

        if (resultado == ResultadoDisparo.Agua)
        {
            Console.Write("Disparo en " + coordenada + ": ");
            EscribirColor("Agua", Colores.Agua);
        }
        else if (resultado == ResultadoDisparo.Impacto)
        {
            Console.Write("Disparo en " + coordenada + ": ");
            EscribirColor("Impacto", Colores.Impacto);
        }
        else if (resultado == ResultadoDisparo.Hundido)
        {
            Console.Write("Disparo en " + coordenada + ": ");
            EscribirColor("Hundido", Colores.Hundido);
        }
        else
        {
            Console.WriteLine("Disparo en " + coordenada + ": Ya disparado");
        }
    }

    public void MostrarDisparoCpu(ResultadoDisparo resultado, int fila, int columna)
    {
        string coordenada = ConvertirACoordenadaHumana(fila, columna);

        if (resultado == ResultadoDisparo.Agua)
        {
            Console.Write("La CPU dispara en " + coordenada + ": ");
            EscribirColor("Agua", Colores.Agua);
        }
        else if (resultado == ResultadoDisparo.Impacto)
        {
            Console.Write("La CPU dispara en " + coordenada + ": ");
            EscribirColor("Impacto", Colores.Impacto);
        }
        else if (resultado == ResultadoDisparo.Hundido)
        {
            Console.Write("La CPU dispara en " + coordenada + ": ");
            EscribirColor("Hundido", Colores.Hundido);
        }
        else
        {
            Console.WriteLine("La CPU dispara en " + coordenada + ": Ya disparado");
        }
    }

    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador)
    {
        Console.WriteLine();

        if (ganaJugador)
        {
            EscribirColor(ArteAscii.Victoria, Colores.Exito);
        }
        else
        {
            EscribirColor(ArteAscii.Derrota, Colores.Error);
        }

        Console.WriteLine("║  Disparos: " + AjustarNumero(jugador.Disparos, 25) + "║");
        Console.WriteLine("║  Aciertos: " + AjustarNumero(jugador.Aciertos, 25) + "║");
        Console.WriteLine("║  Fallos: " + AjustarNumero(jugador.Fallos, 27) + "║");
        Console.WriteLine(
            "║  Precisión: "
                + AjustarTextoDerecha(jugador.Precision.ToString("F2") + " %", 24)
                + "║"
        );
        EscribirColor(ArteAscii.MarcoFinalInferior, ganaJugador ? Colores.Exito : Colores.Error);
        Console.WriteLine();
    }

    public void MostrarError(string mensaje)
    {
        Console.ForegroundColor = Colores.Error;
        Console.WriteLine("Error: " + mensaje);
        Console.ResetColor();
    }

    private void ImprimirCasilla(Casilla casilla, bool esPropio)
    {
        string simbolo;
        ConsoleColor color;

        if (casilla.EsImpacto)
        {
            if (casilla.Barco != null && casilla.Barco.EstaHundido)
            {
                simbolo = "#";
                color = Colores.Hundido;
            }
            else
            {
                simbolo = "X";
                color = Colores.Impacto;
            }
        }
        else if (casilla.EsAgua)
        {
            simbolo = "~";
            color = Colores.Agua;
        }
        else if (!casilla.EstaVacia && esPropio)
        {
            simbolo = "S";
            color = Colores.BarcoPropio;
        }
        else
        {
            simbolo = ".";
            color = Colores.CasillaVacia;
        }

        Console.ForegroundColor = color;
        Console.Write($"{simbolo, 2} ");
        Console.ResetColor();
    }

    private bool EsCasillaPreview(
        int filaActual,
        int columnaActual,
        int filaPreview,
        int columnaPreview,
        int tamanio,
        bool horizontal
    )
    {
        if (filaPreview < 0 || columnaPreview < 0)
        {
            return false;
        }

        for (int i = 0; i < tamanio; i++)
        {
            int filaBarco = horizontal ? filaPreview : filaPreview + i;
            int columnaBarco = horizontal ? columnaPreview + i : columnaPreview;

            if (filaActual == filaBarco && columnaActual == columnaBarco)
            {
                return true;
            }
        }

        return false;
    }

    private string ConvertirACoordenadaHumana(int fila, int columna)
    {
        char letra = (char)('A' + fila);
        return letra + (columna + 1).ToString();
    }

    private string AjustarLineaMarco(string texto)
    {
        const int anchoInterior = 76;

        if (texto.Length < anchoInterior + 1)
        {
            return texto.PadRight(anchoInterior + 1) + "║";
        }

        if (texto.Length == anchoInterior + 1)
        {
            return texto + "║";
        }

        return texto.Substring(0, anchoInterior + 1) + "║";
    }

    private string AjustarTextoColocacion(string texto, int ancho)
    {
        if (texto.Length > ancho)
        {
            return texto.Substring(0, ancho);
        }

        return texto.PadRight(ancho);
    }

    private string AjustarTextoDerecha(string texto, int ancho)
    {
        if (texto.Length > ancho)
        {
            return texto.Substring(0, ancho);
        }

        return texto.PadRight(ancho);
    }

    private string AjustarNumero(int numero, int espaciosDerecha)
    {
        return numero.ToString().PadRight(espaciosDerecha);
    }

    private void EscribirColor(string texto, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(texto);
        Console.ResetColor();
    }

    private void EscribirColorInline(string texto, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.Write(texto);
        Console.ResetColor();
    }

    public void MostrarRecords(List<HundirLaFlota.Datos.EntradaMarcador> entradas)
    {
        EscribirColor(ArteAscii.LogoPrincipal, Colores.Titulo);
        Console.WriteLine(
            "╔════════════════════════════════════════════════════════════════════════════╗"
        );
        Console.WriteLine(
            "║                                RÉCORDS                                     ║"
        );
        Console.WriteLine(
            "╠════════════════════════════════════════════════════════════════════════════╣"
        );

        if (entradas.Count == 0)
        {
            Console.WriteLine(
                "║  No hay puntuaciones guardadas todavía.                                   ║"
            );
        }
        else
        {
            Console.WriteLine(
                "║  #  Jugador              Disparos   Precisión   Puntuación                ║"
            );
            Console.WriteLine(
                "╠════════════════════════════════════════════════════════════════════════════╣"
            );

            for (int i = 0; i < entradas.Count; i++)
            {
                string linea =
                    "  "
                    + (i + 1).ToString().PadRight(2)
                    + " "
                    + entradas[i].NombreJugador.PadRight(20)
                    + " "
                    + entradas[i].Disparos.ToString().PadRight(10)
                    + " "
                    + (entradas[i].Precision.ToString("F1") + " %").PadRight(11)
                    + " "
                    + entradas[i].Puntuacion.ToString("F2").PadRight(10);

                Console.WriteLine("║" + linea.PadRight(76) + "║");
            }
        }

        Console.WriteLine(
            "╚════════════════════════════════════════════════════════════════════════════╝"
        );
        Console.WriteLine();
    }
}
