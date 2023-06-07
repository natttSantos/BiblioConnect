using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public bool Estado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}