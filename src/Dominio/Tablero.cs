namespace HundirLaFlota.Dominio;

public class Tablero
{
    // Guardar casillas del tablero
    private Casilla[,] casillas;

    // Guardar barcos colocados
    private List<Barco> barcos;

    // Comprobar si todos los barcos están hundidos
    public bool TodosHundidos
    {
        get
        {
            return barcos.Count > 0 && barcos.All(b => b.EstaHundido);
        }
    }

    // Contar barcos que siguen a flote
    public int BarcosRestantes
    {
        get
        {
            return barcos.Count(b => !b.EstaHundido);
        }
    }

    // Inicializar tablero
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

    // Devolver casilla concreta
    public Casilla ObtenerCasilla(int fila, int columna)
    {
        return casillas[fila, columna];
    }

    // Comprobar si un barco se puede colocar
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

    // Colocar barco en tablero
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

    // Registrar disparo
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

    // Restaurar un barco en una casilla concreta
    public void AsignarBarcoEnCasilla(int fila, int columna, Barco barco)
    {
        Casilla casilla = casillas[fila, columna];
        casilla.Barco = barco;

        if (!barco.Casillas.Contains(casilla))
        {
            barco.Casillas.Add(casilla);
        }

        if (!barcos.Contains(barco))
        {
            barcos.Add(barco);
        }
    }

    // Marcar una casilla como disparada al cargar partida
    public void MarcarCasillaDisparada(int fila, int columna)
    {
        casillas[fila, columna].Disparada = true;
    }
}