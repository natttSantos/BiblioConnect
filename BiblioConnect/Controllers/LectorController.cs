using BiblioConnect.Modelo;
using ProyectoBiblioteca.Logica;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiblioConnect.Controllers
{
    public class LectorController : Controller
    {
        // NAVBAR
        public ActionResult Catalogo()
        {
            return View();
        }
        public ActionResult Inicio()
        {
            return View();
        }
        public ActionResult Eventos()
        {
            return View();
        }
        public ActionResult Reservas()
        {
            return View();
        }
        // FIN NAVBAR

        public ActionResult DetallesLibro(int id)
        {
            return View(id);
        }
        [HttpGet]
        public JsonResult ObtenerLibro(int id)
        {
            Libro oLibro= LibroDAL.Instancia.Obtener(id);
            return Json(new { data = oLibro }, JsonRequestBehavior.AllowGet);
        }
    }
}