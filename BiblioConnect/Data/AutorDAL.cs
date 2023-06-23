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
    public class AutorDAL
    {

        private static AutorDAL instancia = null;

        public AutorDAL()
        {

        }

        public static AutorDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new AutorDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Autor oAutor)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarAutor", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oAutor.Nombre);
                    cmd.Parameters.AddWithValue("idBiblioteca", oAutor.idBiblioteca);
                    cmd.Parameters.AddWithValue("Estado", oAutor.Estado);
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

        public bool Modificar(Autor oAutor)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarAutor", oConexion);
                    cmd.Parameters.AddWithValue("Id", oAutor.Id);
                    cmd.Parameters.AddWithValue("Nombre", oAutor.Nombre);
                    cmd.Parameters.AddWithValue("Estado", oAutor.Estado);
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


        public List<Autor> Listar(int id)
        {
            List<Autor> Lista = new List<Autor>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select Id,Nombre,Estado from Autor where idBiblioteca = @idBiblioteca", oConexion);
                    cmd.Parameters.AddWithValue("@idBiblioteca", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Autor()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Estado = Convert.ToBoolean(dr["Estado"])
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Autor>();
                }
            }
            return Lista;
        }

        public Autor Obtener(int Id)
        {
            Autor oAutor = new Autor();
            string consultaSql = "SELECT Nombre " +
                "FROM Autor " +
                "WHERE Id = @autorId ";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@autorId", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oAutor.Nombre = reader["Nombre"].ToString();
                }
                
                connection.Close();
                reader.Close();
            }
            return oAutor;
        }
        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Autor where Id = @id", oConexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = true;

                }
                catch (Exception ex)
                {
                    respuesta = false;
                }

            }

            return respuesta;

        }
    }
}