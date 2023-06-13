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
    public class LectorDAL
    {

        private static LectorDAL instancia = null;

        public LectorDAL()
        {

        }

        public static LectorDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LectorDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Lector oLector)
        {
            bool registrado = true;
            string mensaje;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLector", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oLector.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oLector.Apellidos);
                    cmd.Parameters.AddWithValue("Dni", oLector.Dni);
                    cmd.Parameters.AddWithValue("Email", oLector.Email);
                    cmd.Parameters.AddWithValue("Telefono", oLector.Telefono);
                    cmd.Parameters.AddWithValue("Contraseña", oLector.Contraseña);
                    cmd.Parameters.AddWithValue("Calle", oLector.Calle);
                    cmd.Parameters.AddWithValue("Pais", oLector.Pais);
                    cmd.Parameters.AddWithValue("Estado", oLector.Estado);
                    cmd.Parameters.AddWithValue("CodPostal", oLector.CodPostal);
                    cmd.Parameters.AddWithValue("Foto", oLector.Foto);
                    cmd.Parameters.AddWithValue("Ciudad", oLector.Ciudad);
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
        //public Biblioteca ObtenerBiblio(int Id)
        //{
        //    Biblioteca oBiblioteca = new Biblioteca();
        //    string consultaSql = "SELECT Nombre, Calle, Ciudad, CodPostal, Descripcion, Email, Telefono, Estado, Pais, Foto FROM Biblioteca WHERE Id = @Id";

        //    using (SqlConnection connection = new SqlConnection(Conexion.CN))
        //    {
        //        SqlCommand command = new SqlCommand(consultaSql, connection);
        //        command.Parameters.AddWithValue("@Id", Id);
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {                  
        //            oBiblioteca.Calle = reader["Calle"].ToString();
        //            oBiblioteca.Ciudad = reader["Ciudad"].ToString();
        //            oBiblioteca.CodPostal = reader["CodPostal"].ToString();
        //            oBiblioteca.Descripcion = reader["Descripcion"].ToString();
        //            oBiblioteca.Nombre = reader["Nombre"].ToString();
        //            oBiblioteca.Email = reader["Email"].ToString();
        //            oBiblioteca.Telefono = reader["Telefono"].ToString();
        //            oBiblioteca.Foto = reader["Foto"].ToString();
        //            oBiblioteca.Estado = reader["Estado"].ToString();
        //            oBiblioteca.Pais = reader["Pais"].ToString();
        //        }
        //        connection.Close();
        //        reader.Close();
        //    }
        //    return oBiblioteca;
        //}


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