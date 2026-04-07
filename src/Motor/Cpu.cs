using HundirLaFlota.Dominio;

namespace HundirLaFlota.Motor;

public class Cpu : Jugador
{
    // Guardar objetivos disponibles
    private List<(int fila, int columna)> objetivos;

    // Guardar generador aleatorio
    private Random random;

    // Inicializar CPU
    public Cpu(string nombre) : base(nombre)
    {
        // Inicializar lista de objetivos
        objetivos = new List<(int fila, int columna)>();

        // Inicializar random
        random = new Random();

        // Añadir todas las coordenadas del tablero
        for (int fila = 0; fila < 10; fila++)
        {
            for (int columna = 0; columna < 10; columna++)
            {
                objetivos.Add((fila, columna));
            }
        }

        // Barajar coordenadas con Fisher-Yates
        for (int i = objetivos.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);

            (int fila, int columna) temp = objetivos[i];
            objetivos[i] = objetivos[j];
            objetivos[j] = temp;
        }
    }

    // Colocar flota de forma aleatoria
    public void ColocarFlotaAleatoria(List<Barco> barcos)
    {
        // Recorrer barcos de la flota
        foreach (Barco barco in barcos)
        {
            bool colocado = false;

            // Repetir hasta encontrar posición válida
            while (!colocado)
            {
                // Generar fila aleatoria
                int fila = random.Next(0, 10);

                // Generar columna aleatoria
                int columna = random.Next(0, 10);

                // Generar orientación aleatoria
                bool esHorizontal = random.Next(0, 2) == 0;

                // Comprobar si el barco se puede colocar
                if (Tablero.PuedeColocar(barco, fila, columna, esHorizontal))
                {
                    // Colocar barco en tablero
                    Tablero.ColocarBarco(barco, fila, columna, esHorizontal);

                    // Marcar barco como colocado
                    colocado = true;
                }
            }
        }
    }

    // Elegir siguiente objetivo
    public (int fila, int columna) ElegirObjetivo()
    {
        // Obtener primer objetivo disponible
        (int fila, int columna) objetivo = objetivos[0];

        // Eliminar objetivo usado
        objetivos.RemoveAt(0);

        return objetivo;
    }

    public void EliminarObjetivoUsado(int fila, int columna)
{
    objetivos.Remove((fila, columna));
}

}