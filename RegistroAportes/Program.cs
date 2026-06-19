using System;
using System.Collections.Generic;

// =====================================================================
//  GUÍA DE PRÁCTICAS #01 - Estructura de Datos
//  Universidad Estatal Amazónica
//  Sistema: Registro de Aportes de Integrantes de una Asociación
//  Lenguaje: C#  |  Paradigma: Programación Orientada a Objetos
//  Estructuras usadas: struct, array (vector), List<T> (arreglo dinámico)
// =====================================================================

namespace RegistroAportes
{
    // ------------------------------------------------------------------
    // STRUCT: representa un aporte individual (registro de dato compuesto)
    // ------------------------------------------------------------------
    struct Aporte
    {
        public int    Id;
        public string Concepto;
        public double Monto;
        public string Fecha;       // formato DD/MM/YYYY

        public Aporte(int id, string concepto, double monto, string fecha)
        {
            Id       = id;
            Concepto = concepto;
            Monto    = monto;
            Fecha    = fecha;
        }

        public override string ToString()
        {
            return $"  [{Id:D3}] {Concepto,-25} ${Monto,8:F2}   {Fecha}";
        }
    }

    // ------------------------------------------------------------------
    // CLASE: Empleado  (encapsula datos y comportamientos del empleado)
    // ------------------------------------------------------------------
    class Empleado
    {
        // --- Atributos ---
        public int    Id       { get; private set; }
        public string Nombre   { get; private set; }
        public string Cedula   { get; private set; }
        public string Cargo    { get; private set; }

        // Vector (List<T>) de aportes del empleado
        private List<Aporte> _aportes;

        // --- Constructor ---
        public Empleado(int id, string nombre, string cedula, string cargo)
        {
            Id      = id;
            Nombre  = nombre;
            Cedula  = cedula;
            Cargo   = cargo;
            _aportes = new List<Aporte>();
        }

        // --- Métodos ---

        /// <summary>Agrega un aporte al vector del empleado.</summary>
        public void AgregarAporte(string concepto, double monto, string fecha)
        {
            int nuevoId = _aportes.Count + 1;
            _aportes.Add(new Aporte(nuevoId, concepto, monto, fecha));
            Console.WriteLine($"\n  ✔ Aporte registrado exitosamente para {Nombre}.");
        }

        /// <summary>Devuelve la suma total de aportes.</summary>
        public double TotalAportes()
        {
            double total = 0;
            foreach (Aporte a in _aportes)
                total += a.Monto;
            return total;
        }

        /// <summary>Muestra todos los aportes del empleado.</summary>
        public void MostrarAportes()
        {
            if (_aportes.Count == 0)
            {
                Console.WriteLine("  (Sin aportes registrados)");
                return;
            }
            Console.WriteLine($"  {"ID",-6} {"Concepto",-25} {"Monto",9}   {"Fecha"}");
            Console.WriteLine("  " + new string('-', 55));
            foreach (Aporte a in _aportes)
                Console.WriteLine(a.ToString());
            Console.WriteLine("  " + new string('-', 55));
            Console.WriteLine($"  {"TOTAL",-32} ${TotalAportes(),8:F2}");
        }

        /// <summary>Busca un aporte por concepto (búsqueda lineal).</summary>
        public void BuscarAporte(string concepto)
        {
            bool encontrado = false;
            foreach (Aporte a in _aportes)
            {
                if (a.Concepto.ToLower().Contains(concepto.ToLower()))
                {
                    Console.WriteLine(a.ToString());
                    encontrado = true;
                }
            }
            if (!encontrado)
                Console.WriteLine($"  No se encontró ningún aporte con el concepto '{concepto}'.");
        }

        public override string ToString()
        {
            return $"  [{Id:D3}] {Nombre,-25} CI: {Cedula,-12} Cargo: {Cargo,-20} Aportes: {_aportes.Count,3}   Total: ${TotalAportes():F2}";
        }
    }

    // ------------------------------------------------------------------
    // CLASE: Asociacion  (gestiona el vector de empleados)
    // ------------------------------------------------------------------
    class Asociacion
    {
        private string      _nombre;
        private List<Empleado> _empleados;   // vector dinámico de empleados

        public Asociacion(string nombre)
        {
            _nombre    = nombre;
            _empleados = new List<Empleado>();
        }

        // ---- Registro de empleados ----

        public void RegistrarEmpleado(string nombre, string cedula, string cargo)
        {
            // Verificar cédula duplicada
            foreach (Empleado e in _empleados)
            {
                if (e.Cedula == cedula)
                {
                    Console.WriteLine($"\n  ⚠ Ya existe un empleado con la cédula {cedula}.");
                    return;
                }
            }
            int id = _empleados.Count + 1;
            _empleados.Add(new Empleado(id, nombre, cedula, cargo));
            Console.WriteLine($"\n  ✔ Empleado '{nombre}' registrado con ID {id:D3}.");
        }

        // ---- Búsqueda de empleado por ID ----

        private Empleado BuscarEmpleadoPorId(int id)
        {
            foreach (Empleado e in _empleados)
                if (e.Id == id) return e;
            return null;
        }

        // ---- Registro de aporte ----

        public void RegistrarAporte(int idEmpleado, string concepto, double monto, string fecha)
        {
            Empleado emp = BuscarEmpleadoPorId(idEmpleado);
            if (emp == null)
            {
                Console.WriteLine($"\n  ⚠ No existe un empleado con ID {idEmpleado:D3}.");
                return;
            }
            emp.AgregarAporte(concepto, monto, fecha);
        }

        // ---- Reportes ----

        public void ListarEmpleados()
        {
            Console.WriteLine($"\n  === LISTADO DE EMPLEADOS — {_nombre} ===");
            if (_empleados.Count == 0)
            {
                Console.WriteLine("  (No hay empleados registrados)");
                return;
            }
            Console.WriteLine($"  {"ID",-6} {"Nombre",-25} {"Cédula",-14} {"Cargo",-22} {"Aportes",8}   {"Total"}");
            Console.WriteLine("  " + new string('-', 85));
            foreach (Empleado e in _empleados)
                Console.WriteLine(e.ToString());
        }

        public void MostrarAportesEmpleado(int idEmpleado)
        {
            Empleado emp = BuscarEmpleadoPorId(idEmpleado);
            if (emp == null)
            {
                Console.WriteLine($"\n  ⚠ No existe un empleado con ID {idEmpleado:D3}.");
                return;
            }
            Console.WriteLine($"\n  === APORTES DE: {emp.Nombre} (CI: {emp.Cedula}) ===");
            emp.MostrarAportes();
        }

        public void ReporteGeneral()
        {
            Console.WriteLine($"\n  === REPORTE GENERAL DE APORTES — {_nombre} ===");
            double granTotal = 0;
            foreach (Empleado e in _empleados)
            {
                Console.WriteLine($"\n  Empleado: {e.Nombre} (ID {e.Id:D3})");
                e.MostrarAportes();
                granTotal += e.TotalAportes();
            }
            Console.WriteLine($"\n  {'=',5} GRAN TOTAL DE APORTES: ${granTotal:F2} {'=',5}");
        }

        public void BuscarAporteEmpleado(int idEmpleado, string concepto)
        {
            Empleado emp = BuscarEmpleadoPorId(idEmpleado);
            if (emp == null) { Console.WriteLine($"\n  ⚠ No existe empleado con ID {idEmpleado:D3}."); return; }
            Console.WriteLine($"\n  === Búsqueda '{concepto}' en aportes de {emp.Nombre} ===");
            emp.BuscarAporte(concepto);
        }

        // ---- Reporte: matriz de aportes por empleado (mes x empleado) ----
        // Muestra cuántos aportes registró cada empleado (estructura tipo matriz)
        public void MatrizResumen()
        {
            Console.WriteLine($"\n  === MATRIZ RESUMEN (Empleado vs Total $) ===");
            Console.WriteLine($"  {"#",-4} {"Nombre",-25} {"Total Aportes ($)",18}");
            Console.WriteLine("  " + new string('-', 50));
            foreach (Empleado e in _empleados)
                Console.WriteLine($"  {e.Id,-4} {e.Nombre,-25} {e.TotalAportes(),18:F2}");
        }
    }

    // ------------------------------------------------------------------
    // CLASE: Program  (menú principal e interacción con el usuario)
    // ------------------------------------------------------------------
    class Program
    {
        static Asociacion asociacion = new Asociacion("Asociación de Empleados UEA");

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CargarDatosDemostracion();   // datos de ejemplo
            MenuPrincipal();
        }

        // ---- Menú principal ----
        static void MenuPrincipal()
        {
            bool salir = false;
            while (!salir)
            {
                Console.WriteLine("\n╔══════════════════════════════════════════╗");
                Console.WriteLine("║  SISTEMA DE REGISTRO DE APORTES          ║");
                Console.WriteLine("║  Asociación de Empleados UEA             ║");
                Console.WriteLine("╠══════════════════════════════════════════╣");
                Console.WriteLine("║  1. Registrar empleado                   ║");
                Console.WriteLine("║  2. Registrar aporte                     ║");
                Console.WriteLine("║  3. Listar empleados                     ║");
                Console.WriteLine("║  4. Ver aportes de un empleado           ║");
                Console.WriteLine("║  5. Reporte general                      ║");
                Console.WriteLine("║  6. Buscar aporte por concepto           ║");
                Console.WriteLine("║  7. Matriz resumen                       ║");
                Console.WriteLine("║  0. Salir                                ║");
                Console.WriteLine("╚══════════════════════════════════════════╝");
                Console.Write("  Seleccione una opción: ");
                string op = Console.ReadLine();

                switch (op)
                {
                    case "1": OpRegistrarEmpleado();         break;
                    case "2": OpRegistrarAporte();           break;
                    case "3": asociacion.ListarEmpleados();  break;
                    case "4": OpVerAportes();                break;
                    case "5": asociacion.ReporteGeneral();   break;
                    case "6": OpBuscarAporte();              break;
                    case "7": asociacion.MatrizResumen();    break;
                    case "0": salir = true; Console.WriteLine("\n  Hasta pronto.\n"); break;
                    default:  Console.WriteLine("\n  ⚠ Opción inválida."); break;
                }
            }
        }

        // ---- Opciones ----

        static void OpRegistrarEmpleado()
        {
            Console.WriteLine("\n  -- Registrar nuevo empleado --");
            Console.Write("  Nombre completo : "); string nombre = Console.ReadLine();
            Console.Write("  Cédula          : "); string cedula = Console.ReadLine();
            Console.Write("  Cargo           : "); string cargo  = Console.ReadLine();
            asociacion.RegistrarEmpleado(nombre, cedula, cargo);
        }

        static void OpRegistrarAporte()
        {
            Console.WriteLine("\n  -- Registrar aporte --");
            asociacion.ListarEmpleados();
            Console.Write("\n  ID del empleado : "); 
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("  ⚠ ID inválido."); return; }
            Console.Write("  Concepto        : "); string concepto = Console.ReadLine();
            Console.Write("  Monto ($)       : "); 
            if (!double.TryParse(Console.ReadLine(), out double monto)) { Console.WriteLine("  ⚠ Monto inválido."); return; }
            Console.Write("  Fecha (DD/MM/YYYY): "); string fecha = Console.ReadLine();
            asociacion.RegistrarAporte(id, concepto, monto, fecha);
        }

        static void OpVerAportes()
        {
            Console.Write("\n  ID del empleado : ");
            if (int.TryParse(Console.ReadLine(), out int id))
                asociacion.MostrarAportesEmpleado(id);
            else
                Console.WriteLine("  ⚠ ID inválido.");
        }

        static void OpBuscarAporte()
        {
            Console.Write("\n  ID del empleado : ");
            if (!int.TryParse(Console.ReadLine(), out int id)) { Console.WriteLine("  ⚠ ID inválido."); return; }
            Console.Write("  Concepto a buscar: "); string concepto = Console.ReadLine();
            asociacion.BuscarAporteEmpleado(id, concepto);
        }

        // ---- Datos de demostración ----
        static void CargarDatosDemostracion()
        {
            asociacion.RegistrarEmpleado("Ana Lucía Toapanta",  "1712345678", "Docente");
            asociacion.RegistrarEmpleado("Carlos René Shiguango","2201234567", "Administrativo");
            asociacion.RegistrarEmpleado("María José Grefa",    "1523456789", "Técnico de TI");

            asociacion.RegistrarAporte(1, "Cuota mensual enero",   25.00, "05/01/2026");
            asociacion.RegistrarAporte(1, "Cuota mensual febrero", 25.00, "04/02/2026");
            asociacion.RegistrarAporte(1, "Aporte fondo navidad",  50.00, "10/02/2026");

            asociacion.RegistrarAporte(2, "Cuota mensual enero",   25.00, "06/01/2026");
            asociacion.RegistrarAporte(2, "Aporte evento deportivo",15.00,"15/01/2026");

            asociacion.RegistrarAporte(3, "Cuota mensual enero",   25.00, "07/01/2026");
            asociacion.RegistrarAporte(3, "Cuota mensual febrero", 25.00, "05/02/2026");

            Console.Clear();
        }
    }
}
