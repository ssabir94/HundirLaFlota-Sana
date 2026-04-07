using System.Collections.Generic;

namespace HundirLaFlota.Datos
{
    public class EstadoPartida
    {
        public string NombreJugador { get; set; } = "Jugador";

        public string FaseActual { get; set; } = "Colocacion";
        public string TurnoActual { get; set; } = "Jugador";

        public int DisparosJugador { get; set; }
        public int AciertosJugador { get; set; }
        public int FallosJugador { get; set; }

        public int DisparosCpu { get; set; }
        public int AciertosCpu { get; set; }
        public int FallosCpu { get; set; }

        public List<BarcoGuardado> BarcosJugador { get; set; } = new List<BarcoGuardado>();
        public List<BarcoGuardado> BarcosCpu { get; set; } = new List<BarcoGuardado>();

        public List<CasillaDisparadaGuardada> DisparosTableroJugador { get; set; } = new List<CasillaDisparadaGuardada>();
        public List<CasillaDisparadaGuardada> DisparosTableroCpu { get; set; } = new List<CasillaDisparadaGuardada>();

        public string Dificultad { get; set; } = "Normal";
    }

    public class BarcoGuardado
    {
        public string Nombre { get; set; } = "";
        public int Tamanio { get; set; }
        public int Impactos { get; set; }

        public List<CoordenadaGuardada> Coordenadas { get; set; } = new List<CoordenadaGuardada>();
    }

    public class CoordenadaGuardada
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
    }

    public class CasillaDisparadaGuardada
    {
        public int Fila { get; set; }
        public int Columna { get; set; }
        public bool TeniaBarco { get; set; }
    }
}