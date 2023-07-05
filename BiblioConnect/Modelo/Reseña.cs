using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Reseña 
    {
        public int Id { get; set; }
        public int Puntuacion { get; set; }
        public string Descripcion { get; set; }
        public int idLibro { get; set; }
        public int idLector { get; set; }
        public DateTime Fecha { get; set; }
    }
}