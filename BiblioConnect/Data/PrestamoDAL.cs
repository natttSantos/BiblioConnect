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
                    cmd.Parameters.AddWithValue("Tipo", "prestamo");
                    cmd.Parameters.AddWithValue("IdBiblioteca", oPrestamo.idBiblioteca);
                    cmd.Parameters.AddWithValue("IdLibro", oPrestamo.idLibro);
                    cmd.Parameters.AddWithValue("IdLector", oPrestamo.idLector);
                    cmd.Parameters.AddWithValue("EstadoEntregado", oPrestamo.EstadoEntregado);
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
        public bool Validar(int id)
        {
            bool disponible = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(
                        "SELECT 1 FROM Prestamo P INNER JOIN Transaccion T ON P.Id = T.Id" +
                        " WHERE T.idLibro = @idLibro AND P.Estado = 1", oConexion);

                    cmd.Parameters.AddWithValue("@idLibro", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (!reader.HasRows)
                        {
                            disponible = true;
                        }
                        else
                        {
                            disponible = false;
                        }
                    }

                }
                catch (Exception ex)
                {
                    disponible = true;
                }

            }
            return disponible;
        }
        public bool ValidarPrimerPrestamo(int id)
        {
            bool primerPrestamo = true; 
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS CantidadRegistros " +
                        "FROM Transaccion T INNER JOIN Prestamo R ON T.Id = R.Id" +
                        "WHERE T.idLibro = @idLibro", oConexion);

                    cmd.Parameters.AddWithValue("@idLibro", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        primerPrestamo = false; 
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                }
            }

            return primerPrestamo;
        }
        public List<object> Listar(int idBiblioteca)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("SELECT p.FechaDevolucion, p.FechaDevolConfirmada, p.Estado, p.EstadoEntregado, p.EstadoRecibido, t.Id, " +
                                                 "l.Titulo, lec.Dni, lec.Apellidos, u.Nombre " +
                                                 "FROM Prestamo p " +
                                                 "INNER JOIN Transaccion t ON t.Id = p.Id " +
                                                 "INNER JOIN Biblioteca b ON b.Id = @idBiblioteca " +
                                                 "INNER JOIN Lector lec ON lec.Id = t.idLector " +
                                                 "INNER JOIN Libro l ON l.Id = t.idLibro " +
                                                 "INNER JOIN Usuario u ON u.Id = lec.Id where b.Id = t.idBiblioteca", connection);

                cmd.Parameters.AddWithValue("@idBiblioteca", idBiblioteca);

                DataTable dataTable = new DataTable();
                connection.Open();

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    adapter.Fill(dataTable);
                }

                List<object> listaDatos = new List<object>();

                foreach (DataRow row in dataTable.Rows)
                {
                    // Check for null value before converting to DateTime
                    DateTime? fechaDevolConfirmada = row["FechaDevolConfirmada"] != DBNull.Value
                        ? Convert.ToDateTime(row["FechaDevolConfirmada"])
                        : (DateTime?)null;
                    // Crear un objeto anónimo con los datos de cada fila y agregarlo a la lista
                    var datos = new
                    {
                        FechaDevolucion = Convert.ToDateTime(row["FechaDevolucion"]),
                        FechaDevolConfirmada = fechaDevolConfirmada,
                        Estado = Convert.ToBoolean(row["Estado"]),
                        EstadoEntregado = Convert.ToString(row["EstadoEntregado"]),
                        EstadoRecibido = Convert.ToString(row["EstadoRecibido"]),
                        Titulo = Convert.ToString(row["Titulo"]),
                        Dni = Convert.ToString(row["Dni"]),
                        Apellidos = Convert.ToString(row["Apellidos"]),
                        Nombre = Convert.ToString(row["Nombre"]), 
                        Id = int.Parse(row["Id"].ToString())
                };

                    listaDatos.Add(datos);
                }

                return listaDatos;
            }
        }

        public Prestamo Obtener(int Id)
        {
            Prestamo oPrestamo = new Prestamo();
            string consultaSql = "SELECT  P.FechaEntrega,  P.FechaDevolConfirmada,  P.FechaDevolucion,  P.EstadoEntregado,  P.Duracion " +
                "FROM Prestamo P INNER JOIN Transaccion T ON P.Id = T.Id " +
                "WHERE T.idLibro = @libroId";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@libroId", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oPrestamo.EstadoEntregado = reader["EstadoEntregado"].ToString();
                    oPrestamo.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"].ToString(), new CultureInfo("es-PE"));
                    oPrestamo.FechaDevolucion = Convert.ToDateTime(reader["FechaDevolucion"].ToString(), new CultureInfo("es-PE"));
                    oPrestamo.Duracion = reader.GetInt32(reader.GetOrdinal("Duracion"));
                }

                connection.Close();
                reader.Close();
            }
            return oPrestamo;
        }
        public Prestamo ObtenerUltimo(int Id)
        {
            Prestamo oPrestamo = new Prestamo();
            string consultaSql = "SELECT MIN(p.FechaDevolConfirmada) AS UltimoPrestamo, p.EstadoEntregado " +
                "FROM Prestamo p INNER JOIN Transaccion t ON t.Id = p.Id " +
                "WHERE t.idLibro = @idLibro GROUP BY p.EstadoEntregado";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@idLibro", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oPrestamo.EstadoEntregado = reader["EstadoEntregado"].ToString();
                    oPrestamo.FechaDevolConfirmada = Convert.ToDateTime(reader["UltimoPrestamo"].ToString(), new CultureInfo("es-PE"));
                }
                connection.Close();
                reader.Close();
            }
            return oPrestamo;
        }

        public bool Devolver(Prestamo oPrestamo)
        {
            {
                bool respuesta = true;
                using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
                {
                    try
                    {
                        SqlCommand cmd = new SqlCommand("sp_RegistrarRecepcionPrestamo", oConexion);
                        cmd.Parameters.AddWithValue("Id", oPrestamo.Id); 
                        cmd.Parameters.AddWithValue("EstadoRecibido", oPrestamo.EstadoRecibido);
                        cmd.Parameters.AddWithValue("Estado", 0);
                        cmd.Parameters.AddWithValue("FechaDevolConfirmada", DateTime.Now);
                        cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        cmd.CommandType = CommandType.StoredProcedure;

                        oConexion.Open();

                        cmd.ExecuteNonQuery();

                        respuesta = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);

                    }
                    catch (Exception ex)
                    {
                        respuesta = false;
                    }
                }
                return respuesta;
            }
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
}