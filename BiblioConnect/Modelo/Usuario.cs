using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }
        public string Ciudad { get; set; }
        public string Calle { get; set; }
        public string Pais { get; set; }
        public string Estado { get; set; }
        public string CodPostal { get; set; }
        public string Foto { get; set; }
        public string confirmarContraseña { get; set; }
    }
}