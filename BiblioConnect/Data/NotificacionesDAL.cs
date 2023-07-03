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
            DateTime fechaActual = DateTime.Now;
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
                    cmd.Parameters.AddWithValue("FechaEnvio", Convert.ToDateTime(fechaActual, new CultureInfo("es-PE")));
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
    }
}