using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Lector : Usuario
    {
        public string Apellidos { get; set; }
        public string Dni { get; set; }
    }
}