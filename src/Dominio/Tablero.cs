namespace HundirLaFlota.Dominio;

public class Tablero
{
    private Casilla[,] casillas;
    private List<Barco> barcos;

    public bool TodosHundidos => false;
    public int BarcosRestantes => 0;

    public Tablero()
    {
        casillas = new Casilla[10, 10];
        barcos = new List<Barco>();
    }

    public Casilla ObtenerCasilla(int fila, int columna)
    {
        return casillas[fila, columna];
    }

    public bool PuedeColocar(Barco barco, int fila, int columna, bool esHorizontal)
    {
        return false;
    }

    public void ColocarBarco(Barco barco, int fila, int columna, bool esHorizontal)
    {
    }

    public ResultadoDisparo Disparar(int fila, int columna)
    {
        return ResultadoDisparo.Agua;
    }
}