using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Evento 
    {
        public int Id { get; set; }
        public string NombreAutor { get; set; }
        public string Tipo { get; set; }
        public string Foto { get; set; }
        public string DescripcionAutor { get; set; }
        public string DescripcionEvento { get; set; }
        public int idBiblioteca { get; set; }
        public DateTime FechaRealizacion { get; set; }
        public String HoraInicio { get; set; }
        public String HoraFin { get; set; }
    }
}