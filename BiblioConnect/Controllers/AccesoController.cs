using BiblioConnect.Modelo;
using BiblioConnect.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace BiblioConnect.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=LAPTOP-OO0FD7IU;Initial Catalog=DB_BiblioConnect;Integrated Security=True";
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult RegistrarBiblioteca()
        {
            return View();
        }
        public ActionResult RegistrarLector()
        {
            return View();
        }


        [HttpPost]
        public async Task<JsonResult> RegistrarBiblioteca(string objeto, HttpPostedFileBase imagenArchivo)
        {
            //Get url de la imagen subida a Firebase
            Stream image = imagenArchivo.InputStream;
            string fileName = Path.GetFileName(imagenArchivo.FileName);
            string urlImagen = await new Helpers().SetImageToFirebase(image, fileName, "Fotos_Bibliotecas");

            Biblioteca oBiblioteca = new Biblioteca();
            oBiblioteca = JsonConvert.DeserializeObject<Biblioteca>(objeto);
            oBiblioteca.Foto = urlImagen;

            bool respuesta = false;
            respuesta = BibliotecaDAL.Instancia.Registrar(oBiblioteca);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public async Task<JsonResult> ModificarBiblioteca(string objeto, HttpPostedFileBase imagenArchivo)
        {
            //Get url de la imagen subida a Firebase
            Stream image = imagenArchivo.InputStream;
            string fileName = Path.GetFileName(imagenArchivo.FileName);
            string urlImagen = await new Helpers().SetImageToFirebase(image, fileName, "Fotos_Bibliotecas");

            Biblioteca oBiblioteca = new Biblioteca();
            oBiblioteca = JsonConvert.DeserializeObject<Biblioteca>(objeto);
            oBiblioteca.Foto = urlImagen;

            bool respuesta = false;
            respuesta = BibliotecaDAL.Instancia.Registrar(oBiblioteca);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<JsonResult> RegistrarLector(string objeto, HttpPostedFileBase imagenArchivo)
        {
            //Get url de la imagen subida a Firebase
            Stream image = imagenArchivo.InputStream;
            string fileName = Path.GetFileName(imagenArchivo.FileName);
            string urlImagen = await new Helpers().SetImageToFirebase(image, fileName, "Fotos_Lectores");

            Lector oLector = new Lector();
            oLector = JsonConvert.DeserializeObject<Lector>(objeto);
            oLector.Foto = urlImagen;

            bool respuesta = false;
            respuesta = LectorDAL.Instancia.Registrar(oLector);
            return Json(new { resultado = respuesta }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            string tipoUsuario = "";

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                SqlCommand command = new SqlCommand("SELECT TipoUsuario FROM Usuario WHERE Email = @Email", connection);
                command.Parameters.AddWithValue("@Email", oUsuario.Email);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        tipoUsuario = reader["TipoUsuario"].ToString();
                    }
                }
                connection.Close();
                reader.Close();
            }
            using (SqlConnection connection = new SqlConnection(cadena))
            {
                SqlCommand command = new SqlCommand("SELECT Nombre, Id, TipoUsuario FROM Usuario WHERE Email = @Email AND Contraseña = @Contraseña", connection);
                command.Parameters.AddWithValue("@Email", oUsuario.Email);
                command.Parameters.AddWithValue("@Contraseña", oUsuario.Contraseña);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        oUsuario.Nombre = reader["Nombre"].ToString();
                        oUsuario.TipoUsuario = reader["TipoUsuario"].ToString();
                        oUsuario.Id = int.Parse(reader["Id"].ToString());
                    }
                }
                connection.Close();
                reader.Close();
                if (oUsuario.Id != 0)
                {
                    Session["Usuario"] = oUsuario;
                    if (tipoUsuario.Equals("biblioteca"))
                    {
                        return RedirectToAction("Dashboard", "Biblioteca");
                    }
                    else { return RedirectToAction("Inicio", "Lector"); }
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario no encontrado";
                    return View();
                }
            }
        }
        [HttpPost]
        public static string ConvertirSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
