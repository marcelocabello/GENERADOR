using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

/*
    Requerimiento 1: Todos los métodos son privados excepto el primero: CUMPLIDO
    Requerimiento 2: Es necesario generar un Program.cs para invocar el 
    primer metdo: CUMPLIDO
    Requerimiento 3: Incluir la cerradura epsilon
    Requerimiento 4: Incluir el operador = AYVA
*/
namespace GENERADOR
{
    public class Lenguaje : Sintaxis
    {
        bool primermetodo = true;
        public Lenguaje()
        {
        }
        public Lenguaje(string nombre) : base(nombre)
        {
        }
        public void Gramatica()
        {
            match("Gramatica");
            match(":");
            match(Tipos.SNT);
            match(Tipos.EOL);
            match("{");
            match(Tipos.EOL);

            lenguaje.WriteLine("using System;");
            lenguaje.WriteLine("using System.Collections;");
            lenguaje.WriteLine("using System.Collections.Generic;");
            lenguaje.WriteLine("using System.Linq;");
            lenguaje.WriteLine("using System.Runtime.CompilerServices;");
            lenguaje.WriteLine("using System.Runtime.Intrinsics.X86;");
            lenguaje.WriteLine("using System.Threading.Tasks;");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("namespace Generado");
            lenguaje.WriteLine("{");
            lenguaje.WriteLine("    public class Lenguaje : Sintaxis");
            lenguaje.WriteLine("    {");
            lenguaje.WriteLine("        public Lenguaje()");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("        }");
            lenguaje.WriteLine("        public Lenguaje(string nombre) : base(nombre)");
            lenguaje.WriteLine("        {");
            lenguaje.WriteLine("        }");
            ListaProducciones();
            lenguaje.WriteLine("    }");
            lenguaje.WriteLine("}");
            lenguaje.WriteLine("");
            lenguaje.WriteLine("");
            match("}");

            program.WriteLine("using System;");
            program.WriteLine("using System.Collections.Generic;");
            program.WriteLine("using System.Linq;");
            program.WriteLine("using System.Threading.Tasks;");
            program.WriteLine("\n");
            program.WriteLine("namespace GENERADO");
            program.WriteLine("{");
            program.WriteLine("    class Program");
            program.WriteLine("    {");
            program.WriteLine("        static void Main(string[] args) ");
            program.WriteLine("       {");
            program.WriteLine("            try");
            program.WriteLine("            {");
            program.WriteLine("                using (Lenguaje L = new Lenguaje())");
            program.WriteLine("                {");
            program.WriteLine("                    L.Gramatica();");
            program.WriteLine("                    /*");
            program.WriteLine("                    while (!L.FinArchivo())");
            program.WriteLine("                    {");
            program.WriteLine("                        L.nextToken();");
            program.WriteLine("                    }");
            program.WriteLine("                    */");
            program.WriteLine("                }");
            program.WriteLine("            } ");
            program.WriteLine("            catch (Exception e)");
            program.WriteLine("            {");
            program.WriteLine("                Console.WriteLine(\"Error\" +e.Message);");
            program.WriteLine("            }");
            program.WriteLine("        }");
            program.WriteLine("    }");
            program.WriteLine("}");

        }

        private void ListaProducciones()
        {
            Produccion();
            if (getClasificacion() == Tipos.SNT)
            {
                ListaProducciones();
            }
        }
        private void Produccion()
        {
            if (primermetodo)
            {
                lenguaje.WriteLine("        public void " + getContenido() + "()");
                lenguaje.WriteLine("        {");
                primermetodo = false;
            }
            else
            {
                lenguaje.WriteLine("        private void " + getContenido() + "()");
                lenguaje.WriteLine("        {");
            }
            match(Tipos.SNT);
            match(Tipos.Flecha);
            ListaSimbolos();
            match(Tipos.EOL);
            lenguaje.WriteLine("        }");
        }
        private bool esTipo(string tipo)
        {
            switch (tipo)
            {
                case "Identificador":
                case "Numero":
                case "Caracter":
                case "Asignacion":
                case "FinSentencia":
                case "FinArchivo":
                case "OpLogico":
                case "OperadorRelacional":
                case "OperadorTermino":
                case "IncrementoTermino":
                case "OperadorFactor":
                case "IncrementoFactor":
                case "OpTernario":
                case "Cadena":
                case "Inicio":
                case "Fin":
                case "TipoDatos":
                case "Reservada": return true;
            }
            return false;
        }
        private void ListaSimbolos()
        {
            if (getClasificacion() == Tipos.Epsilon)
            {
                lenguaje.WriteLine("            // epsilon");
            }
            else if (getClasificacion() == Tipos.ST)
            {
                lenguaje.WriteLine("            match(\"" + getContenido() + "\");");
                match(Tipos.ST);
            }

            else
            {
                if (esTipo(getContenido()))
                {
                    lenguaje.WriteLine("            match(Tipos." + getContenido() + ");");
                }
                else
                {
                    lenguaje.WriteLine("            " + getContenido() + "();");
                }
                match(Tipos.SNT);
            }

            if (getClasificacion() == Tipos.SNT || getClasificacion() == Tipos.ST || getClasificacion() == Tipos.Pipe)
            {
                ListaSimbolos();
            }
            
        }

    }
}
