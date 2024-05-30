using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GENERADOR
{
    public class Token
    {
        public enum Tipos
        {
            Epsilon, Pderecho, Pizquierdo, Pipe, EOL, ST, SNT, Flecha, PDerecho, PIzquierdo
        }
        private string contenido;
        private Tipos  clasificacion;
        public Token()
        {
            contenido = "";
            clasificacion = Tipos.ST;
        }
        public void setContenido(string contenido)
        {
            this.contenido = contenido;
        }
        public void setClasificacion(Tipos clasificacion)
        {
            this.clasificacion = clasificacion;
        }
        public string getContenido()
        {
            return this.contenido;
        }
        public Tipos getClasificacion()
        {
            return this.clasificacion;
        }
    }
}