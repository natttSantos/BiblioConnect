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
        public bool Eliminar(Prestamo objeto)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Notificacion " +
                        "where idLibro = @idLibro AND idLector = @idLector AND idBiblioteca = @idBiblioteca", oConexion);
                    cmd.Parameters.AddWithValue("@idLibro", objeto.idLibro);
                    cmd.Parameters.AddWithValue("@idLector", objeto.idLector);
                    cmd.Parameters.AddWithValue("@idBiblioteca", objeto.idBiblioteca);
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