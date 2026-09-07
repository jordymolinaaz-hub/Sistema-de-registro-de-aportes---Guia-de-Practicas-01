using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PremiacionDeportistas
{
    // Clase que representa a un deportista participante
    class Deportista
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Disciplina { get; set; }
        public string PaisOEquipo { get; set; }
        public double Puntos { get; set; }

        public Deportista(int id, string nombre, string disciplina, string pais, double puntos)
        {
            Id = id;
            Nombre = nombre;
            Disciplina = disciplina;
            PaisOEquipo = pais;
            Puntos = puntos;
        }

        public override string ToString()
        {
            return $"[{Id}] {Nombre} - {Disciplina} - {PaisOEquipo} - {Puntos:0.0} pts";
        }
    }

    class Program
    {
        // 1) CONJUNTO (HashSet<T>): garantiza disciplinas únicas, sin duplicados.
        static HashSet<string> conjuntoDisciplinas = new HashSet<string>();

        // 2) MAPA (Dictionary<TKey, TValue>): asocia cada Id único al deportista -> acceso O(1).
        static Dictionary<int, Deportista> mapaDeportistas = new Dictionary<int, Deportista>();

        // 3) DICCIONARIO agrupador (Dictionary<string, List<Deportista>>): agrupa deportistas por disciplina
        //    para poder generar el medallero (reportería) de cada disciplina.
        static Dictionary<string, List<Deportista>> diccionarioPorDisciplina =
            new Dictionary<string, List<Deportista>>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== SISTEMA DE PREMIACIÓN DE DEPORTISTAS DE VARIAS DISCIPLINAS ===\n");

            // --------- Datos de prueba (registro de deportistas) ---------
            var datos = new List<Deportista>
            {
                new Deportista(1,  "Ana Torres",    "Atletismo", "Ecuador",  95),
                new Deportista(2,  "Carlos Pérez",  "Atletismo", "Colombia", 88),
                new Deportista(3,  "Lucía Gómez",   "Atletismo", "Ecuador",  92),
                new Deportista(4,  "Pedro Ruiz",    "Natación",  "Perú",     90),
                new Deportista(5,  "María Salas",   "Natación",  "Ecuador",  85),
                new Deportista(6,  "Jorge León",    "Natación",  "Chile",    93),
                new Deportista(7,  "Daniela Vera",  "Ciclismo",  "Ecuador",  78),
                new Deportista(8,  "Andrés Mora",   "Ciclismo",  "Colombia", 82),
                new Deportista(9,  "Paula Ríos",    "Ciclismo",  "Perú",     80),
                new Deportista(10, "Sofía Cedeño",  "Gimnasia",  "Ecuador",  97),
                new Deportista(11, "Iván Castro",   "Gimnasia",  "Chile",    91),
                new Deportista(12, "Karen Ortiz",   "Gimnasia",  "Ecuador",  89),
            };

            // --------- Medición de tiempo de ejecución: registro / inserción ---------
            Stopwatch cronometro = Stopwatch.StartNew();

            foreach (var d in datos)
            {
                RegistrarDeportista(d);
            }

            cronometro.Stop();
            double tiempoRegistro = cronometro.Elapsed.TotalMilliseconds;

            // --------- Reportería 1: disciplinas registradas (desde el CONJUNTO) ---------
            Console.WriteLine("--- Disciplinas registradas (Conjunto - HashSet<string>) ---");
            foreach (var disciplina in conjuntoDisciplinas.OrderBy(d => d))
            {
                Console.WriteLine($" - {disciplina}");
            }

            // --------- Reportería 2: listado general de deportistas (desde el MAPA) ---------
            Console.WriteLine("\n--- Listado general de deportistas (Mapa - Dictionary<int, Deportista>) ---");
            foreach (var id in mapaDeportistas.Keys.OrderBy(k => k))
            {
                Console.WriteLine(" " + mapaDeportistas[id]);
            }

            // --------- Reportería 3: medallero por disciplina (DICCIONARIO agrupador) ---------
            Console.WriteLine("\n--- Medallero por disciplina (Diccionario<string, List<Deportista>>) ---");
            foreach (var disciplina in diccionarioPorDisciplina.Keys.OrderBy(d => d))
            {
                Console.WriteLine($"\nDisciplina: {disciplina}");
                var top3 = diccionarioPorDisciplina[disciplina]
                            .OrderByDescending(x => x.Puntos)
                            .Take(3)
                            .ToList();

                string[] medallas = { "🥇 Oro   ", "🥈 Plata ", "🥉 Bronce" };
                for (int i = 0; i < top3.Count; i++)
                {
                    Console.WriteLine($"   {medallas[i]}: {top3[i].Nombre} ({top3[i].PaisOEquipo}) - {top3[i].Puntos:0.0} pts");
                }
            }

            // --------- Búsqueda puntual por Id (demuestra acceso O(1) en el Mapa) ---------
            Console.WriteLine("\n--- Búsqueda puntual por Id (Mapa) ---");
            cronometro.Restart();
            bool encontrado = mapaDeportistas.TryGetValue(5, out Deportista buscado);
            cronometro.Stop();
            double tiempoBusqueda = cronometro.Elapsed.TotalMilliseconds;

            if (encontrado)
                Console.WriteLine($"Id 5 encontrado -> {buscado}");
            else
                Console.WriteLine("Id 5 no encontrado.");

            // Búsqueda de un Id inexistente para mostrar el otro caso
            bool existeId99 = mapaDeportistas.ContainsKey(99);
            Console.WriteLine($"¿Existe el Id 99? {(existeId99 ? "Sí" : "No")}");

            // --------- Análisis de tiempos de ejecución ---------
            Console.WriteLine("\n--- Análisis de tiempo de ejecución ---");
            Console.WriteLine($"Tiempo de registro de {datos.Count} deportistas: {tiempoRegistro:0.0000} ms");
            Console.WriteLine($"Tiempo de búsqueda por Id (Dictionary, O(1) promedio): {tiempoBusqueda:0.0000} ms");
            Console.WriteLine("\nNota: los tiempos exactos varían según el equipo donde se ejecute el programa;");
            Console.WriteLine("lo relevante es el orden de magnitud y la complejidad algorítmica de cada operación.");

            Console.WriteLine("\n=== FIN DE LA EJECUCIÓN ===");
        }

        // Registra un deportista en las tres estructuras de datos
        static void RegistrarDeportista(Deportista d)
        {
            // 1) Conjunto: agrega la disciplina; si ya existe, HashSet la ignora automáticamente (sin duplicados)
            conjuntoDisciplinas.Add(d.Disciplina);

            // 2) Mapa: asocia el Id (clave única) con el objeto Deportista completo
            mapaDeportistas[d.Id] = d;

            // 3) Diccionario agrupador: agrupa por disciplina para la reportería del medallero
            if (!diccionarioPorDisciplina.ContainsKey(d.Disciplina))
            {
                diccionarioPorDisciplina[d.Disciplina] = new List<Deportista>();
            }
            diccionarioPorDisciplina[d.Disciplina].Add(d);
        }
    }
}