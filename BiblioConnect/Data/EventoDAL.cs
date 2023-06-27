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
                    cmd.Parameters.AddWithValue("Hora", oEvento.Hora);
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
                cmd.CommandText = "SELECT e.NombreAutor, e.Foto, e.DescripcionAutor, e.DescripcionEvento, e.FechaRealizacion, e.Hora, e.Tipo " +
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
                            NombreAutor = dr["NombreAutor"].ToString(),
                            Foto = dr["Foto"].ToString(),
                            DescripcionAutor = dr["DescripcionAutor"].ToString(),
                            DescripcionEvento = dr["DescripcionEvento"].ToString(),
                            Tipo = dr["Tipo"].ToString(),
                            Hora = dr["Hora"].ToString(), 
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
        //public bool Modificar(Autor oAutor)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("sp_ModificarAutor", oConexion);
        //            cmd.Parameters.AddWithValue("Id", oAutor.Id);
        //            cmd.Parameters.AddWithValue("Nombre", oAutor.Nombre);
        //            cmd.Parameters.AddWithValue("Estado", oAutor.Estado);
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


        //public List<Autor> Listar(int id)
        //{
        //    List<Autor> Lista = new List<Autor>();
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("select Id,Nombre,Estado from Autor where idBiblioteca = @idBiblioteca", oConexion);
        //            cmd.Parameters.AddWithValue("@idBiblioteca", id);
        //            cmd.CommandType = CommandType.Text;

        //            oConexion.Open();
        //            using (SqlDataReader dr = cmd.ExecuteReader())
        //            {
        //                while (dr.Read())
        //                {
        //                    Lista.Add(new Autor()
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
        //            Lista = new List<Autor>();
        //        }
        //    }
        //    return Lista;
        //}

        //public Autor Obtener(int Id)
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
        //        }

        //        connection.Close();
        //        reader.Close();
        //    }
        //    return oAutor;
        //}
        //public bool Eliminar(int id)
        //{
        //    bool respuesta = true;
        //    using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("delete from Autor where Id = @id", oConexion);
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