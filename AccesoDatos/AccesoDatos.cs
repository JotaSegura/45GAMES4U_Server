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

        // Insertar nuevo administrador
        public bool InsertarAdministrador(AdministradorEntidad admin)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Administrador 
                        (Identificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, FechaContratacion)
                        VALUES (@Identificacion, @Nombre, @Apellido1, @Apellido2, @FechaNac, @FechaCont)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Identificacion", (int)admin.Identificacion);
                cmd.Parameters.AddWithValue("@Nombre", admin.Nombre);
                cmd.Parameters.AddWithValue("@Apellido1", admin.PrimerApellido);
                cmd.Parameters.AddWithValue("@Apellido2", admin.SegundoApellido);
                cmd.Parameters.AddWithValue("@FechaNac", admin.FechaNacimiento.Date);
                cmd.Parameters.AddWithValue("@FechaCont", admin.FechaContratacion.Date);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Obtener todos los administradores
        public List<AdministradorEntidad> ObtenerAdministradores()
        {
            List<AdministradorEntidad> admins = new List<AdministradorEntidad>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT * FROM Administrador";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    admins.Add(new AdministradorEntidad
                    {
                        Identificacion = Convert.ToInt32(reader["Identificacion"]),
                        Nombre = reader["Nombre"].ToString(),
                        PrimerApellido = reader["PrimerApellido"].ToString(),
                        SegundoApellido = reader["SegundoApellido"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        FechaContratacion = Convert.ToDateTime(reader["FechaContratacion"])
                    });
                }
            }
            return admins;
        }

        // Verificar si identificación ya existe
        public bool ExisteAdministrador(long identificacion)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT COUNT(1) FROM Administrador WHERE Identificacion = @Id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Id", identificacion);

                conn.Open();
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        // Insertar nueva tienda
        public bool InsertarTienda(TiendaEntidad tienda)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Tienda 
                        (Nombre, Id_Administrador, Direccion, Telefono, Activa)
                        VALUES (@Nombre, @IdAdmin, @Direccion, @Telefono, @Activa)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Nombre", tienda.Nombre);
                cmd.Parameters.AddWithValue("@IdAdmin", tienda.Administrador.Identificacion);
                cmd.Parameters.AddWithValue("@Direccion", tienda.Direccion);
                cmd.Parameters.AddWithValue("@Telefono", tienda.Telefono);
                cmd.Parameters.AddWithValue("@Activa", tienda.Activa);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Obtener todas las tiendas con info de administrador
        public List<TiendaEntidad> ObtenerTiendas()
        {
            List<TiendaEntidad> tiendas = new List<TiendaEntidad>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"SELECT t.Id, t.Nombre, t.Direccion, t.Telefono, t.Activa,
                        a.Identificacion, a.Nombre as AdminNombre, 
                        a.PrimerApellido, a.SegundoApellido
                        FROM Tienda t
                        INNER JOIN Administrador a ON t.Id_Administrador = a.Identificacion";

                SqlCommand cmd = new SqlCommand(query, conn);
                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        tiendas.Add(new TiendaEntidad
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Activa = Convert.ToBoolean(reader["Activa"]),
                            Administrador = new AdministradorEntidad
                            {
                                Identificacion = Convert.ToInt32(reader["Identificacion"]),
                                Nombre = reader["AdminNombre"].ToString(),
                                PrimerApellido = reader["PrimerApellido"].ToString(),
                                SegundoApellido = reader["SegundoApellido"].ToString()
                            }
                        });
                    }
                }
            }
            return tiendas;
        }

        // Obtener administradores para ComboBox
        public List<AdministradorEntidad> ObtenerAdministradoresParaCombo()
        {
            var admins = ObtenerAdministradores();

            // Debug: Verificar datos recuperados
            Console.WriteLine($"Administradores encontrados: {admins.Count}");
            foreach (var admin in admins)
            {
                Console.WriteLine($"{admin.Identificacion}: {admin.NombreCompleto}");
            }

            return admins;
        }
    }
}