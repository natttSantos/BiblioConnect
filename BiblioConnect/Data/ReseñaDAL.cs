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
    public class ReseñaDAL
    {

        private static ReseñaDAL instancia = null;

        public ReseñaDAL()
        {


        }

        public static ReseñaDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new ReseñaDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Reseña oReseña)
        {
            oReseña.Fecha = DateTime.Now;
            bool respuesta = true;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarReseña", oConexion);
                    cmd.Parameters.AddWithValue("Descripcion", oReseña.Descripcion);
                    cmd.Parameters.AddWithValue("idLector", oReseña.idLector);
                    cmd.Parameters.AddWithValue("idLibro", oReseña.idLibro);
                    cmd.Parameters.AddWithValue("Puntuacion", oReseña.Puntuacion);
                    cmd.Parameters.AddWithValue("Fecha", Convert.ToDateTime(oReseña.Fecha, new CultureInfo("es-PE")));
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

        public List<Reseña> Listar(int id)
        {
            List<Reseña> Lista = new List<Reseña>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("select Descripcion, idLector, Puntuacion, Fecha " +
                        "from Reseña " +
                        "where idLibro = @idLibro", oConexion);
                    cmd.Parameters.AddWithValue("@idLibro", id);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Reseña()
                            {
                                idLector = Convert.ToInt32(dr["idLector"]),
                                Descripcion = dr["Descripcion"].ToString(),
                                Puntuacion = Convert.ToInt32(dr["Puntuacion"]),
                                Fecha = Convert.ToDateTime(dr["Fecha"].ToString(), new CultureInfo("es-PE"))
                        });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Reseña>();
                }
            }
            return Lista;
        }
    }
}