using HundirLaFlota.Dominio;

namespace HundirLaFlota.Presentacion;

public class Renderizador
{
    public void MostrarTablero(Tablero tablero)
    {
        // Números columna
        Console.Write("  ");
        for (int i = 1; i <= 10; i++)
        {
            Console.Write(i + " ");
        }
        Console.WriteLine();

        // Files A-J
        for (int fila = 0; fila < 10; fila++)
        {
            char letra = (char)('A' + fila);
            Console.Write(letra + " ");

            for (int columna = 0; columna < 10; columna++)
            {
                Casilla casilla = tablero.ObtenerCasilla(fila, columna);

                char simbolo = '.';

                if (casilla.Disparada)
                {
                    if (casilla.Barco == null)
                        simbolo = '~';
                    else
                        simbolo = 'X';
                }
                else
                {
                    if (casilla.Barco != null)
                        simbolo = 'S';
                }

                Console.Write(simbolo + " ");
            }

            Console.WriteLine();
        }
    }
}