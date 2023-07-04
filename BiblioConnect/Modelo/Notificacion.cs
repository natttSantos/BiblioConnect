using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Notificacion
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int idBiblioteca { get; set; }
        public int idLector { get; set; }
        public int idLibro { get; set; }
        public DateTime FechaEnvio { get; set; }
    }
}