using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Transaccion
    {
        public int Id { get; set; }
        public int idBiblioteca { get; set; }
        public int idLector { get; set; }
        public int idLibro { get; set; }
        public string Tipo { get; set; }
    }
}