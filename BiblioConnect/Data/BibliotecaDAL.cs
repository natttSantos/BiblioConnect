using BiblioConnect.Data;
using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BiblioConnect.Data
{
    public class BibliotecaDAL
    {

        private static BibliotecaDAL instancia = null;

        public BibliotecaDAL()
        {

        }

        public static BibliotecaDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new BibliotecaDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Biblioteca oBiblioteca)
        {
            bool registrado = true;
            string mensaje;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarBiblioteca", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oBiblioteca.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oBiblioteca.Descripcion);
                    cmd.Parameters.AddWithValue("Email", oBiblioteca.Email);
                    cmd.Parameters.AddWithValue("Telefono", oBiblioteca.Telefono);
                    cmd.Parameters.AddWithValue("Contraseña", oBiblioteca.Contraseña);
                    cmd.Parameters.AddWithValue("Calle", oBiblioteca.Calle);
                    cmd.Parameters.AddWithValue("Pais", oBiblioteca.Pais);
                    cmd.Parameters.AddWithValue("Estado", oBiblioteca.Estado);
                    cmd.Parameters.AddWithValue("CodPostal", oBiblioteca.CodPostal);
                    cmd.Parameters.AddWithValue("Foto", oBiblioteca.Foto);
                    cmd.Parameters.AddWithValue("Ciudad", oBiblioteca.Ciudad);
                    cmd.Parameters.AddWithValue("TipoUsuario", "biblioteca");
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 100).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);
                    mensaje = cmd.Parameters["Mensaje"].Value.ToString();

                }
                catch (Exception ex)
                {
                    registrado = false;
                }
            }
            return registrado;
        }
        public bool Modificar(Biblioteca oBiblioteca)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarBiblioteca", oConexion);
                    cmd.Parameters.AddWithValue("BibliotecaId", oBiblioteca.Id);
                    cmd.Parameters.AddWithValue("Nombre", oBiblioteca.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", oBiblioteca.Descripcion);
                    cmd.Parameters.AddWithValue("Email", oBiblioteca.Email);
                    cmd.Parameters.AddWithValue("Telefono", oBiblioteca.Telefono);
                    cmd.Parameters.AddWithValue("Contraseña", oBiblioteca.Contraseña);
                    cmd.Parameters.AddWithValue("Calle", oBiblioteca.Calle);
                    cmd.Parameters.AddWithValue("Pais", oBiblioteca.Pais);
                    cmd.Parameters.AddWithValue("Estado", oBiblioteca.Estado);
                    cmd.Parameters.AddWithValue("CodPostal", oBiblioteca.CodPostal);
                    cmd.Parameters.AddWithValue("Foto", oBiblioteca.Foto);
                    cmd.Parameters.AddWithValue("Ciudad", oBiblioteca.Ciudad);
                    cmd.Parameters.AddWithValue("TipoUsuario", "biblioteca");
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }

        public Biblioteca ObtenerBiblio(int Id)
        {
            Biblioteca oBiblioteca = new Biblioteca();
            string consultaSql = "SELECT u.Nombre, u.Calle, u.Ciudad, u.CodPostal, u.Email, u.Telefono, u.Estado, u.Contraseña, u.Pais, u.Foto, b.Descripcion " +
                "FROM Usuario u JOIN Biblioteca b " +
                "ON u.Id = b.Id " +
                "WHERE u.Id = @usuarioId ";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@usuarioId", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {                  
                    oBiblioteca.Calle = reader["Calle"].ToString();
                    oBiblioteca.Ciudad = reader["Ciudad"].ToString();
                    oBiblioteca.CodPostal = reader["CodPostal"].ToString();
                    oBiblioteca.Descripcion = reader["Descripcion"].ToString();
                    oBiblioteca.Nombre = reader["Nombre"].ToString();
                    oBiblioteca.Email = reader["Email"].ToString();
                    oBiblioteca.Telefono = reader["Telefono"].ToString();
                    oBiblioteca.Foto = reader["Foto"].ToString();
                    oBiblioteca.Estado = reader["Estado"].ToString();
                    oBiblioteca.Pais = reader["Pais"].ToString();
                    oBiblioteca.Contraseña = reader["Contraseña"].ToString();
                }
                connection.Close();
                reader.Close();
            }
            return oBiblioteca;
        }
    }
}