namespace HundirLaFlota.Dominio;

public class Barco
{
    // Guardar nombre del barco
    public string Nombre { get; set; }

    // Guardar tamaño del barco
    public int Tamanio { get; set; }

    // Guardar impactos recibidos
    public int Impactos { get; private set; }

    // Guardar casillas ocupadas
    public List<Casilla> Casillas { get; set; }

    // Comprobar si el barco está hundido
    public bool EstaHundido => Impactos >= Tamanio;

    // Inicializar barco
    public Barco(string nombre, int tamanio)
    {
        Nombre = nombre;
        Tamanio = tamanio;
        Impactos = 0;
        Casillas = new List<Casilla>();
    }

    // Registrar impacto
    public void RecibirImpacto()
    {
        Impactos++;
    }
}