using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiblioConnect.Logica;
using BiblioConnect.Modelo;
using BiblioConnect.Permisos; //La ruta se especifica por puntos

namespace BiblioConnect.Controllers
{
    public class InicioController : Controller
    {
        [ValidarSesion] //Antes de que se ejecute cualquiera de las vistas anteriores se ejecuta la clase validarsesion
        public ActionResult Dashboard() //Vista
        {
            //string nombreUsuario = TempData["nombreUsuario"] as string;
            return View();
        }
        public ActionResult Perfil() //Vista
        {
            Biblioteca biblioSession = Session["Usuario"] as Biblioteca;
            int Id = biblioSession.Id;

            Biblioteca biblio = new BibliotecaLogica().ObtenerBiblio(Id);
            return View(biblio);
        }
        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }
    }
}