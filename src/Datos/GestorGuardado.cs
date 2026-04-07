using System;
using System.IO;
using System.Text.Json;

namespace HundirLaFlota.Datos
{
    public class GestorGuardado
    {
        private const string RutaArchivo = "partida_guardada.json";

        public bool ExistePartidaGuardada
        {
            get { return File.Exists(RutaArchivo); }
        }

        public void Guardar(EstadoPartida estado)
        {
            try
            {
                JsonSerializerOptions opciones = new JsonSerializerOptions
                {
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(estado, opciones);
                File.WriteAllText(RutaArchivo, json);
            }
            catch
            {
            }
        }

        public EstadoPartida? Cargar()
        {
            try
            {
                if (!File.Exists(RutaArchivo))
                {
                    return null;
                }

                string json = File.ReadAllText(RutaArchivo);
                EstadoPartida? estado = JsonSerializer.Deserialize<EstadoPartida>(json);

                return estado;
            }
            catch
            {
                return null;
            }
        }

        public void EliminarGuardado()
        {
            try
            {
                if (File.Exists(RutaArchivo))
                {
                    File.Delete(RutaArchivo);
                }
            }
            catch
            {
            }
        }
    }
}