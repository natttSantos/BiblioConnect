using BiblioConnect.Data;
using BiblioConnect.Modelo;
using BiblioConnect.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Firebase.Auth;

namespace BiblioConnect.Data
{
    public class LibroDAL
    {
        private static LibroDAL instancia = null;
        public LibroDAL(){}
        public static LibroDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LibroDAL();
                }

                return instancia;
            }
        }
        public Libro Obtener(int Id)
        {
            Libro oLibro = new Libro();
            string consultaSql = "SELECT Titulo, Foto, idBiblioteca, Autor, idCategoria, Editorial, numEjemplares, Idioma " +
                "FROM Libro " +
                "WHERE Id = @libroId ";
            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@libroId", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oLibro.Titulo = reader["Titulo"].ToString();
                    oLibro.Idioma = reader["Idioma"].ToString();
                    oLibro.Foto = reader["Foto"].ToString();
                    oLibro.numEjemplares = reader.GetInt32(reader.GetOrdinal("numEjemplares")); 
                    oLibro.idBiblioteca = reader.GetInt32(reader.GetOrdinal("idBiblioteca"));
                    oLibro.Autor = reader["Autor"].ToString();
                    oLibro.idCategoria = reader.GetInt32(reader.GetOrdinal("idCategoria"));
                    oLibro.Editorial = reader["Editorial"].ToString();
                }
                connection.Close();
                reader.Close();
            }
            return oLibro;
        }
        public List<int> ListarId(string nombre)
        {
            List<int> listaIDLibros = new List<int>();
            string consultaSql = "SELECT l.Id FROM Libro l WHERE l.Titulo LIKE @nombreFiltro or l.Autor LIKE @nombreFiltro OR l.Editorial LIKE @nombreFiltro";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@nombreFiltro", nombre);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    listaIDLibros.Add(Convert.ToInt32(reader["Id"].ToString())); 
                }
                connection.Close();
                reader.Close();
            }
            return listaIDLibros;
        }
        public List<Libro> Listar()
        {
            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                string query = @"
                    SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.Autor, l.idCategoria, l.Editorial,
                    l.Ubicacion, l.numEjemplares, l.Estado
                    FROM LIBRO l
                    INNER JOIN biblioteca b ON b.Id = l.idBiblioteca
                    INNER JOIN CATEGORIA c ON c.Id = l.idCategoria";

                SqlCommand cmd = new SqlCommand(query, oConexion);
                cmd.CommandType = CommandType.Text;
                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        listaLibros.Add(new Libro()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Titulo = dr["Titulo"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            Autor = dr["Autor"].ToString(),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            Editorial = dr["Editorial"].ToString(),
                            Ubicacion = dr["Ubicacion"].ToString(),
                            numEjemplares = Convert.ToInt32(dr["numEjemplares"].ToString()),
                        });
                    }
                    dr.Close();

                    return listaLibros;

                }
                catch (Exception ex)
                {
                    listaLibros = null;
                    return listaLibros;
                }
            }
        }
        public List<Libro> ListarPorBiblioteca(int id)
        {
            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.Autor, l.idCategoria, l.Editorial, l.Idioma, l.ISBN, l.Ubicacion, l.numEjemplares, l.Estado " +
                                  "FROM LIBRO l " +
                                  "INNER JOIN biblioteca b ON b.Id = l.idBiblioteca " +
                                  "INNER JOIN CATEGORIA c ON c.Id = l.idCategoria " +
                                  "WHERE l.idBiblioteca = @idBiblioteca";

                cmd.Parameters.AddWithValue("@idBiblioteca", id);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        listaLibros.Add(new Libro()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Idioma = dr["Idioma"].ToString(),
                            Titulo = dr["Titulo"].ToString(),
                            ISBN = dr["ISBN"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            Autor = dr["Autor"].ToString(),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            Editorial = dr["Editorial"].ToString(),
                            Ubicacion = dr["Ubicacion"].ToString(),
                            numEjemplares = Convert.ToInt32(dr["numEjemplares"].ToString()),
                            //Estado = Convert.ToBoolean(dr["Estado"].ToString())
                        });
                    }
                    dr.Close();

                    return listaLibros;

                }
                catch (Exception ex)
                {
                    listaLibros = null;
                    return listaLibros;
                }
            }
        }
        public List<Libro> ListarLibroPorIdiomaCategoria(string idioma, string categoria)
        {
            List<Libro> listaLibros = new List<Libro>();
            string sql = "";

            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;

                if (string.IsNullOrEmpty(categoria)) // Seleccionado Idioma
                {
                    sql = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.Autor, l.idCategoria, l.Editorial, l.numEjemplares, l.Estado, l.Idioma " +
                        "FROM Libro l WHERE l.Idioma = @idioma";

                    cmd.Parameters.AddWithValue("@idioma", idioma);
                }
                else if (string.IsNullOrEmpty(idioma)) // Seleccionado Categoría
                {
                    sql = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.Autor, l.idCategoria, l.Editorial, l.numEjemplares, l.Estado " +
                        "FROM Libro l INNER JOIN Categoria c ON c.Nombre = @nombreCategoria " +
                        "WHERE c.Id = l.idCategoria";

                    cmd.Parameters.AddWithValue("@nombreCategoria", categoria);
                }
                else // Seleccionado ambos
                {
                    sql = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.Autor, l.idCategoria, l.Editorial, l.numEjemplares, l.Estado " +
                        "FROM Libro l INNER JOIN Categoria c ON c.Nombre = @nombreCategoria " +
                        "WHERE c.Id = l.idCategoria AND l.Idioma = @idioma";

                    cmd.Parameters.AddWithValue("@idioma", idioma);
                    cmd.Parameters.AddWithValue("@nombreCategoria", categoria);
                }

                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        listaLibros.Add(new Libro()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            Titulo = dr["Titulo"].ToString(),
                            //Idioma = dr["Idioma"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            Autor = dr["Autor"].ToString(),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            Editorial = dr["Editorial"].ToString(),
                            numEjemplares = Convert.ToInt32(dr["numEjemplares"].ToString()),
                        });
                    }
                    dr.Close();

                    return listaLibros;
                }
                catch (Exception ex)
                {
                    listaLibros = null;
                    return listaLibros;
                }
            }
        }

        public bool Registrar(Libro objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("Foto", objeto.Foto);
                    cmd.Parameters.AddWithValue("Idioma", objeto.Idioma);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("Autor", objeto.Autor);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.idCategoria);
                    cmd.Parameters.AddWithValue("Editorial", objeto.Editorial);
                    cmd.Parameters.AddWithValue("idBiblioteca", objeto.idBiblioteca);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("numEjemplares", objeto.numEjemplares);
                    cmd.Parameters.AddWithValue("ISBN", objeto.ISBN);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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


        public bool Modificar(Libro objeto)
        {
            bool respuesta = false;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarLibro", oConexion);
                    cmd.Parameters.AddWithValue("LibroId", objeto.Id); 
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("Foto", objeto.Foto);
                    cmd.Parameters.AddWithValue("Idioma", objeto.Idioma);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("Autor", objeto.Autor);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.idCategoria);
                    cmd.Parameters.AddWithValue("Editorial", objeto.Editorial);
                    cmd.Parameters.AddWithValue("idBiblioteca", objeto.idBiblioteca);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("numEjemplares", objeto.numEjemplares);
                    cmd.Parameters.AddWithValue("ISBN", objeto.ISBN);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
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

        public bool ActualizarRutaImagen(Libro objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_actualizarRutaImagen", oConexion);
                    cmd.Parameters.AddWithValue("Id", objeto.Id);
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.CommandType = CommandType.StoredProcedure;
                    oConexion.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    respuesta = false;
                }
            }
            return respuesta;
        }


        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from LIBRO where Id = @id", oConexion);
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