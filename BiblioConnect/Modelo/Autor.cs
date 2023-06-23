using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Autor 
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int idBiblioteca { get; set; }
        public bool Estado { get; set; }
    }
}