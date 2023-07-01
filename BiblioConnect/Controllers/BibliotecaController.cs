using Newtonsoft.Json;
using BiblioConnect.Modelo;
using BiblioConnect.Data; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using System.Data.SqlClient;
using System.Data;
using System.Threading;
using BiblioConnect.Permisos;

namespace BiblioConnect.Controllers
{
    public class BibliotecaController : Controller
    {
        [ValidarSesion] //Antes de que se ejecute cualquiera de las vistas anteriores se ejecuta la clase validarsesion
        public ActionResult Dashboard() 
        {
            return View();
        }
        public ActionResult Perfil(int id)
        {
            ViewBag.IdBiblioteca = id;

            return View();
        }
        public ActionResult CerrarSesion()
        {
            Session["usuario"] = null;
            return RedirectToAction("Login", "Acceso");
        }

        public ActionResult Libros()
        {
            return View();
        }

        public ActionResult Autores()
        {
            return View();
        }

        public ActionResult Editorial()
        {
            return View();
        }
        public ActionResult Prestamo() {
            return View();
        }
        public ActionResult Evento() {
            return View(); 
        }
        public ActionResult Calendario()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerBiblio(int id)
        {
            Biblioteca oBiblioteca = BibliotecaDAL.Instancia.ObtenerBiblio(id);
            return Json(new { data = oBiblioteca }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarPrestamosBiblio(int id, string estado)
        {
            List<Object> oLista = new List<Object>();
            if (estado.Equals("En espera"))
            {
                oLista = ReservaDAL.Instancia.Listar(id, estado);
            }
            else { oLista = PrestamoDAL.Instancia.Listar(id, estado); }
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarEditorial(int id)
        {
            List<Editorial> oLista = new List<Editorial>();
            oLista = EditorialDAL.Instancia.Listar(id);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarAutor(int id)
        {
            List<Autor> oLista = new List<Autor>();
            oLista = AutorDAL.Instancia.Listar(id);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarLibroPorBiblio(int id)
        {
            List<Libro> oLista = new List<Libro>();
            oLista = LibroDAL.Instancia.ListarPorBiblioteca(id);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarLibro()
        {
            List<Libro> oLista = new List<Libro>();
            oLista = LibroDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult ListarEventoPorBiblio(int id)
        {
            List<Evento> oLista = new List<Evento>();
            oLista = EventoDAL.Instancia.ListarPorBiblioteca(id);
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GuardarEvento(string objeto, HttpPostedFileBase imagenArchivo)
        {
            Stream image = imagenArchivo.InputStream;
            string fileName = Path.GetFileName(imagenArchivo.FileName);
            string urlImagen = await new Helpers().SetImageToFirebase(image, fileName, "Fotos_Eventos");

            Evento oEvento = new Evento();
            oEvento = JsonConvert.DeserializeObject<Evento>(objeto);
            oEvento.Foto = urlImagen;

            bool respuesta = false;
            respuesta = EventoDAL.Instancia.Registrar(oEvento);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarEditorial(Editorial objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? EditorialDAL.Instancia.Registrar(objeto) : EditorialDAL.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GuardarAutor(Autor objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? AutorDAL.Instancia.Registrar(objeto) : AutorDAL.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> GuardarLibro(string objeto, HttpPostedFileBase imagenArchivo)
        {
            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Stream image = imagenArchivo.InputStream;
                string fileName = Path.GetFileName(imagenArchivo.FileName);
                string urlimagen = await new Helpers().SetImageToFirebase(image, fileName, "Fotos_Libros");

                Libro oLibro = new Libro();
                oLibro = JsonConvert.DeserializeObject<Libro>(objeto);
                oLibro.Foto = urlimagen;

                Usuario oUsuario = Session["Usuario"] as Usuario;
                oLibro.idBiblioteca = oUsuario.Id;

                //Registro Libro
                if (oLibro.Id == 0)
                {
                    int id = LibroDAL.Instancia.Registrar(oLibro);
                    oLibro.Id = id;
                    oresponse.resultado = oLibro.Id == 0 ? false : true;

                }
                //Modificacion Libro
                else
                {
                    oresponse.resultado = LibroDAL.Instancia.Modificar(oLibro);
                }
            }
            catch (Exception e)
            {
                oresponse.resultado = false;
                oresponse.mensaje = e.Message;
            }

            return Json(oresponse, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DevolverPrestamo(Prestamo objeto)
        {
            bool respuesta = false;
            respuesta = PrestamoDAL.Instancia.Devolver(objeto); 
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarEditorial(int id)
        {
            bool respuesta = false;
            respuesta = EditorialDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAutor(int id)
        {
            bool respuesta = false;
            respuesta = AutorDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarLibro(int id)
        {
            bool respuesta = false;
            respuesta = LibroDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
    public class Response
    {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}