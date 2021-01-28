using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Practica4
{
    public class Asiento
    {
        public int fila { get; set; }
        public int columna { get; set; }
        public string estado { get; set; }

        public Asiento()
        {

        }
        public Asiento(int fila, int columna, string estado)
        {
            this.fila = fila;
            this.columna = columna;
            this.estado = estado;
           
        }

        public void toString()
        {

            MessageBox.Show("El asiento " + fila + ":" + columna + " esta " + estado);

        }
    }
}
