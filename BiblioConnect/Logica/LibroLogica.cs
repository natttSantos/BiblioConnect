using BiblioConnect.Logica;
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

namespace ProyectoBiblioteca.Logica
{
    public class LibroLogica
    {

        private static LibroLogica instancia = null;

        public LibroLogica()
        {

        }

        public static LibroLogica Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LibroLogica();
                }

                return instancia;
            }
        }

        public List<Libro> Listar()
        {

            List<Libro> listaLibros = new List<Libro>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("select l.Id,l.Titulo,l.Foto,");
                sb.AppendLine("a.Id,a.Descripcion[DescripcionAutor],");
                sb.AppendLine("c.Id,c.Descripcion[DescripcionCategoria],");
                sb.AppendLine("e.Id,e.Descripcion[DescripcionEditorial],");
                sb.AppendLine("l.Ubicacion,l.numEjemplares,l.Estado");
                sb.AppendLine("from LIBRO l");
                sb.AppendLine("inner join AUTOR a on a.Id = l.idAutor");
                sb.AppendLine("inner join CATEGORIA c on c.Id = l.idCategoria");
                sb.AppendLine("inner join EDITORIAL e on e.Id = l.idEditorial");

                SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
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
                            oAutor = new Autor() { Id = Convert.ToInt32(dr["Id"].ToString()), Descripcion = dr["DescripcionAutor"].ToString() },
                            oCategoria = new Categoria() { Id = Convert.ToInt32(dr["Id"].ToString()), Descripcion = dr["DescripcionCategoria"].ToString() },
                            oEditorial = new Editorial() { Id = Convert.ToInt32(dr["Id"].ToString()), Descripcion = dr["DescripcionEditorial"].ToString() },
                            Ubicacion = dr["Ubicacion"].ToString(),
                            numEjemplares = Convert.ToInt32(dr["numEjemplares"].ToString()),
                            //base64 = Utilidades.convertirBase64(Path.Combine(dr["RutaPortada"].ToString(), dr["NombrePortada"].ToString())),
                            //extension = Path.GetExtension(dr["NombrePortada"].ToString()).Replace(".", ""),
                            Estado = Convert.ToBoolean(dr["Estado"].ToString())
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
                    SqlCommand cmd = new SqlCommand("sp_registrarLibro", oConexion);
                    cmd.Parameters.AddWithValue("Titulo", objeto.Titulo);
                    cmd.Parameters.AddWithValue("Foto", objeto.Foto);
                    cmd.Parameters.AddWithValue("Estado", objeto.Estado);
                    cmd.Parameters.AddWithValue("idAutor", objeto.oAutor.Id);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.oCategoria.Id);
                    cmd.Parameters.AddWithValue("idEditorial", objeto.oEditorial.Id);
                    cmd.Parameters.AddWithValue("idBiblioteca", objeto.oBiblioteca.Id);
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
                    cmd.Parameters.AddWithValue("idAutor", objeto.oAutor.Id);
                    cmd.Parameters.AddWithValue("idCategoria", objeto.oCategoria.Id);
                    cmd.Parameters.AddWithValue("idEditorial", objeto.oEditorial.Id);
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