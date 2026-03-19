namespace HundirLaFlota.Dominio;

public class Barco
{
    public string Nombre { get; set; }
    public int Tamanio { get; set; }

    private int impactos;
    public List<Casilla> Casillas { get; set; }

    public bool EstaHundido => impactos >= Tamanio;

    public Barco(string nombre, int tamanio)
    {
        Nombre = nombre;
        Tamanio = tamanio;
        Casillas = new List<Casilla>();
        impactos = 0;
    }

    public void RecibirImpacto()
    {
        impactos++;
    }
}