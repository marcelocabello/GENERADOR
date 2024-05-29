using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace GENERADOR
{
    public class Lexico : Token, IDisposable
    {
        const int F = -1;
        const int E = -2;
        protected int linea;
        public StreamReader archivo;
        protected StreamWriter log;
        protected StreamWriter lenguaje;
        protected StreamWriter program;
        protected int ccount;

        int[,] TRAND =
        {
          // WS  \   ?   (   )   |  EOL  L   -   >  Lmd
            {0,  1,  8,  8,  8,  8,  6,  7,  8, 10, 10}, //0
            {F,  F,  2,  3,  4,  5,  F,  F,  F,  F,  F}, //1
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //2
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //3
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //4
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //5
            {F,  F,  F,  F,  E,  F,  F,  F,  F,  F,  F}, //6
            {F,  F,  F,  F,  F,  F,  F,  7,  F,  F,  F}, //7
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  9,  F}, //8
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //9
            {F,  F,  F,  F,  F,  F,  F,  F,  F,  F,  F}, //10
        };

        public Lexico() // Constructor
        {
            log = new StreamWriter("prueba.log");
            log.AutoFlush = true;
            lenguaje = new StreamWriter("/home/olecram/GENERADO/Lenguaje.cs");
            lenguaje.AutoFlush = true;
            program = new StreamWriter("/home/olecram/GENERADO/Program.cs");
            program.AutoFlush = true;
            

            log.WriteLine("Analizador Lexico");
            log.WriteLine("Autor: Guillermo Fernandez");
            if (!File.Exists("prueba.grm"))
            {
                throw new Error("El archivo prueba.grm no existe", log);
            }
            archivo = new StreamReader("prueba.grm");
        }
        public Lexico(string nombre) // Constructor
        {
            log = new StreamWriter(Path.GetFileNameWithoutExtension(nombre) + ".log");
            log.AutoFlush = true;
            lenguaje = new StreamWriter("/home/olecram/GENERADO/Lenguaje.cs");
            lenguaje.AutoFlush = true;
            program = new StreamWriter("/home/olecram/GENERADO/Program.cs");
            program.AutoFlush = true;
            log.WriteLine("Analizador Lexico");
            log.WriteLine("Autor: Guillermo Fernandez");
            if (Path.GetExtension(nombre) != ".grm")
            {
                throw new Error("El archivo " + nombre + " no tiene extension GRM", log);
            }
            if (!File.Exists(nombre))
            {
                throw new Error("El archivo " + nombre + " no existe", log);
            }
            archivo = new StreamReader(nombre);
        }
        public void Dispose()
        {
            archivo.Close();
            log.Close();
            lenguaje.Close();
        }
        private int columna(char c)
        {
            if (c == '\n')
                return 6;
            else if (char.IsWhiteSpace(c))
                return 0;
            else if (c == '\\')
                return 1;
            else if (c == '?')
                return 2;
            else if (c == '(')
                return 3;
            else if (c == ')')
                return 4;
            else if (c == '|')
                return 5;
            else if (char.IsLetter(c))
                return 7;
            else if (c == '-')
                return 8;
            else if (c == '>')
                return 9;
            else
                return 10;
        }
        private void clasificar(int estado)
        {
            switch (estado)
            {
                case 2: setClasificacion(Tipos.Epsilon); break;
                case 3: setClasificacion(Tipos.PDerecho); break;
                case 4: setClasificacion(Tipos.PIzquierdo); break;
                case 5: setClasificacion(Tipos.Pipe); break;
                case 6: setClasificacion(Tipos.EOL); break;
                case 7: 
                case 8: setClasificacion(Tipos.ST); break;
                case 9: setClasificacion(Tipos.Flecha); break;
                case 10: setClasificacion(Tipos.ST); break;
            }
        }
        public void nextToken()
        {
            char c;
            string buffer = "";

            int estado = 0;

            while (estado >= 0)
            {
                c = (char)archivo.Peek();

                estado = TRAND[estado, columna(c)];
                clasificar(estado);

                if (estado >= 0)
                {
                    if (c == '\n')
                    {
                        linea++;
                    }
                    archivo.Read();
                    ccount++;
                    if (estado > 0)
                    {
                        buffer += c;
                    }
                    else
                    {
                        buffer = "";
                    }
                }
            }
            setContenido(buffer);
            if (getClasificacion() == Tipos.ST)
            {
                if (char.IsUpper(getContenido()[0]))
                {
                    setClasificacion(Tipos.SNT);
                    
                }
            }
            log.WriteLine(getContenido() + " = " + getClasificacion());
        }
        public bool FinArchivo()
        {
            return archivo.EndOfStream;
        }
    }
}