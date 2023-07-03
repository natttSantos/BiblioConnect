using BiblioConnect.Modelo;
using BiblioConnect.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.SignalR;
using BiblioConnect.HubSiganlR;

namespace BiblioConnect.Controllers
{
    public class LectorController : Controller
    {
        // NAVBAR
        public ActionResult CatalogoLibros()
        {
            return View();
        }
        public ActionResult Inicio()
        {
            return View();
        }
        public ActionResult CatalogoEventos()
        {
            return View();
        }
        public ActionResult Reservas()
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<NotificacionesHub>();
            hubContext.Clients.All.mostrarNotificacion("¡Hola desde el servidor!");

            return View();
        }
        public ActionResult DetallesLibro(int id)
        {
            return View(id);
        }
        public ActionResult DetallesEvento(int id)
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
        public JsonResult RegistrarPendienteRecogida(Prestamo objeto)
        {
            bool registrado;
            registrado = PrestamoDAL.Instancia.RegistrarPendiente(objeto);
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
        public JsonResult ObtenerEvento(int id)
        {
            Evento oEvento = EventoDAL.Instancia.Obtener(id);
            return Json(new { data = oEvento }, JsonRequestBehavior.AllowGet);
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
        public JsonResult ObtenerUltimaReserva(int id)
        {
            Reserva oReserva = ReservaDAL.Instancia.ObtenerUltima(id);
            return Json(new { data = oReserva }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ObtenerUltimoPrestamo(int id)
        {
            Prestamo oPrestamo = PrestamoDAL.Instancia.ObtenerUltimo(id);
            return Json(new { data = oPrestamo }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarLibroPorCategoria(string nombre)
        {
            List<Libro> oLista = new List<Libro>();
            oLista = LibroDAL.Instancia.ListarPorCategoria(nombre);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarLibroPorIdioma(string idioma)
        {
            List<Libro> oLista = new List<Libro>();
            oLista = LibroDAL.Instancia.ListarPorIdioma(idioma);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarLibroPorFiltrado(string nombre)
        {
            List<int> oLista = new List<int>();
            oLista = LibroDAL.Instancia.ListarId(nombre);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarEvento()
        {
            List<Evento> oLista = new List<Evento>();
            oLista = EventoDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarEventoPorTipo(string tipo)
        {
            List<Evento> oLista = new List<Evento>();
            oLista = EventoDAL.Instancia.ListarPorTipo(tipo);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
    }
}