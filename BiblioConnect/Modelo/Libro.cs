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
        public int idAutor { get; set; }
        public int idCategoria { get; set; }
        public int idEditorial { get; set; }
        public int idBiblioteca { get; set; }
        public int numEjemplares { get; set; }
        public bool Estado { get; set; }
    }
}