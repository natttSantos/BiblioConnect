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
    public class LectorDAL
    {

        private static LectorDAL instancia = null;

        public LectorDAL()
        {

        }

        public static LectorDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new LectorDAL();
                }

                return instancia;
            }
        }

        public bool Registrar(Lector oLector)
        {
            bool registrado = true;
            string mensaje;
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarLector", oConexion);
                    cmd.Parameters.AddWithValue("Nombre", oLector.Nombre);
                    cmd.Parameters.AddWithValue("Apellidos", oLector.Apellidos);
                    cmd.Parameters.AddWithValue("Dni", oLector.Dni);
                    cmd.Parameters.AddWithValue("Email", oLector.Email);
                    cmd.Parameters.AddWithValue("Telefono", oLector.Telefono);
                    cmd.Parameters.AddWithValue("FechaNacimiento", oLector.FechaNacimiento);
                    cmd.Parameters.AddWithValue("Contraseña", oLector.Contraseña);
                    cmd.Parameters.AddWithValue("Calle", oLector.Calle);
                    cmd.Parameters.AddWithValue("Pais", oLector.Pais);
                    cmd.Parameters.AddWithValue("Estado", oLector.Estado);
                    cmd.Parameters.AddWithValue("CodPostal", oLector.CodPostal);
                    cmd.Parameters.AddWithValue("Foto", oLector.Foto);
                    cmd.Parameters.AddWithValue("Ciudad", oLector.Ciudad);
                    cmd.Parameters.AddWithValue("TipoUsuario", "lector");
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
        public Lector Obtener(int Id)
        {
            Lector lector = new Lector();
            string consultaSql = "select u.Nombre, u.Foto " +
                "from Lector l inner join Usuario u on l.Id = u.Id " +
                "where l.Id = @idLector";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@idLector", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    lector.Nombre = reader["Nombre"].ToString();
                    lector.Foto = reader["Foto"].ToString();
                }
                connection.Close();
                reader.Close();
            }
            return lector;
        }

    }
}