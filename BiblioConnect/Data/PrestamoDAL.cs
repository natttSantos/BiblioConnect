using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace BiblioConnect.Data
{
    public class PrestamoDAL
    {
        private static PrestamoDAL instancia = null;

        public PrestamoDAL()
        {

        }

        public static PrestamoDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new PrestamoDAL();
                }

                return instancia;
            }
        }
        public bool Registrar(Prestamo oPrestamo)
        {
            DateTime fechaActual = DateTime.Now;
            DateTime fechaDevolucion = fechaActual.AddDays(oPrestamo.Duracion); 

            bool registrado = true;
            string mensaje;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("Tipo", oPrestamo.Tipo);
                    cmd.Parameters.AddWithValue("IdBiblioteca", oPrestamo.idBiblioteca);
                    cmd.Parameters.AddWithValue("IdLibro", oPrestamo.idLibro);
                    cmd.Parameters.AddWithValue("IdLector", oPrestamo.idLector);
                    cmd.Parameters.AddWithValue("Descripcion", oPrestamo.Descripcion);
                    cmd.Parameters.AddWithValue("Duracion", oPrestamo.Duracion);
                    cmd.Parameters.AddWithValue("FechaEntrega", Convert.ToDateTime(fechaActual, new CultureInfo("es-PE")));
                    cmd.Parameters.AddWithValue("FechaDevolucion", Convert.ToDateTime(fechaDevolucion, new CultureInfo("es-PE")));
                    cmd.Parameters.AddWithValue("Estado", oPrestamo.Estado);
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

        //public bool Modificar(Categoria oCategoria)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_ModificarCategoria", oConexion);
        //            cmd.Parameters.AddWithValue("Id", oCategoria.Id);
        //            cmd.Parameters.AddWithValue("Nombre", oCategoria.Nombre);
        //            cmd.Parameters.AddWithValue("Estado", oCategoria.Estado);
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


        //public List<Categoria> Listar()
        //{
        //    List<Categoria> Lista = new List<Categoria>();
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("Select Id,Nombre,Estado from Categoria", oConexion);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Lista.Add(new Categoria()
        //                    {
        //                        Id = Convert.ToInt32(dr["Id"]),
        //                        Nombre = dr["Nombre"].ToString(),
        //                        Estado = Convert.ToBoolean(dr["Estado"])
        //                    });
        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            Lista = new List<Categoria>();
        //        }
        //    }
        //    return Lista;
        //}
        //public Categoria Obtener(int Id)
        //{
        //    Categoria oCategoria = new Categoria();
        //    string consultaSql = "SELECT Nombre " +
        //        "FROM Categoria " +
        //        "WHERE Id = @categoriaId ";

        //    using (SqlConnection connection = new SqlConnection(Conexion.CN))
        //    {
        //        SqlCommand command = new SqlCommand(consultaSql, connection);
        //        command.Parameters.AddWithValue("@categoriaId", Id);
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            oCategoria.Nombre = reader["Nombre"].ToString();
        //        }

        //        connection.Close();
        //        reader.Close();
        //    }
        //    return oCategoria;
        //}
        //public bool Eliminar(int id)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("delete from Categoria where Id = @id", oConexion);
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