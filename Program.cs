using System;
using System.Threading;
using System.IO;

namespace Agenda_Telefonica {
    struct persona {
        public string nombre;
        public string apellido;
        public string telefono;
        public string direccion;
        public string correo;
    }
    class Program {
        static void Main(string[] args) {
            encavezado();
            int op;

            do {
                printLayoutInCoord(ConsoleColor.White, 0, 6, 80, 19);

                op = mainMenu();

                string f = "contactos.db";
                switch (op) {
                    case 0:
                        leerContactos(f);
                        break;
                    case 1:
                        agregarContatcto(f);
                        break;
                    case 2:
                        editarContatcto(f);
                        break;
                    case 3:
                        borrarContatcto(f);
                        break;
                }
            } while (op != -1);



            gotoxy(0, 21);
        }

        static void gotoxy(int x, int y) { Console.SetCursorPosition(x, y); }

        static void printLayout(ConsoleColor color, int w = 5, int h = 5) {
            Console.BackgroundColor = color;
            for (int i = 0; i < h; i++) {
                gotoxy(0, i);
                Console.Write(new string(' ', w));
            }
        }

        static void printLayoutInCoord(ConsoleColor color, int x = 0, int y = 0, int w = 5, int h = 5) {
            Console.BackgroundColor = color;
            for (int i = 0; i < h; i++) {
                gotoxy(x, y + i);
                Console.Write(new string(' ', w));
            }
            gotoxy(0, 0);
        }

        static void encavezado() {
            printLayout(ConsoleColor.Blue, 80, 6);
            gotoxy(0, 1);
            Console.Write(centeredString("Cristobal Rodas"));
            Console.Write(centeredString("Agenda Telefonica"));
            Console.Write(centeredString(".: Todos los derechos son de libre uso :."));
        }

        static void editYborrar(string f, bool borrar = false) {
            string line = "";
            string select;
            string listId = "";
            int id = 0;
            string ft = "temp.tmp";

            gotoxy(0, 7);

            if(!borrar) Console.Write(centeredString("Dijete los datos del contacto que desea editar"));
            else Console.Write(centeredString("Dijete los datos del contacto que desea borrar"));

            Console.Write(centeredString("si desconose un dato omitalo"));

            persona pb = crearPersona();
            persona p;


            gotoxy(0, 14);
            Console.WriteLine(centeredString("Buscado contactos..."));
            cargando(15, 15, 50, 40);


            printLayoutInCoord(ConsoleColor.DarkGray, 0, 6, 80, 19);


            try {
                StreamReader sr = new StreamReader(f);
                line = sr.ReadLine();

                gotoxy(0, 7);
                Console.Write(centeredString($"┌{RepeatChar('─', 3)}┬{RepeatChar('─', 25)}┬{RepeatChar('─', 10)}┬{RepeatChar('─', 15)}┬{RepeatChar('─', 20)}┐"));
                Console.Write(centeredString($"│{leftString("ID", 3)}│{centeredString("Nombre", 25)}│{centeredString("Telefono", 10)}│{centeredString("Direccion", 15)}│{centeredString("Correo", 20)}│"));

                while (line != null) {

                    p = stringToperson(line);

                    if (p.nombre == pb.nombre || p.apellido == pb.apellido || p.telefono == pb.telefono || p.direccion == pb.direccion || p.correo == pb.direccion) {
                        Console.Write(centeredString($"├{RepeatChar('─', 3)}│{RepeatChar('─', 25)}┼{RepeatChar('─', 10)}┼{RepeatChar('─', 15)}┼{RepeatChar('─', 20)}┤"));
                        Console.Write(centeredString($"│{leftString(id.ToString(), 3)}│{centeredString($"{p.nombre} {p.apellido}", 25)}│{centeredString(p.telefono, 10)}│{centeredString(p.direccion, 15)}│{centeredString(p.correo, 20)}│"));
                        listId = $"{listId},{id}";
                    }

                    line = sr.ReadLine();
                    id++;
                }

                Console.Write(centeredString($"└{RepeatChar('─', 3)}┴{RepeatChar('─', 25)}┴{RepeatChar('─', 10)}┴{RepeatChar('─', 15)}┴{RepeatChar('─', 20)}┘"));

                sr.Close();
            } catch (Exception e) {
                Console.WriteLine($"error: {e.Message}");
                Console.ReadLine();
            }

            string[] listIdList = listId.Split(',');
            bool existe = false;

            Console.WriteLine(centeredString("Seleccione el ID"));
            Console.Write("ID: ");
            select = Console.ReadLine();

            foreach (string s in listIdList) {
                if (s.Equals(select)) existe = true;
            }

            if (!existe) {
                Console.WriteLine($"el ID: {select} es incorecto");
                Console.ReadLine();
                return;
            }

            int idAux = 0;

            try {
                StreamWriter sw = new StreamWriter(ft);
                StreamReader sr = new StreamReader(f);
                do {
                    line = sr.ReadLine();
                    p = stringToperson(line);
                    if (select == idAux.ToString()) {
                        if (!borrar) {
                            printLayoutInCoord(ConsoleColor.DarkGray, 0, 6, 80, 19);
                            gotoxy(0, 7);
                            Console.Write(centeredString("Introdusca los nuevos datos a actualizar"));
                            pb = crearPersona();

                            if (pb.nombre != "'") p.nombre = pb.nombre;
                            if (pb.apellido != "'") p.apellido = pb.apellido;
                            if (pb.telefono != "'") p.telefono = pb.telefono;
                            if (pb.direccion != "'") p.direccion = pb.direccion;
                            if (pb.correo != "'") p.correo = pb.correo;
                        } else {
                            idAux++;
                            continue;
                        }
                        
                    }

                    if (line != null) sw.WriteLine($"{p.nombre},{p.apellido},{p.telefono},{p.direccion},{p.correo}");
                    idAux++;
                } while (line != null);

                sw.Close();
                sr.Close();

            } catch (Exception e) {
                Console.WriteLine($"error: {e.Message}");
                Console.ReadLine();
            }

            gotoxy(0, 14);
            if (!borrar) Console.WriteLine(centeredString("Guardando contacto..."));
            else Console.WriteLine(centeredString("Borrando contacto..."));

            cargando(15, 15, 40, 40);

            File.Delete(f);
            File.Move(ft, f);
        }

        static void editarContatcto(string f) {
            printLayoutInCoord(ConsoleColor.DarkGray, 0, 6, 80, 19);
            editYborrar(f);            
        }

        static void borrarContatcto(string f) {
            printLayoutInCoord(ConsoleColor.DarkGray, 0, 6, 80, 19);
            editYborrar(f, true);
        }

        static persona stringToperson(string line) {

            persona p = new persona();

            if (line == null) return p;

             string[] datos = line.Split(',');

            p.nombre = datos[0] == "'" ? "" : datos[0];
            p.apellido = datos[1] == "'" ? "" : datos[1];
            p.telefono = datos[2] == "'" ? "" : datos[2];
            p.direccion = datos[3] == "'" ? "" : datos[3];
            p.correo = datos[4] == "'" ? "" : datos[4];

            return p;
        }

        static void leerContactos(string f) {

            printLayoutInCoord(ConsoleColor.DarkGreen, 0, 6, 80, 19);
            

            try {
                // ─ ┌ ┐ ┬ ├ ┤ │ ┼ ┴ └ ┘
                StreamReader sr = new StreamReader(f);
                string line;

                line = sr.ReadLine();

                gotoxy(0, 7);
                Console.WriteLine(centeredString("Cargando contactos..."));
                cargando(15, 8, 40, 40);

                gotoxy(0, 7);
                Console.Write(centeredString($"┌{RepeatChar('─', 25)}┬{RepeatChar('─', 10)}┬{RepeatChar('─', 15)}┬{RepeatChar('─', 20)}┐"));
                Console.Write(centeredString($"│{centeredString("Nombre",25)}│{centeredString("Telefono", 10)}│{centeredString("Direccion", 15)}│{centeredString("Correo", 20)}│"));

                while (line != null) {

                    persona p = stringToperson(line);

                    Console.Write(centeredString($"├{RepeatChar('─', 25)}┼{RepeatChar('─', 10)}┼{RepeatChar('─', 15)}┼{RepeatChar('─', 20)}┤"));
                    Console.Write(centeredString($"│{centeredString($"{p.nombre} {p.apellido}", 25)}│{centeredString(p.telefono, 10)}│{centeredString(p.direccion, 15)}│{centeredString(p.correo, 20)}│"));

                    line = sr.ReadLine();
                }

                sr.Close();
                Console.Write(centeredString($"└{RepeatChar('─', 25)}┴{RepeatChar('─', 10)}┴{RepeatChar('─', 15)}┴{RepeatChar('─', 20)}┘"));
                Console.ReadKey();

            } catch (Exception e) {
                StreamWriter sw = new StreamWriter(f, true);
                sw.Close();
                leerContactos(f);
            }
        }

        static persona crearPersona() {
            persona p = new persona();

            Console.Write("Nombre: ");
            p.nombre = Console.ReadLine();
            if (p.nombre == string.Empty) p.nombre = "'";

            Console.Write("Apellido: ");
            p.apellido = Console.ReadLine();
            if (p.apellido == string.Empty) p.apellido = "'";

            Console.Write("Telefono: ");
            p.telefono = Console.ReadLine();
            if (p.telefono == string.Empty) p.telefono = "'";

            Console.Write("Dirección: ");
            p.direccion = Console.ReadLine();
            if (p.direccion == string.Empty) p.direccion = "'";

            Console.Write("correo: ");
            p.correo = Console.ReadLine();
            if (p.correo == string.Empty) p.correo = "'";

            return p;
        }

        static void agregarContatcto(string f) {

            printLayoutInCoord(ConsoleColor.DarkRed, 0, 6, 80, 19);

            gotoxy(0, 7);
            persona p = new persona();

            p = crearPersona();

            gotoxy(0, 13);
            Console.Write(centeredString("Guardadndo..."));
            cargando(15, 14, 40, 45);

            gotoxy(10, 16);
            Console.Write("Contato Guardado...");

            try {
                StreamWriter sw = new StreamWriter(f, true);
                sw.WriteLine($"{p.nombre},{p.apellido},{p.telefono},{p.direccion},{p.correo}");
                sw.Close();
            } catch (Exception e) {
                Console.WriteLine($"Exception: {e.Message}");
                Console.ReadLine();
            }
        }

        static void cargando(int x, int y, int tam = 73, int espera = 110) {
            int estado = 0;
            string carga = "|/-\\";
            for (int i = 0; i <= 100; i++) {
                gotoxy(x, y);
                string barra = $"{new string('*', (i * tam) / 100)}";
                Console.Write($"{leftString(i.ToString(), 3)}%{carga[estado]}[{barra}{new string(' ', tam - barra.Length)}]");
                estado++;
                if (estado == carga.Length) estado = 0;
                Thread.Sleep(espera);
            }
        }

        static int mainMenu() {
            printLayoutInCoord(ConsoleColor.DarkBlue, 20, 9, 40, 10);

            ConsoleKeyInfo input;
            int pos = 0;
            do {
                string[] menu = { "Ver Contactos", "Agregar Contatcto", "Editar Contacto", "Eliminar Contacto" };
                Console.ForegroundColor = ConsoleColor.White;

                for (int i = 0; i < menu.Length; i++) {
                    gotoxy(22, i + 10);
                    if (pos == i) Console.Write($"{(char)26} ");
                    Console.Write(menu[i]);
                }
                gotoxy(22, 18);
                Console.Write($"Salir {(char)16} q");


                input = Console.ReadKey(true);

                if (input.Key == ConsoleKey.DownArrow) pos++;
                else if (input.Key == ConsoleKey.UpArrow) pos--;

                if (pos >= menu.Length) pos = 0;
                else if (pos < 0) pos = menu.Length - 1;

            } while (!(input.Key == ConsoleKey.Q || input.Key == ConsoleKey.Enter));

            if (input.Key == ConsoleKey.Q) return -1; 

            return pos;
        }

        /// Centra una cadena de texto en un ancho específico.
        static string centeredString(string s, int width = 80) {

            if (s.Length >= width)  return s;

            int leftPadding = (width - s.Length) / 2;
            int rightPadding = width - s.Length - leftPadding;

            return new string(' ', leftPadding) + s + new string(' ', rightPadding);
        }

        /// alinea a la izquierda una cadena de texto en un ancho específico.
        static string leftString(string s, int width = 80) {
            int rightPadding = width - s.Length;
            return s + new string(' ', rightPadding);
        }

        /// alinea a la derecha una cadena de texto en un ancho específico.
        static string rightString(string s, int width = 80) {
            int leftPadding = width - s.Length;
            return new string(' ', leftPadding) + s;
        }

        /// Repite un carácter un número especificado de veces.
        static string RepeatChar(char c, int val = 80) {
            return new string(c, val);
        }
    }
}
