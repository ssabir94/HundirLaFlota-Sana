// using HundirLaFlota.Motor;

// internal class Program
// {
//     static void Main(string[] args)
//     {
//         Juego juego = new Juego();
//         juego.Iniciar();

//         Console.WriteLine("Proyecto iniciado correctamente");
//     }
// }









//ESTO ES PARA PROBAR LA RENDERIZACIÓN DE LA FASE 2!!!!!! :)


// using HundirLaFlota.Dominio;
// using HundirLaFlota.Presentacion;

// class Program
// {
//     static void Main()
//     {
//         Tablero tablero = new Tablero();
//         Renderizador render = new Renderizador();

//         // Crear flota
//         var barcos = Flota.CrearFlota();

//         // Colocar un parell de barcos manualment
//         tablero.ColocarBarco(barcos[0], 1, 1, true); // portaaviones
//         tablero.ColocarBarco(barcos[1], 4, 2, false); // acorazado

//         // Simular disparos
//         tablero.Disparar(3, 2);
//         tablero.Disparar(4, 2);

//         // Mostrar tablero
//         render.MostrarTablero(tablero);
//     }
// }




//ESTO ES PARA PROBAR LA FASE 3!!!! :P

using HundirLaFlota.Motor;

internal class Program
{
    static void Main(string[] args)
    {
        Juego juego = new Juego();
        juego.Iniciar();
    }
}

