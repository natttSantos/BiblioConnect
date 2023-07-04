using BiblioConnect.Data;
using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BiblioConnect.Data
{
    public class NotificacionDAL
    {

        private static NotificacionDAL instancia = null;

        public NotificacionDAL()
        {

        }

        public static NotificacionDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new NotificacionDAL();
                }

                return instancia;
            }
        }

        public bool Enviar(Notificacion oNotificacion)
        {
            oNotificacion.Descripcion = "El libro ya está disponible, puede ir a recogerlo hoy mismo."; 
            oNotificacion.FechaEnvio = DateTime.Now;
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_EnviarNotificacion", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oNotificacion.Descripcion);
                    cmd.Parameters.AddWithValue("idBiblioteca", oNotificacion.idBiblioteca);
                    cmd.Parameters.AddWithValue("idLector", oNotificacion.idLector);
                    cmd.Parameters.AddWithValue("idLibro", oNotificacion.idLibro);
                    cmd.Parameters.AddWithValue("FechaEnvio", Convert.ToDateTime(oNotificacion.FechaEnvio, new CultureInfo("es-PE")));
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

        public List<Notificacion> Listar(int id)
        {
            List<Notificacion> Lista = new List<Notificacion>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("SELECT Descripcion, FechaEnvio, idBiblioteca, idLibro " +
                        "FROM Notificacion " +
                        "Where idLector = @idLector", oConexion);
                    cmd.Parameters.AddWithValue("@idLector", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Notificacion()
                            {
                                Descripcion = dr["Descripcion"].ToString(),
                                idBiblioteca = Convert.ToInt32(dr["idBiblioteca"]),
                                idLibro = Convert.ToInt32(dr["idLibro"]),
                                FechaEnvio = Convert.ToDateTime(dr["FechaEnvio"].ToString(), new CultureInfo("es-PE"))
                        });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Notificacion>();
                }
            }
            return Lista;
        }

        //public Notificacion ListaPorLector(int Id)
        //{
        //    Autor oAutor = new Autor();
        //    string consultaSql = "SELECT Nombre " +
        //        "FROM Autor " +
        //        "WHERE Id = @autorId ";

        //    using (SqlConnection connection = new SqlConnection(Conexion.CN))
        //    {
        //        SqlCommand command = new SqlCommand(consultaSql, connection);
        //        command.Parameters.AddWithValue("@autorId", Id);
        //        connection.Open();

        //        SqlDataReader reader = command.ExecuteReader();
        //        while (reader.Read())
        //        {
        //            oAutor.Nombre = reader["Nombre"].ToString();
        //            oPrestamo.EstadoEntregado = reader["EstadoEntregado"].ToString();
        //            oPrestamo.FechaEntrega = Convert.ToDateTime(reader["FechaEntrega"].ToString(), new CultureInfo("es-PE"));
        //            oPrestamo.FechaDevolucion = Convert.ToDateTime(reader["FechaDevolucion"].ToString(), new CultureInfo("es-PE"));
        //            oPrestamo.Duracion = reader.GetInt32(reader.GetOrdinal("Duracion"));
        //        }

        //        connection.Close();
        //        reader.Close();
        //    }
        //    return oAutor;
        //}
    }
}