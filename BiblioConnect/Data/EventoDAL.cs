using BiblioConnect.Data;
using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace BiblioConnect.Data
{
    public class EventoDAL
    {

        private static EventoDAL instancia = null;

        public EventoDAL()
        {

        }

        public static EventoDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new EventoDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Evento oEvento)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarEvento", oConexion);
                    cmd.Parameters.AddWithValue("NombreAutor", oEvento.NombreAutor);
                    cmd.Parameters.AddWithValue("Foto", oEvento.Foto);
                    cmd.Parameters.AddWithValue("DescripcionAutor", oEvento.DescripcionAutor);
                    cmd.Parameters.AddWithValue("FechaRealizacion", oEvento.FechaRealizacion);
                    cmd.Parameters.AddWithValue("HoraInicio", oEvento.HoraInicio);
                    cmd.Parameters.AddWithValue("HoraFin", oEvento.HoraFin);
                    cmd.Parameters.AddWithValue("Tipo", oEvento.Tipo);
                    cmd.Parameters.AddWithValue("idBiblioteca", oEvento.idBiblioteca);
                    cmd.Parameters.AddWithValue("DescripcionEvento", oEvento.DescripcionEvento);
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
        public bool Modificar(Evento oEvento)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ModificarEvento", oConexion);
                    cmd.Parameters.AddWithValue("EventoId", oEvento.Id);
                    cmd.Parameters.AddWithValue("NombreAutor", oEvento.NombreAutor);
                    cmd.Parameters.AddWithValue("Foto", oEvento.Foto);
                    cmd.Parameters.AddWithValue("DescripcionAutor", oEvento.DescripcionAutor);
                    cmd.Parameters.AddWithValue("FechaRealizacion", oEvento.FechaRealizacion);
                    cmd.Parameters.AddWithValue("HoraInicio", oEvento.HoraInicio);
                    cmd.Parameters.AddWithValue("HoraFin", oEvento.HoraFin);
                    cmd.Parameters.AddWithValue("Tipo", oEvento.Tipo);
                    cmd.Parameters.AddWithValue("idBiblioteca", oEvento.idBiblioteca);
                    cmd.Parameters.AddWithValue("DescripcionEvento", oEvento.DescripcionEvento);
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
        public List<Evento> ListarPorBiblioteca(int id)
        {
            List<Evento> lista = new List<Evento>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT e.NombreAutor, e.Id, e.Foto, e.DescripcionAutor, e.DescripcionEvento, e.FechaRealizacion, e.HoraInicio, e.HoraFin, e.Tipo " +
                    "FROM Evento e INNER JOIN biblioteca b ON b.Id = e.idBiblioteca " +
                    "WHERE e.idBiblioteca = @idBiblioteca";

                cmd.Parameters.AddWithValue("@idBiblioteca", id);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Evento()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            NombreAutor = dr["NombreAutor"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            DescripcionAutor = dr["DescripcionAutor"].ToString(),
                            DescripcionEvento = dr["DescripcionEvento"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            HoraInicio = dr["HoraInicio"].ToString(),
                            HoraFin = dr["HoraFin"].ToString(),
                            FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]),
                        });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    lista = null;
                }
                return lista;
            }
        }
        public List<Evento> Listar()
        {
            List<Evento> lista = new List<Evento>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT e.Id, e.idBiblioteca, e.NombreAutor, e.Foto, e.DescripcionAutor, e.DescripcionEvento, e.FechaRealizacion, e.HoraInicio, e.HoraFin, e.Tipo " +
                    "FROM Evento e INNER JOIN biblioteca b ON b.Id = e.idBiblioteca "; 

                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Evento()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            NombreAutor = dr["NombreAutor"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            DescripcionAutor = dr["DescripcionAutor"].ToString(),
                            DescripcionEvento = dr["DescripcionEvento"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            HoraInicio = dr["HoraInicio"].ToString(),
                            HoraFin = dr["HoraFin"].ToString(),
                            FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]),
                        });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    lista = null;
                }
                return lista;
            }
        }
        public List<Evento> ListarPorTipo(string tipo)
        {
            List<Evento> lista = new List<Evento>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = oConexion;
                cmd.CommandText = "SELECT e.Id, e.idBiblioteca, e.NombreAutor, e.Foto, e.DescripcionAutor, e.DescripcionEvento, e.FechaRealizacion, e.HoraInicio, e.HoraFin, e.Tipo " +
                    "FROM Evento e INNER JOIN biblioteca b ON b.Id = e.idBiblioteca " +
                    "WHERE e.Tipo = @tipoEvento";

                cmd.Parameters.AddWithValue("@tipoEvento", tipo);
                cmd.CommandType = CommandType.Text;

                try
                {
                    oConexion.Open();
                    SqlDataReader dr = cmd.ExecuteReader();

                    while (dr.Read())
                    {
                        lista.Add(new Evento()
                        {
                            Id = Convert.ToInt32(dr["Id"].ToString()),
                            idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString()),
                            NombreAutor = dr["NombreAutor"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            DescripcionAutor = dr["DescripcionAutor"].ToString(),
                            DescripcionEvento = dr["DescripcionEvento"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            HoraInicio = dr["HoraInicio"].ToString(),
                            HoraFin = dr["HoraFin"].ToString(),
                            FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]),
                        });
                    }
                    dr.Close();
                }
                catch (Exception ex)
                {
                    lista = null;
                }
                return lista;
            }
        }
        public Evento Obtener(int Id)
        {
            Evento oEvento = new Evento();
            string consultaSql = "SELECT e.Id, e.idBiblioteca, e.NombreAutor, e.Foto, e.DescripcionAutor, e.DescripcionEvento, e.FechaRealizacion, e.HoraInicio, e.HoraFin, e.Tipo " +
                "FROM Evento e " +
                "WHERE e.Id = @eventoId ";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@eventoId", Id);
                connection.Open();

                SqlDataReader dr = command.ExecuteReader();
                while (dr.Read())
                {
                    oEvento.Id = Convert.ToInt32(dr["Id"].ToString());
                    oEvento.idBiblioteca = Convert.ToInt32(dr["idBiblioteca"].ToString());
                    oEvento.NombreAutor = dr["NombreAutor"].ToString();
                    oEvento.Foto = dr["Foto"].ToString();
                    oEvento.DescripcionAutor = dr["DescripcionAutor"].ToString();
                    oEvento.DescripcionEvento = dr["DescripcionEvento"].ToString();
                    oEvento.Tipo = dr["Tipo"].ToString();
                    oEvento.HoraInicio = dr["HoraInicio"].ToString();
                    oEvento.HoraFin = dr["HoraFin"].ToString(); 
                    oEvento.FechaRealizacion = Convert.ToDateTime(dr["FechaRealizacion"]); 
                }

                connection.Close();
                dr.Close();
            }
            return oEvento;
        }

        public bool Eliminar(int id)
        {
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("delete from Evento where Id = @id", oConexion);
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