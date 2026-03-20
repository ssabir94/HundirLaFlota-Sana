namespace HundirLaFlota.Dominio;

public class Tablero
{
    private Casilla[,] casillas;
    private List<Barco> barcos;

    public bool TodosHundidos => barcos.Count > 0 && barcos.All(b => b.EstaHundido);
    public int BarcosRestantes => barcos.Count(b => !b.EstaHundido);

    public Tablero()
    {
        casillas = new Casilla[10, 10];
        barcos = new List<Barco>();

        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                casillas[fila, columna] = new Casilla(fila, columna);
            }
        }
    }

    public Casilla ObtenerCasilla(int fila, int columna)
    {
        return casillas[fila, columna];
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            if (esHorizontal)
            {
                columnaActual += i;
            }
            else
            {
                filaActual += i;
            }

            if (filaActual < 0 || filaActual >= 10 || columnaActual < 0 || columnaActual >= 10)
            {
                return false;
            }

            for (int f = filaActual - 1; f <= filaActual + 1; f++)
            {
                for (int c = columnaActual - 1; c <= columnaActual + 1; c++)
                {
                    if (f >= 0 && f < 10 && c >= 0 && c < 10)
                    {
                        if (casillas[f, c].Barco != null)
                        {
                            return false;
                        }
                    }
                }
            }
        }

        return true;
    }

    public void ColocarBarco(Barco barco, int fila, int columna, bool esHorizontal)
    {
        if (!PuedeColocar(barco, fila, columna, esHorizontal))
        {
            return;
        }

        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            if (esHorizontal)
            {
                columnaActual += i;
            }
            else
            {
                filaActual += i;
            }

            Casilla casilla = casillas[filaActual, columnaActual];
            casilla.Barco = barco;
            barco.Casillas.Add(casilla);
        }

        barcos.Add(barco);
    }

    public ResultadoDisparo Disparar(int fila, int columna)
    {
        Casilla casilla = casillas[fila, columna];

        if (casilla.Disparada)
        {
            return ResultadoDisparo.YaDisparado;
        }

        casilla.Disparada = true;

        if (casilla.Barco == null)
        {
            return ResultadoDisparo.Agua;
        }

        casilla.Barco.RecibirImpacto();

        if (casilla.Barco.EstaHundido)
        {
            return ResultadoDisparo.Hundido;
        }

        return ResultadoDisparo.Impacto;
    }
}