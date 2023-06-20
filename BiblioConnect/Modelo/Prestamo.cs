using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Prestamo : Transaccion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int Duracion { get; set; }
        public DateTime FechaEntrega { get; set; }
        public DateTime FechaDevolucion { get; set; }
        public DateTime FechaDevolConfirmada { get; set; }
        public string TextoFechaDevolConfirmada { get; set; }
        public bool Estado { get; set; }
    }
}