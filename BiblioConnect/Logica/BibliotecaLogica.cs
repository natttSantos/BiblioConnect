using BiblioConnect.Logica;
using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BiblioConnect.Logica
{
    public class BibliotecaLogica
    {

        private static BibliotecaLogica instancia = null;

        public BibliotecaLogica()
        {

        }

        public static BibliotecaLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new BibliotecaLogica();
                }

                return instancia;
            }
        }

        public bool Registrar(Biblioteca oBiblioteca)
        {
            bool registrado;
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
                    cmd.Parameters.AddWithValue("CodPostal", oBiblioteca.CodPostal);
                    cmd.Parameters.AddWithValue("Foto", oBiblioteca.Foto);
                    cmd.Parameters.AddWithValue("Ciudad", oBiblioteca.Ciudad);
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
        public Biblioteca ObtenerBiblio(int Id)
        {
            Biblioteca oBiblioteca = new Biblioteca();
            string consultaSql = "SELECT Nombre, Calle, Ciudad, CodPostal, Descripcion, Email, Telefono, Foto FROM Biblioteca WHERE Id = @Id";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@Id", Id);
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
                }
                connection.Close();
                reader.Close();
            }
            return oBiblioteca;
        }


        //public List<Autor> Listar()
        //{
        //    List<Autor> Lista = new List<Autor>();
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("select Id,Descripcion,Estado from Autor", oConexion);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Lista.Add(new Autor()
        //                    {
        //                        Id = Convert.ToInt32(dr["Id"]),
        //                        Descripcion = dr["Descripcion"].ToString(),
        //                        Estado = Convert.ToBoolean(dr["Estado"])
        //                    });
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Lista = new List<Autor>();
        //        }
        //    }
        //    return Lista;
        //}

        //public bool Modificar(Autor oAutor)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_ModificarAutor", oConexion);
        //            cmd.Parameters.AddWithValue("Id", oAutor.Id);
        //            cmd.Parameters.AddWithValue("Descripcion", oAutor.Descripcion);
        //            cmd.Parameters.AddWithValue("Estado", oAutor.Estado);
        //            cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;

        //            cmd.CommandType = CommandType.StoredProcedure;

        //            oConexion.Open();

        //            cmd.ExecuteNonQuery();

        //            respuesta = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);

        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = false;
        //        }

        //    }

        //    return respuesta;

        //}

        //public bool Eliminar(int id)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("delete from Autor where Id = @id", oConexion);
        //            cmd.Parameters.AddWithValue("@id", id);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();

        //            cmd.ExecuteNonQuery();

        //            respuesta = true;

        //        }
        //        catch (Exception ex)
        //        {
        //            respuesta = false;
        //        }

        //    }

        //    return respuesta;

        //}
    }
}