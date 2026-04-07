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
    public void MostrarTablerosBatalla(Tablero propio, Tablero enemigo)
    {
        // Definir letras de filas
        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        Console.WriteLine();
        Console.WriteLine("      TU TABLERO                         MAR ENEMIGO");
        Console.Write("      ");

        // Mostrar números del tablero propio
        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,3}");
        }

        Console.Write("      ");

        // Mostrar números del tablero enemigo
        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,3}");
        }

        Console.WriteLine();

        // Recorrer filas
        for (int f = 0; f < 10; f++)
        {
            Console.Write($"  {letras[f]}   ");

            // Imprimir tablero propio
            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(propio.ObtenerCasilla(f, c), true);
            }

            Console.Write($"      {letras[f]}   ");

            // Imprimir tablero enemigo
            for (int c = 0; c < 10; c++)
            {
                ImprimirCasilla(enemigo.ObtenerCasilla(f, c), false);
            }

            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("  [ S = barco propio   X = impacto   ~ = agua   . = vacío   # = hundido ]");
        Console.WriteLine();
    }

    // Mostrar tablero durante la colocación
    public void MostrarTableroColocacion(Tablero tablero, Barco barco)
    {
        Console.WriteLine();
        Console.WriteLine("Coloca tu " + barco.Nombre + " (" + barco.Tamanio + " casillas)");
        Console.WriteLine();

        string[] letras = { "A", "B", "C", "D", "E", "F", "G", "H", "I", "J" };

        Console.Write("      ");
        for (int c = 1; c <= 10; c++)
        {
            Console.Write($"{c,3}");
        }
        Console.WriteLine();

        for (int f = 0; f < 10; f++)
        {
            Console.Write($"  {letras[f]}   ");

            for (int c = 0; c < 10; c++)
            {
                Casilla casilla = tablero.ObtenerCasilla(f, c);
                ImprimirCasilla(casilla, true);
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

    // Pedir posición de colocación
    public string PedirPosicion(Barco barco)
    {
        Console.Write("Introduce posición para " + barco.Nombre + ": ");
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
        {
            Console.WriteLine("Disparo en " + coordenada + ": Agua");
        }
        else if (resultado == ResultadoDisparo.Impacto)
        {
            Console.WriteLine("Disparo en " + coordenada + ": Impacto");
        }
        else if (resultado == ResultadoDisparo.Hundido)
        {
            Console.WriteLine("Disparo en " + coordenada + ": Hundido");
        }
        else if (resultado == ResultadoDisparo.YaDisparado)
        {
            Console.WriteLine("Disparo en " + coordenada + ": Ya disparado");
        }
    }

    // Mostrar resultado del disparo de la CPU
    public void MostrarDisparoCpu(ResultadoDisparo resultado, int fila, int columna)
    {
        string coordenada = ConvertirACoordenadaHumana(fila, columna);

        if (resultado == ResultadoDisparo.Agua)
        {
            Console.WriteLine("La CPU dispara en " + coordenada + ": Agua");
        }
        else if (resultado == ResultadoDisparo.Impacto)
        {
            Console.WriteLine("La CPU dispara en " + coordenada + ": Impacto");
        }
        else if (resultado == ResultadoDisparo.Hundido)
        {
            Console.WriteLine("La CPU dispara en " + coordenada + ": Hundido");
        }
        else if (resultado == ResultadoDisparo.YaDisparado)
        {
            Console.WriteLine("La CPU dispara en " + coordenada + ": Ya disparado");
        }
    }

    // Mostrar resultado final de la partida
    public void MostrarResultadoFinal(bool ganaJugador, Jugador jugador)
    {
        Console.WriteLine();
        Console.WriteLine("=== FINAL DE PARTIDA ===");

        if (ganaJugador)
        {
            Console.WriteLine("¡Has ganado!");
        }
        else
        {
            Console.WriteLine("¡Has perdido!");
        }

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

    // Imprimir una casilla según su estado
    private void ImprimirCasilla(Casilla casilla, bool esPropio)
    {
        // Comprobar impacto
        if (casilla.EsImpacto)
        {
            if (casilla.Barco != null && casilla.Barco.EstaHundido)
            {
                Console.Write("  #");
            }
            else
            {
                Console.Write("  X");
            }
        }
        // Comprobar agua
        else if (casilla.EsAgua)
        {
            Console.Write("  ~");
        }
        // Mostrar barco solo en tablero propio
        else if (!casilla.EstaVacia && esPropio)
        {
            Console.Write("  S");
        }
        // Mostrar vacío
        else
        {
            Console.Write("  .");
        }
    }

    // Convertir fila y columna a coordenada humana
    private string ConvertirACoordenadaHumana(int fila, int columna)
    {
        char letra = (char)('A' + fila);
        return letra + (columna + 1).ToString();
    }
}