using Newtonsoft.Json;
using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiblioConnect.Logica;
using ProyectoBiblioteca.Logica;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Storage;
using System.Data.SqlClient;
using System.Data;
using System.Threading;


namespace BiblioConnect.Controllers
{
    public class RecursosController : Controller
    {
        // GET: Biblioteca
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

        //[HttpPost]
        //public async Task<int> Registrar(Libro objeto, HttpPostedFileBase imagenArchivo)
        //{
        //    //RECIBIR LOS DATOS DEL FORMULARIO
        //    Stream image = imagenArchivo.InputStream;
        //    string fileName = Path.GetFileName(imagenArchivo.FileName);
        //    string urlimagen = await SubirStorage(image, fileName);


        //    int respuesta = 0;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
        //            cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
        //            cmd.Parameters.AddWithValue("Foto", urlimagen);
        //            cmd.Parameters.AddWithValue("idAutor", objeto.oAutor.Id);
        //            cmd.Parameters.AddWithValue("idCategoria", objeto.oCategoria.Id);
        //            cmd.Parameters.AddWithValue("idEditorial", objeto.oEditorial.Id);
        //            cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
        //            cmd.Parameters.AddWithValue("numEjemplares", objeto.numEjemplares);
        //            cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();

        //            cmd.ExecuteNonQuery();

        //            respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = 0;
        //        }
        //    }
        //    return RedirectToAction("Libros");
        //}
        //FIN NUEVO

        [HttpGet]
        public JsonResult ListarCategoria()
        {
            List<Categoria> oLista = new List<Categoria>();
            oLista = CategoriaLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarCategoria(Categoria objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? CategoriaLogica.Instancia.Registrar(objeto) : CategoriaLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarCategoria(int id)
        {
            bool respuesta = false;
            respuesta = CategoriaLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarEditorial()
        {
            List<Editorial> oLista = new List<Editorial>();
            oLista = EditorialLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarEditorial(Editorial objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? EditorialLogica.Instancia.Registrar(objeto) : EditorialLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarEditorial(int id)
        {
            bool respuesta = false;
            respuesta = EditorialLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarAutor()
        {
            List<Autor> oLista = new List<Autor>();
            oLista = AutorLogica.Instancia.Listar();
            return Json(new { data = oLista }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GuardarAutor(Autor objeto)
        {
            bool respuesta = false;
            respuesta = (objeto.Id == 0) ? AutorLogica.Instancia.Registrar(objeto) : AutorLogica.Instancia.Modificar(objeto);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EliminarAutor(int id)
        {
            bool respuesta = false;
            respuesta = AutorLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }



        [HttpGet]
        public JsonResult ListarLibro()
        {
            List<Libro> oLista = new List<Libro>();

            oLista = LibroLogica.Instancia.Listar();

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

                if (oLibro.Id == 0)
                {
                    int id = LibroLogica.Instancia.Registrar(oLibro);
                    oLibro.Id = id;
                    oresponse.resultado = oLibro.Id == 0 ? false : true;

                }
                else
                {
                    oresponse.resultado = LibroLogica.Instancia.Modificar(oLibro);
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
            respuesta = LibroLogica.Instancia.Eliminar(id);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
    }
    public class Response
    {

        public bool resultado { get; set; }
        public string mensaje { get; set; }
    }
}