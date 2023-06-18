using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiblioConnect.Modelo
{
    public class Biblioteca : Usuario
    {
        public string Descripcion { get; set; }

        //Pasar de Usuario a Biblioteca
        //public Biblioteca(Usuario oUsuario)
        //{
        //    Id = oUsuario.Id;
        //    Nombre = oUsuario.Nombre;
        //    Email = oUsuario.Email;
            
        //}
    }
}