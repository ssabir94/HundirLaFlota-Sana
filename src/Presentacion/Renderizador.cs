using HundirLaFlota.Dominio;
using HundirLaFlota.Motor;

namespace HundirLaFlota.Presentacion;

public class Renderizador
{
    // Mostrar pantalla de bienvenida
    public void MostrarBienvenida()
    {
        Console.WriteLine("==================================");
        Console.WriteLine("        HUNDIR LA FLOTA");
        Console.WriteLine("==================================");
        Console.WriteLine();
    }

    // Mostrar los dos tableros de batalla
    public void MostrarTablerosBatalla(Tablero propio, Tablero enemigo, Jugador jugador)
    {
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        Console.WriteLine();
        Console.WriteLine("TU TABLERO".PadRight(38) + "MAR ENEMIGO");
        Console.Write("    ");

        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,2} ");
        }

        Console.Write("      ");

        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,2} ");
        }

        Console.WriteLine();

        for (int f = 0; f < 10; f++)
        {
            Console.Write($"{letras[f]}   ");

            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(propio.ObtenerCasilla(f, c), true);
            }

            Console.Write("    ");
            Console.Write($"{letras[f]}   ");

            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(enemigo.ObtenerCasilla(f, c), false);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("[ S = barco propio   X = impacto   ~ = agua   . = vacío   # = hundido ]");
        Console.WriteLine();
        Console.WriteLine("Disparos: " + jugador.Disparos +
                          "   Aciertos: " + jugador.Aciertos +
                          "   Fallos: " + jugador.Fallos +
                          "   Precisión: " + jugador.Precision.ToString("F2") + "%");
        Console.WriteLine("Barcos propios restantes: " + propio.BarcosRestantes +
                          "   Barcos enemigos restantes: " + enemigo.BarcosRestantes);
        Console.WriteLine();
    }

    // Mostrar tablero durante la colocación
    public void MostrarTableroColocacion(Tablero tablero, Barco barco)
    {
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        Console.WriteLine();

        Console.WriteLine("Coloca tu " + barco.Nombre + " (" + barco.Tamanio + " casillas)");
        Console.WriteLine();

        Console.Write("    ");
        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,2} ");
        }
        Console.WriteLine();

        for (int f = 0; f < 10; f++)
        {
            Console.Write($"{letras[f]}   ");

            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(tablero.ObtenerCasilla(f, c), true);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    // Pedir coordenada de disparo
    public string PedirCoordenada()
    {
        Console.Write("Coordenada (ej. B7): ");
        string? entrada = Console.ReadLine();

        if (entrada == null)
        {
            return "";
        }

        return entrada.Trim().ToUpper();
    }

    // Mostrar resultado del disparo del jugador
    public void MostrarResultadoDisparo(ResultadoDisparo resultado, int fila, int columna)
    {
        string coordenada = ConvertirACoordenadaHumana(fila, columna);

        if (resultado == ResultadoDisparo.Agua)
            Console.WriteLine("Disparo en " + coordenada + ": Agua");
        else if (resultado == ResultadoDisparo.Impacto)
            Console.WriteLine("Disparo en " + coordenada + ": Impacto");
        else if (resultado == ResultadoDisparo.Hundido)
            Console.WriteLine("Disparo en " + coordenada + ": Hundido");
        else
            Console.WriteLine("Disparo en " + coordenada + ": Ya disparado");
    }

    // Mostrar resultado del disparo de la CPU
    public void MostrarDisparoCpu(ResultadoDisparo resultado, int fila, int columna)
    {
        string coordenada = ConvertirACoordenadaHumana(fila, columna);

        if (resultado == ResultadoDisparo.Agua)
            Console.WriteLine("La CPU dispara en " + coordenada + ": Agua");
        else if (resultado == ResultadoDisparo.Impacto)
            Console.WriteLine("La CPU dispara en " + coordenada + ": Impacto");
        else if (resultado == ResultadoDisparo.Hundido)
            Console.WriteLine("La CPU dispara en " + coordenada + ": Hundido");
        else
            Console.WriteLine("La CPU dispara en " + coordenada + ": Ya disparado");
    }

    // Mostrar resultado final de la partida
    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador)
    {
        Console.WriteLine();
        Console.WriteLine("=== FINAL DE PARTIDA ===");

        if (ganaJugador)
            Console.WriteLine("¡Has ganado!");
        else
            Console.WriteLine("¡Has perdido!");

        Console.WriteLine("Disparos: " + jugador.Disparos);
        Console.WriteLine("Aciertos: " + jugador.Aciertos);
        Console.WriteLine("Fallos: " + jugador.Fallos);
        Console.WriteLine("Precisión: " + jugador.Precision.ToString("F2") + "%");
    }

    // Mostrar mensaje de error
    public void MostrarError(string mensaje)
    {
        Console.WriteLine("Error: " + mensaje);
    }

    // Imprimir una casilla con ancho fijo
    private void ImprimirCasilla(Casilla casilla, bool esPropio)
    {
        string simbolo = ".";

        if (casilla.EsImpacto)
        {
            if (casilla.Barco != null && casilla.Barco.EstaHundido)
                simbolo = "#";
            else
                simbolo = "X";
        }
        else if (casilla.EsAgua)
        {
            simbolo = "~";
        }
        else if (!casilla.EstaVacia && esPropio)
        {
            simbolo = "S";
        }

        Console.Write($"{simbolo,2} ");
    }

    // Convertir fila y columna a coordenada humana
    private string ConvertirACoordenadaHumana(int fila, int columna)
    {
        char letra = (char)('A' + fila);
        return letra + (columna + 1).ToString();
    }
}

