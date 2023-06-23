using BiblioConnect.Modelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace BiblioConnect.Data
{
    public class CategoriaDAL
    {
        private static CategoriaDAL instancia = null;

        public CategoriaDAL()
        {

        }

        public static CategoriaDAL Instancia
        {
            get
            {
                if (instancia == null)
                {
                    instancia = new CategoriaDAL();
                }

                return instancia;
            }
        }

        public List<Categoria> Listar()
        {
            List<Categoria> Lista = new List<Categoria>();
            using (SqlConnection oConexion = new SqlConnection(Conexion.CN))
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("Select Id,Nombre from Categoria", oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            Lista.Add(new Categoria()
                            {
                                Id = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                            });
                        }
                    }

                }
                catch (Exception ex)
                {
                    Lista = new List<Categoria>();
                }
            }
            return Lista;
        }
        public Categoria Obtener(int Id)
        {
            Categoria oCategoria = new Categoria();
            string consultaSql = "SELECT Nombre " +
                "FROM Categoria " +
                "WHERE Id = @categoriaId ";

            using (SqlConnection connection = new SqlConnection(Conexion.CN))
            {
                SqlCommand command = new SqlCommand(consultaSql, connection);
                command.Parameters.AddWithValue("@categoriaId", Id);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    oCategoria.Nombre = reader["Nombre"].ToString();
                }

                connection.Close();
                reader.Close();
            }
            return oCategoria;
        }
    }
}