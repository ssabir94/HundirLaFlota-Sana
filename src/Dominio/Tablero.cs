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
        // Crear array de casillas
        casillas = new Casilla[10, 10];

        // Inicializar lista de barcos
        barcos = new List<Barco>();

        // Crear las 100 casillas del tablero
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
        // Recorrer tamaño del barco
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Calcular posición según orientación
            if (esHorizontal)
            {
                columnaActual += i;
            }
            else
            {
                filaActual += i;
            }

            // Comprobar salida del tablero
            if (filaActual < 0 || filaActual >= 10 || columnaActual < 0 || columnaActual >= 10)
            {
                return false;
            }

            // Comprobar casillas adyacentes, incluidas diagonales
            for (int f = filaActual - 1; f <= filaActual + 1; f++)
            {
                for (int c = columnaActual - 1; c <= columnaActual + 1; c++)
                {
                    // Comprobar límites antes de acceder
                    if (f >= 0 && f < 10 && c >= 0 && c < 10)
                    {
                        // Comprobar si ya hay barco
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
        // Comprobar si la colocación es válida
        if (!PuedeColocar(barco, fila, columna, esHorizontal))
        {
            return;
        }

        // Recorrer tamaño del barco
        for (int i = 0; i < barco.Tamanio; i++)
        {
            int filaActual = fila;
            int columnaActual = columna;

            // Calcular posición según orientación
            if (esHorizontal)
            {
                columnaActual += i;
            }
            else
            {
                filaActual += i;
            }

            // Obtener casilla actual
            Casilla casilla = casillas[filaActual, columnaActual];

            // Asignar barco a la casilla
            casilla.Barco = barco;

            // Guardar casilla en el barco
            barco.Casillas.Add(casilla);
        }

        // Guardar barco en el tablero
        barcos.Add(barco);
    }

    // Registrar disparo
    public ResultadoDisparo Disparar(int fila, int columna)
    {
        // Obtener casilla objetivo
        Casilla casilla = casillas[fila, columna];

        // Comprobar disparo repetido
        if (casilla.Disparada)
        {
            return ResultadoDisparo.YaDisparado;
        }

        // Marcar casilla como disparada
        casilla.Disparada = true;

        // Comprobar agua
        if (casilla.Barco == null)
        {
            return ResultadoDisparo.Agua;
        }

        // Registrar impacto en barco
        casilla.Barco.RecibirImpacto();

        // Comprobar hundimiento
        if (casilla.Barco.EstaHundido)
        {
            return ResultadoDisparo.Hundido;
        }

        return ResultadoDisparo.Impacto;
    }
}