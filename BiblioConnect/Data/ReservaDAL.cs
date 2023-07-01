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
    public class ReservaDAL
    {
        private static ReservaDAL instancia = null;

        public ReservaDAL()
        {

        }

        public static ReservaDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ReservaDAL();
                }

                return instancia;
            }
        }
        public bool Registrar(Reserva oReserva)
        {
            DateTime fechaActual = DateTime.Now;

            bool registrado = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarReserva", oConexion);
                    cmd.Parameters.AddWithValue("IdBiblioteca", oReserva.idBiblioteca);
                    cmd.Parameters.AddWithValue("IdLibro", oReserva.idLibro);
                    cmd.Parameters.AddWithValue("IdLector", oReserva.idLector);
                    cmd.Parameters.AddWithValue("FechaReserva", Convert.ToDateTime(fechaActual, new CultureInfo("es-PE")));
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
        public DateTime ObtenerUltimaFecha(int id)
        {
            DateTime fechaReserva = DateTime.MinValue; // Inicializar variable
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT MIN(R.FechaReserva) AS FechaReservaMasAntigua " +
                    "FROM Transaccion T INNER JOIN Reserva R ON T.Id = R.Id " +
                    "WHERE T.idLibro = @idLibro", oConexion);

                    cmd.Parameters.AddWithValue("@idLibro", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        fechaReserva = Convert.ToDateTime(result);
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                }
            }

            return fechaReserva;
        }
        public bool Validar(Reserva oReserva)
        {
            bool usuarioRepetido = false; 
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT COUNT(*) AS CantidadRegistros " + 
                    "FROM Reserva r INNER JOIN Transaccion t ON r.Id = t.Id " +
                    "WHERE t.idLector = @idLector AND t.idLibro = @idLibro", oConexion);

                    cmd.Parameters.AddWithValue("@idLibro", oReserva.idLibro);
                    cmd.Parameters.AddWithValue("@idLector", oReserva.idLector);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    int cantidadRegistros = Convert.ToInt32(cmd.ExecuteScalar());
                    if (cantidadRegistros != 0)
                    {
                        usuarioRepetido = true;
                    }
                }
                catch (Exception ex)
                {
                    // Manejar la excepción
                }
            }

            return usuarioRepetido;
        }
        public List<object> Listar(int idBiblioteca, string estado)
        {
            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand("select t.Id, t.Estado, r.FechaReserva, l.Titulo, lec.Dni, lec.Apellidos, u.Nombre " +
                    "from Reserva r inner join Transaccion t on t.Id = r.Id " +
                    "inner join Biblioteca b on b.Id = t.idBiblioteca " +
                    "inner join Lector lec on lec.Id = t.idLector " +
                    "inner join Libro l on l.Id = t.idLibro " +
                    "inner join Usuario u on u.Id = lec.Id " +
                    "where b.Id = @idBiblioteca AND T.Estado = @estado", connection);

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
                    DateTime? fechaReserva = row["FechaReserva"] != DBNull.Value ? Convert.ToDateTime(row["FechaReserva"]) : (DateTime?)null;

                    // Crear un objeto anónimo con los datos de cada fila y agregarlo a la lista
                    var datos = new
                    {
                        FechaReserva = fechaReserva,
                        Titulo = Convert.ToString(row["Titulo"]),
                        Estado = Convert.ToString(row["Estado"]),
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
        //public Prestamo Obtener(int Id)
        //{
        //    Prestamo oPrestamo = new Prestamo();
        //    string consultaSql = "SELECT  P.FechaEntrega,  P.FechaDevolConfirmada,  P.FechaDevolucion,  P.Descripcion,  P.Duracion " +
        //        "FROM Prestamo P INNER JOIN Transaccion T ON P.Id = T.Id " +
        //        "WHERE T.idLibro = @libroId";

        //    using (SqlConnection connection = new SqlConnection(Conexion.CN))
        //    {
        //        SqlCommand command = new SqlCommand(consultaSql, connection);
        //        command.Parameters.AddWithValue("@libroId", Id);
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            oPrestamo.Descripcion = reader["Descripcion"].ToString();
        //            oPrestamo.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"].ToString(), new CultureInfo("es-PE"));
        //            oPrestamo.FechaDevolucion = Convert.ToDateTime(reader["FechaDevolucion"].ToString(), new CultureInfo("es-PE"));
        //            oPrestamo.Duracion = reader.GetInt32(reader.GetOrdinal("Duracion"));
        //        }

        //        connection.Close();
        //        reader.Close();
        //    }
        //    return oPrestamo;
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