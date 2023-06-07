using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BiblioConnect.Controllers
{
    public class AccesoController : Controller
    {
        static string cadena = "Data Source=LAPTOP-OO0FD7IU;Initial Catalog=DB_BiblioConnect;Integrated Security=True";
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrar(Usuario oUsuario)
        {
            {
                bool registrado;
                string mensaje;

                if (oUsuario.Contraseña == oUsuario.confirmarContraseña)
                {

                    oUsuario.Contraseña = ConvertirSha256(oUsuario.Contraseña);
                }
                else
                {
                    ViewData["Mensaje"] = "Las contraseñas no coinciden";
                    return View();
                }

                //Utilizar el procedimiento almacenado de registrar usuario
                using (SqlConnection cn = new SqlConnection(cadena))
                {

                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                    cmd.Parameters.AddWithValue("Nombre", oUsuario.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oUsuario.Apellidos);
                    cmd.Parameters.AddWithValue("Dni", oUsuario.Dni);
                    cmd.Parameters.AddWithValue("Email", oUsuario.Email);
                    cmd.Parameters.AddWithValue("Telefono", oUsuario.Telefono);
                    cmd.Parameters.AddWithValue("Contraseña", oUsuario.Contraseña);
                    cmd.Parameters.AddWithValue("FechaNacimiento", oUsuario.FechaNacimiento);
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    cn.Open();

                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();


                }

                ViewData["Mensaje"] = mensaje;

                if (registrado)
                {
                    return RedirectToAction("Login", "Acceso");
                }
                else
                {
                    return View();
                }

            }
        }

        [HttpPost]
        public ActionResult Login(Usuario oUsuario)
        {
            oUsuario.Contraseña = ConvertirSha256(oUsuario.Contraseña);

            using (SqlConnection cn = new SqlConnection(cadena))
            {
                SqlCommand cmd = new SqlCommand("sp_ValidarUsuario", cn);
                cmd.Parameters.AddWithValue("Email", oUsuario.Email);
                cmd.Parameters.AddWithValue("Contraseña", oUsuario.Contraseña);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                oUsuario.Id = Convert.ToInt32(cmd.ExecuteScalar().ToString());

            }
            if (oUsuario.Id != 0)
            {

                Session["usuario"] = oUsuario;
                //TempData["nombreUsuario"] = oUsuario.nombreUsuario;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewData["Mensaje"] = "Usuario no encontrado";
                return View();
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