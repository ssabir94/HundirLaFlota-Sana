namespace HundirLaFlota.Dominio;

public class Casilla
{
    public int Fila { get; }
    public int Columna { get; }

    public Barco? Barco { get; set; }
    public bool Disparada { get; set; }

    public bool EstaVacia => Barco == null;
    public bool EsImpacto => Disparada && Barco != null;
    public bool EsAgua => Disparada && Barco == null;

    public Casilla(int fila, int columna)
    {
        Fila = fila;
        Columna = columna;
    }
}