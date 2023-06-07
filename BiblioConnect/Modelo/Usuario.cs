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
        public string Apellidos { get; set; }
        public string Dni { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Contraseña { get; set; }
        public DateTime FechaNacimiento { get; set; }

        public string confirmarContraseña { get; set; }
    }
}