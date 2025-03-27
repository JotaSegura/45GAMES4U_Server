using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Entities;

namespace ServerApp
{
    public class AccesoDatos
    {
        private string _cadenaConexion = "Data Source=JOTA\\SQLEXPRESS;Initial Catalog=GAMES4U2;Integrated Security=True;Encrypt=False;";


        // Insertar nuevo tipo de Videojuego
        public bool InsertarTipoVideojuego(TipoVideojuegoEntidad tipo)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = "INSERT INTO TipoVideojuego (Nombre, Descripcion) VALUES (@Nombre, @Descripcion)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", tipo.Nombre);
                cmd.Parameters.AddWithValue("@Descripcion", tipo.Descripcion);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Obtener todos los tipos
        public List<TipoVideojuegoEntidad> ObtenerTiposVideojuegos()
        {
            List<TipoVideojuegoEntidad> tipos = new List<TipoVideojuegoEntidad>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT Id, Nombre, Descripcion FROM TipoVideojuego";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    tipos.Add(new TipoVideojuegoEntidad
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Nombre = reader["Nombre"].ToString(),
                        Descripcion = reader["Descripcion"].ToString()
                    });
                }
            }
            return tipos;
        }

       //Método para validar cliente
        public ClienteEntidad ValidarCliente(int identificacion)
        {
            ClienteEntidad cliente = null;

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT Nombre, PrimerApellido, SegundoApellido, JugadorEnLinea " +
                              "FROM Cliente WHERE Identificacion = @Identificacion";

                SqlCommand comando = new SqlCommand(query, conexion);
                comando.Parameters.AddWithValue("@Identificacion", identificacion);

                conexion.Open();
                SqlDataReader reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    cliente = new ClienteEntidad
                    {
                        Identificacion = identificacion,
                        Nombre = reader["Nombre"].ToString(),
                        PrimerApellido = reader["PrimerApellido"].ToString(),
                        SegundoApellido = reader["SegundoApellido"].ToString(),
                        JugadorEnLinea = (bool)reader["JugadorEnLinea"]
                    };
                }
            }

            return cliente;
        }

        public bool InsertarVideojuego(VideojuegoEntidad videojuego)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Videojuego 
                        (Nombre, Id_TipoVideojuego, Desarrollador, Lanzamiento, Fisico)
                        VALUES (@Nombre, @IdTipo, @Desarrollador, @Lanzamiento, @Fisico)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", videojuego.Nombre);
                cmd.Parameters.AddWithValue("@IdTipo", videojuego.TipoVideojuego.Id);
                cmd.Parameters.AddWithValue("@Desarrollador", videojuego.Desarrollador);
                cmd.Parameters.AddWithValue("@Lanzamiento", videojuego.Lanzamiento);
                cmd.Parameters.AddWithValue("@Fisico", videojuego.Fisico);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<VideojuegoEntidad> ObtenerVideojuegos()
        {
            List<VideojuegoEntidad> videojuegos = new List<VideojuegoEntidad>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"SELECT v.Id, v.Nombre, v.Desarrollador, v.Lanzamiento, v.Fisico,
                        t.Id as TipoId, t.Nombre as TipoNombre
                        FROM Videojuego v
                        INNER JOIN TipoVideojuego t ON v.Id_TipoVideojuego = t.Id";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        videojuegos.Add(new VideojuegoEntidad
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Desarrollador = reader["Desarrollador"].ToString(),
                            Lanzamiento = Convert.ToInt32(reader["Lanzamiento"]),
                            Fisico = Convert.ToBoolean(reader["Fisico"]),
                            TipoVideojuego = new TipoVideojuegoEntidad
                            {
                                Id = Convert.ToInt32(reader["TipoId"]),
                                Nombre = reader["TipoNombre"].ToString()
                            }
                        });
                    }
                }
            }
            return videojuegos;
        }

        // Obtener tipos de videojuego para el ComboBox
        public List<TipoVideojuegoEntidad> ObtenerTiposParaCombo()
        {
            return ObtenerTiposVideojuegos(); // Reutilizamos el método existente
        }
    }
}