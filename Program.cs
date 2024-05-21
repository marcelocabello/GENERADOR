﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GENERADOR
{
    class Program
    {
        static void Main(string[] args) 
        {
            try
            {
                using (Lenguaje L = new Lenguaje())
                {
                    L.Gramatica();
                    /*
                    while (!L.FinArchivo())
                    {
                        L.nextToken();
                    }
                    */
                }
            } 
            catch (Exception e)
            {
                Console.WriteLine("Error "+e.Message);
            }
        }
    }
}
