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
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPrestamo", oConexion);
                    cmd.Parameters.AddWithValue("EstadoEntregado", oPrestamo.EstadoEntregado);
                    cmd.Parameters.AddWithValue("Id", oPrestamo.Id);
                    cmd.Parameters.AddWithValue("Duracion", oPrestamo.Duracion);
                    cmd.Parameters.AddWithValue("FechaEntrega", Convert.ToDateTime(fechaActual, new CultureInfo("es-PE")));
                    cmd.Parameters.AddWithValue("FechaDevolucion", Convert.ToDateTime(fechaDevolucion, new CultureInfo("es-PE")));
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);

                }
                catch (Exception ex)
                {
                    registrado = false;
                }
            }
            return registrado;
        }
        public bool RegistrarPendiente(Prestamo oPrestamo)
        {
            bool registrado = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarPendienteRecogida", oConexion);
                    cmd.Parameters.AddWithValue("IdBiblioteca", oPrestamo.idBiblioteca);
                    cmd.Parameters.AddWithValue("IdLibro", oPrestamo.idLibro);
                    cmd.Parameters.AddWithValue("IdLector", oPrestamo.idLector);
                    cmd.Parameters.Add("Registrado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oConexion.Open();

                    cmd.ExecuteNonQuery();

                    registrado = Convert.ToBoolean(cmd.Parameters["Registrado"].Value);

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
                        " WHERE T.idLibro = @idLibro AND T.Estado != 'Devuelto'", oConexion);

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

        //Recoge parametros de diferentes tablas por eso se crea un Object
        public List<object> Listar(int idBiblioteca, string estado)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                string sql = "select t.Id, p.FechaDevolucion, p.FechaDevolConfirmada, t.Estado, p.EstadoEntregado, p.EstadoRecibido, l.Id AS idLibro, l.Titulo, lec.Dni, lec.Apellidos, u.Nombre " +
                    "from Prestamo p inner join Transaccion t on t.Id = p.Id " +
                    "inner join Biblioteca b on b.Id = t.idBiblioteca " +
                    "inner join Lector lec on lec.Id = t.idLector " +
                    "inner join Libro l on l.Id = t.idLibro " +
                    "inner join Usuario u on u.Id = lec.Id " +
                    "where b.Id = @idBiblioteca";
                if (!estado.Equals("Todos"))
                {
                    sql += " AND T.Estado = @estado";
                }
                SqlCommand cmd = new SqlCommand(sql, connection);

                cmd.Parameters.AddWithValue("@idBiblioteca", idBiblioteca);
                cmd.Parameters.AddWithValue("@estado", estado);

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
                    DateTime? fechaDevolConfirmada = row["FechaDevolConfirmada"] != DBNull.Value ? Convert.ToDateTime(row["FechaDevolConfirmada"]) : (DateTime?)null;
                    DateTime? fechaDevol = row["FechaDevolucion"] != DBNull.Value ? Convert.ToDateTime(row["FechaDevolucion"]) : (DateTime?)null;
                    string estadoEntregado = row["EstadoEntregado"] != DBNull.Value ? Convert.ToString(row["EstadoEntregado"]) : null;
                    string estadoRecibido = row["EstadoRecibido"] != DBNull.Value ? Convert.ToString(row["EstadoRecibido"]) : null;


                    // Crear un objeto anónimo con los datos de cada fila y agregarlo a la lista
                    var datos = new
                    {
                        FechaDevolucion = fechaDevol, 
                        FechaDevolConfirmada = fechaDevolConfirmada,
                        EstadoEntregado = estadoEntregado, 
                        EstadoRecibido = estadoRecibido, 
                        Titulo = Convert.ToString(row["Titulo"]),
                        Estado = Convert.ToString(row["Estado"]),
                        Dni = Convert.ToString(row["Dni"]),
                        Apellidos = Convert.ToString(row["Apellidos"]),
                        Nombre = Convert.ToString(row["Nombre"]),
                        Id = int.Parse(row["Id"].ToString()),
                        idLibro = int.Parse(row["idLibro"].ToString())
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
        public Prestamo ObtenerIdentificadores(int id)
        {
            Prestamo oPrestamo = new Prestamo();
            string consultaSql = "SELECT t.idLector, t.idBiblioteca, t.idLibro " +
                "FROM Prestamo p INNER JOIN Transaccion t ON t.Id = p.Id " +
                "WHERE p.Id = @idPrestamo";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@idPrestamo", id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oPrestamo.idBiblioteca = int.Parse(reader["idBiblioteca"].ToString());
                    oPrestamo.idLector = int.Parse(reader["idLector"].ToString());
                    oPrestamo.idLibro = int.Parse(reader["idLibro"].ToString());
                }

                connection.Close();
                reader.Close();
            }
            return oPrestamo;
        }
        public Prestamo ObtenerUltimo(int Id)
        {
            Prestamo oPrestamo = new Prestamo();
            string consultaSql = "SELECT TOP 1 p.EstadoRecibido FROM Prestamo p " +
                "INNER JOIN Transaccion t ON t.Id = p.Id " +
                "WHERE t.idLibro = @idLibro AND T.Estado = 'Devuelto' " +
                "ORDER BY p.FechaDevolConfirmada DESC; ";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@idLibro", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oPrestamo.EstadoRecibido = reader["EstadoRecibido"].ToString();
                    //oPrestamo.FechaDevolConfirmada = Convert.ToDateTime(reader["UltimoPrestamo"].ToString(), new CultureInfo("es-PE"));
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
                        SqlCommand cmd = new SqlCommand("sp_RegistrarDevolucion", oConexion);
                        cmd.Parameters.AddWithValue("Id", oPrestamo.Id); 
                        cmd.Parameters.AddWithValue("EstadoRecibido", oPrestamo.EstadoRecibido);
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