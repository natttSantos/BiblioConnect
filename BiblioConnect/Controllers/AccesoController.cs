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
        public ActionResult Login(Biblioteca oBiblioteca)
        {
            //oUsuario.Contraseña = ConvertirSha256(oUsuario.Contraseña);

            string consultaSql = "SELECT Nombre, Calle, Ciudad, CodPostal, Id FROM Biblioteca WHERE Email = @Email AND Contraseña = @Contraseña";

            using (SqlConnection connection = new SqlConnection(cadena))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@Email", oBiblioteca.Email);
                command.Parameters.AddWithValue("@Contraseña", oBiblioteca.Contraseña);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    if (reader.GetValue(0) != DBNull.Value)
                    {
                        oBiblioteca.Nombre = reader["Nombre"].ToString();
                        oBiblioteca.Id = int.Parse(reader["Id"].ToString());
                    }
                }
                connection.Close();
                reader.Close();
                if (oBiblioteca.Id != 0)
                {
                    Session["Usuario"] = oBiblioteca;
                    //TempData["nombreUsuario"] = oUsuario.nombreUsuario;
                    return RedirectToAction("Dashboard", "Biblioteca");
                }
                else
                {
                    ViewData["Mensaje"] = "Usuario no encontrado";
                    return View();
                }
            }
        }

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