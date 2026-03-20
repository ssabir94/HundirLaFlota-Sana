namespace HundirLaFlota.Dominio;

public class Barco
{
    public string Nombre { get; set; }
    public int Tamanio { get; set; }
    public int Impactos { get; private set; }
    public List<Casilla> Casillas { get; set; }

    public bool EstaHundido => Impactos >= Tamanio;

    public Barco(string nombre, int tamanio)
    {
        Nombre = nombre;
        Tamanio = tamanio;
        Impactos = 0;
        Casillas = new List<Casilla>();
    }

    public void RecibirImpacto()
    {
        Impactos++;
    }
}