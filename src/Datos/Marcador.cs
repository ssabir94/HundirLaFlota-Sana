using System.Text.Json;

namespace HundirLaFlota.Datos;

public class Marcador
{
    private const string RutaArchivo = "marcador.json";
    private const int MaxEntradas = 10;

    public List<EntradaMarcador> Cargar()
    {
        try
        {
            if (!File.Exists(RutaArchivo))
            {
                return new List<EntradaMarcador>();
            }

            string json = File.ReadAllText(RutaArchivo);
            List<EntradaMarcador>? entradas = JsonSerializer.Deserialize<List<EntradaMarcador>>(json);

            if (entradas == null)
            {
                return new List<EntradaMarcador>();
            }

            return entradas;
        }
        catch
        {
            return new List<EntradaMarcador>();
        }
    }

    public void Guardar(List<EntradaMarcador> entradas)
    {
        JsonSerializerOptions opciones = new JsonSerializerOptions();
        opciones.WriteIndented = true;

        string json = JsonSerializer.Serialize(entradas, opciones);
        File.WriteAllText(RutaArchivo, json);
    }

    public void AgregarEntrada(string nombreJugador, int disparos, double precision)
    {
        List<EntradaMarcador> entradas = Cargar();

        EntradaMarcador nuevaEntrada = new EntradaMarcador();
        nuevaEntrada.NombreJugador = nombreJugador;
        nuevaEntrada.Disparos = disparos;
        nuevaEntrada.Precision = precision;
        nuevaEntrada.Puntuacion = CalcularPuntuacion(disparos, precision);

        entradas.Add(nuevaEntrada);

        entradas = entradas
            .OrderByDescending(e => e.Puntuacion)
            .ThenBy(e => e.Disparos)
            .ToList();

        if (entradas.Count > MaxEntradas)
        {
            entradas = entradas.Take(MaxEntradas).ToList();
        }

        Guardar(entradas);
    }

    public double CalcularPuntuacion(int disparos, double precision)
    {
        return precision * (100.0 / Math.Max(disparos, 1));
    }
}