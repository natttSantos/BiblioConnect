using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Libro
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Foto { get; set; }
        public string Ubicacion { get; set; }
        public Autor oAutor { get; set; }
        public Categoria oCategoria { get; set; }
        public Editorial oEditorial { get; set; }
        public int numEjemplares { get; set; }
        public bool Estado { get; set; }
    }
}