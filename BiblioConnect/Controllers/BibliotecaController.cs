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
        public ActionResult Perfil() 
        {
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

        public ActionResult Categoria()
        {
            return View();
        }


        [HttpGet]
        public JsonResult ObtenerBiblio()
        {
            Usuario userSession = Session["Usuario"] as Usuario;
            int Id = userSession.Id;
            Biblioteca oBiblioteca = BibliotecaDAL.Instancia.ObtenerBiblio(Id);
            return Json(new { data = oBiblioteca }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? CategoriaDAL.Instancia.Registrar(objeto) : CategoriaDAL.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = CategoriaDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarEditorial()
        {
            List<Editorial> oLista = new List<Editorial>();
            oLista = EditorialDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarEditorial(Editorial objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? EditorialDAL.Instancia.Registrar(objeto) : EditorialDAL.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarEditorial(int id)
        {
            bool respuesta = false;
            respuesta = EditorialDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarAutor()
        {
            List<Autor> oLista = new List<Autor>();
            oLista = AutorDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarAutor(Autor objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? AutorDAL.Instancia.Registrar(objeto) : AutorDAL.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAutor(int id)
        {
            bool respuesta = false;
            respuesta = AutorDAL.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarLibro()
        {
            List<Libro> oLista = new List<Libro>();
            oLista = LibroDAL.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }

        //Subir los archivos a Firebase 
        public async Task<string> SubirStorage(Stream archivo, string nombre)
        {
            //INGRESA AQUÍ TUS PROPIAS CREDENCIALES
            string email = "codigo@gmail.com";
            string clave = "codigo111";
            string ruta = "tfgportalreservas.appspot.com";
            string api_key = "AIzaSyA_4kSyoW9gwGiCX3xCXnTFCmtlIkwItoA";

            var auth = new FirebaseAuthProvider(new FirebaseConfig(api_key));
            var a = await auth.SignInWithEmailAndPasswordAsync(email, clave);

            var cancellation = new CancellationTokenSource();

            var task = new FirebaseStorage(
                ruta,
                new FirebaseStorageOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(a.FirebaseToken),
                    ThrowOnCancel = true
                })
                .Child("Fotos_Perfil")
                .Child(nombre)
                .PutAsync(archivo, cancellation.Token);


            var downloadURL = await task;
            return downloadURL;
        }
        [HttpPost]
        public async Task <JsonResult> GuardarLibro(string objeto, HttpPostedFileBase imagenArchivo)
        {
            Response oresponse = new Response() { resultado = true, mensaje = "" };

            try
            {
                Stream image = imagenArchivo.InputStream;
                string fileName = Path.GetFileName(imagenArchivo.FileName);
                string urlimagen = await SubirStorage(image, fileName);

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