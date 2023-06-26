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

        public LibroDAL()
        {

        }

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
            string consultaSql = "SELECT Titulo, Foto, idBiblioteca, idAutor, idCategoria, idEditorial, numEjemplares " +
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
                    oLibro.Foto = reader["Foto"].ToString();
                    oLibro.numEjemplares = reader.GetInt32(reader.GetOrdinal("numEjemplares")); 
                    oLibro.idBiblioteca = reader.GetInt32(reader.GetOrdinal("idBiblioteca"));
                    oLibro.idAutor = reader.GetInt32(reader.GetOrdinal("idAutor"));
                    oLibro.idCategoria = reader.GetInt32(reader.GetOrdinal("idCategoria"));
                    oLibro.idEditorial = reader.GetInt32(reader.GetOrdinal("idEditorial"));
                }
                connection.Close();
                reader.Close();
            }
            return oLibro;
        }
        public List<int> ListarId(string nombre)
        {
            List<int> listaIDLibros = new List<int>();
            string consultaSql = "SELECT l.Id FROM Autor a inner join Libro l on a.Id = l.idAutor WHERE a.Nombre LIKE @nombreFiltro UNION ALL " +
                "SELECT l.Id FROM Editorial e inner join Libro l on e.Id = l.idAutor WHERE e.Nombre LIKE @nombreFiltro UNION ALL " +
                "SELECT l.Id FROM Libro l WHERE l.Titulo LIKE @nombreFiltro";

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
                    SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.idAutor, l.idCategoria, l.idEditorial,
                    l.Ubicacion, l.numEjemplares, l.Estado
                    FROM LIBRO l
                    INNER JOIN AUTOR a ON a.Id = l.idAutor
                    INNER JOIN biblioteca b ON b.Id = l.idBiblioteca
                    INNER JOIN CATEGORIA c ON c.Id = l.idCategoria
                    INNER JOIN EDITORIAL e ON e.Id = l.idEditorial";

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
                            idAutor = Convert.ToInt32(dr["idAutor"].ToString()),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            idEditorial = Convert.ToInt32(dr["idEditorial"].ToString()),
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
        public List<Libro> ListarPorBiblioteca(int id)
        {
            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.idAutor, l.idCategoria, l.idEditorial, l.Ubicacion, l.numEjemplares, l.Estado " +
                                  "FROM LIBRO l " +
                                  "INNER JOIN AUTOR a ON a.Id = l.idAutor " +
                                  "INNER JOIN biblioteca b ON b.Id = l.idBiblioteca " +
                                  "INNER JOIN CATEGORIA c ON c.Id = l.idCategoria " +
                                  "INNER JOIN EDITORIAL e ON e.Id = l.idEditorial " +
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
                            Titulo = dr["Titulo"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            idAutor = Convert.ToInt32(dr["idAutor"].ToString()),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            idEditorial = Convert.ToInt32(dr["idEditorial"].ToString()),                
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
        public List<Libro> ListarPorCategoria(string nombre)
        {

            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT l.Id, l.Titulo, l.Foto, l.idBiblioteca, l.idAutor, l.idCategoria, l.idEditorial, l.numEjemplares, l.Estado " +
                    "from Libro l inner join Categoria c on c.Nombre = @nombreCategoria " +
                    "where c.Id = l.idCategoria"; 

                cmd.Parameters.AddWithValue("@nombreCategoria", nombre);
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
                            idAutor = Convert.ToInt32(dr["idAutor"].ToString()),
                            idCategoria = Convert.ToInt32(dr["idCategoria"].ToString()),
                            idEditorial = Convert.ToInt32(dr["idEditorial"].ToString()),
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

        public int Registrar(Libro objeto)
        {
            int respuesta = 0;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLibro", oConexion);
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("Foto", objeto.Foto);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("idAutor", objeto.idAutor);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.idCategoria);
                    cmd.Parameters.AddWithValue("idEditorial", objeto.idEditorial);
                    cmd.Parameters.AddWithValue("idBiblioteca", objeto.idBiblioteca);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("numEjemplares", objeto.numEjemplares);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    respuesta = Convert.ToInt32(cmd.Parameters["Resultado"].Value);

                }
                catch (Exception ex)
                {
                    respuesta = 0;
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
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("Foto", objeto.Foto);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("idAutor", objeto.idAutor);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.idCategoria);
                    cmd.Parameters.AddWithValue("idEditorial", objeto.idEditorial);
                    cmd.Parameters.AddWithValue("Ubicacion", objeto.Ubicacion);
                    cmd.Parameters.AddWithValue("numEjemplares", objeto.numEjemplares);
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