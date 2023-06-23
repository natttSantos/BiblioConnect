using BiblioConnect.Modelo;
using BiblioConnect.Data;
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
        public ActionResult DetallesLibro(int id)
        {
            return View(id);
        }


        [HttpGet]
        public JsonResult ValidarTipoTransaccion(int id)
        {
            bool disponible;
            disponible = PrestamoDAL.Instancia.Validar(id);
            return Json(new { resultado = disponible }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ValidarPrimerPrestamo(int id)
        {
            bool primerPrestamo;
            primerPrestamo = PrestamoDAL.Instancia.ValidarPrimerPrestamo(id);
            return Json(new { resultado = primerPrestamo }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ValidarUsuarioRepetido(Reserva oReserva)
        {
            bool usuarioRepetido;
            usuarioRepetido = ReservaDAL.Instancia.Validar(oReserva);
            return Json(new { resultado = usuarioRepetido }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistrarPrestamo(Prestamo objeto)
        {
            bool registrado;
            registrado = PrestamoDAL.Instancia.Registrar(objeto);
            return Json(new { resultado = registrado }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RegistrarReserva(Reserva objeto)
        {
            bool registrado;
            registrado = ReservaDAL.Instancia.Registrar(objeto);
            return Json(new { resultado = registrado }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ObtenerLibro(int id)
        {
            Libro oLibro= LibroDAL.Instancia.Obtener(id);
            return Json(new { data = oLibro }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerAutor(int id)
        {
            Autor oAutor = AutorDAL.Instancia.Obtener(id);
            return Json(new { data = oAutor }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerCategoria(int id)
        {
            Categoria oCategoria = CategoriaDAL.Instancia.Obtener(id);
            return Json(new { data = oCategoria }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerEditorial(int id)
        {
            Editorial oEditorial = EditorialDAL.Instancia.Obtener(id);
            return Json(new { data = oEditorial }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerBiblio(int id)
        {
            Biblioteca oBiblioteca = BibliotecaDAL.Instancia.ObtenerBiblio(id);
            return Json(new { data = oBiblioteca }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerPrestamo(int id)
        {
            Prestamo oPrestamo = PrestamoDAL.Instancia.Obtener(id);
            return Json(new { data = oPrestamo }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerUltimaFechaReserva(int id)
        {
            DateTime fechaReserva;
            fechaReserva = ReservaDAL.Instancia.ObtenerUltimaFecha(id);
            return Json(new { data = fechaReserva }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerUltimoPrestamo(int id)
        {
            Prestamo oPrestamo = PrestamoDAL.Instancia.ObtenerUltimo(id);
            return Json(new { data = oPrestamo }, JsonRequestBehavior.AllowGet);
        }
    }
}