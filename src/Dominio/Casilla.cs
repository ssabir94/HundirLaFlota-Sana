namespace HundirLaFlota.Dominio;

public class Casilla
{
    // Guardar fila de la casilla
    public int Fila { get; }

    // Guardar columna de la casilla
    public int Columna { get; }

    // Guardar barco asociado
    public Barco? Barco { get; set; }

    // Guardar si la casilla ha sido disparada
    public bool Disparada { get; set; }

    // Comprobar si la casilla está vacía
    public bool EstaVacia => Barco == null;

    // Comprobar si la casilla tiene impacto
    public bool EsImpacto => Disparada && Barco != null;

    // Comprobar si la casilla es agua
    public bool EsAgua => Disparada && Barco == null;

    // Inicializar casilla
    public Casilla(int fila, int columna)
    {
        Fila = fila;
        Columna = columna;
        Barco = null;
        Disparada = false;
    }
}