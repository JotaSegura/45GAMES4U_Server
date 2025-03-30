/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Entities;
using System.Data;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ServerApp
{
    public class AccesoDatos
    {
        // Dirección IP y puerto del servidor TCP
        private const string IP_SERVIDOR = "127.0.0.1";
        private const int PUERTO_SERVIDOR = 14100;
        // Cadena de conexión a la base de datos SQL Server
        private static string connectionString = "Data Source=JOTA\\SQLEXPRESS;Initial Catalog=GAMES4U2;Integrated Security=True;Encrypt=False;";
        private string _cadenaConexion = "Data Source=JOTA\\SQLEXPRESS;Initial Catalog=GAMES4U2;Integrated Security=True;Encrypt=False;";




        #region Metodos para el servidorTCP
        public static Cliente ObtenerClientePorIdentificacion(long identificacion)
            {
                Cliente cliente = null;

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "SELECT Nombre, PrimerApellido, SegundoApellido FROM Cliente WHERE Identificacion = @Identificacion";
                        using (SqlCommand cmd = new SqlCommand(query, conn))
                        {
                            // Agregar parámetro para la consulta
                            cmd.Parameters.AddWithValue("@Identificacion", identificacion);

                            // Ejecutar la consulta y leer los datos
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    // Si el cliente existe, crear un objeto Cliente con los datos
                                    cliente = new Cliente
                                    {
                                        Identificacion = identificacion,
                                        Nombre = reader["Nombre"].ToString(),
                                        PrimerApellido = reader["PrimerApellido"].ToString(),
                                        SegundoApellido = reader["SegundoApellido"].ToString()
                                    };
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Manejo de excepciones
                        Console.WriteLine("Error al obtener los datos del cliente: " + ex.Message);
                    }
                }

                return cliente; // Si no se encuentra, devuelve null
            }
       

        public static bool VerificarClienteEnServidor(string identificacion)
{
    bool clienteExiste = false;
    
    try
    {
        // Asegúrate de que la conexión a la base de datos está configurada correctamente
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT COUNT(*) FROM Cliente WHERE Identificacion = @Identificacion";
            
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Identificacion", identificacion);
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                
                if (count > 0)
                {
                    clienteExiste = true;
                }
            }
        }
    }
    catch (Exception ex)
    {
        //LogBitacora($"Error al verificar cliente: {ex.Message}");
    }

    return clienteExiste;
}
        #endregion

        #region Metodos usados en Servidor solucion para sus formularios
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

        //Metodo para insertar un videojuego
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
        
        //Metodo para obtener todos los vieojuegos
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

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

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
                catch (Exception ex)
                {
                    // Loggear el error
                    Console.WriteLine($"Error al obtener tiendas: {ex.Message}");
                    throw; // Relanzar la excepción para manejo superior
                }
            }

            return tiendas;
        }

        // Insertar nuevo cliente
        public bool InsertarCliente(ClienteEntidad cliente)
        {
            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = @"INSERT INTO Cliente 
                        (Identificacion, Nombre, PrimerApellido, SegundoApellido, FechaNacimiento, JugadorEnLinea)
                        VALUES (@Identificacion, @Nombre, @Apellido1, @Apellido2, @FechaNac, @JugadorOnline)";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Identificacion", cliente.Identificacion);
                cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                cmd.Parameters.AddWithValue("@Apellido1", cliente.PrimerApellido);
                cmd.Parameters.AddWithValue("@Apellido2", cliente.SegundoApellido);
                cmd.Parameters.AddWithValue("@FechaNac", cliente.FechaNacimiento.Date);
                cmd.Parameters.AddWithValue("@JugadorOnline", cliente.JugadorEnLinea);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        // Obtener todos los clientes
        public List<ClienteEntidad> ObtenerClientes()
        {
            List<ClienteEntidad> clientes = new List<ClienteEntidad>();

            using (SqlConnection conn = new SqlConnection(_cadenaConexion))
            {
                string query = "SELECT * FROM Cliente";
                SqlCommand cmd = new SqlCommand(query, conn);

                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new ClienteEntidad
                    {
                        Identificacion = Convert.ToInt32(reader["Identificacion"]),
                        Nombre = reader["Nombre"].ToString(),
                        PrimerApellido = reader["PrimerApellido"].ToString(),
                        SegundoApellido = reader["SegundoApellido"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                        JugadorEnLinea = Convert.ToBoolean(reader["JugadorEnLinea"])
                    });
                }
            }
            return clientes;
        }

        //Obtener todas las tiendas activas
        public List<TiendaEntidad> ObtenerTiendasActivas()
        {
            var tiendas = new List<TiendaEntidad>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                var query = @"SELECT t.Id, t.Nombre, t.Direccion, t.Telefono, 
                            a.Identificacion, a.Nombre AS AdminNombre,
                            a.PrimerApellido, a.SegundoApellido
                            FROM Tienda t
                            INNER JOIN Administrador a ON t.Id_Administrador = a.Identificacion
                            WHERE t.Activa = 1
                            ORDER BY t.Nombre";

                var cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tiendas.Add(new TiendaEntidad
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Direccion = reader["Direccion"].ToString(),
                                Telefono = reader["Telefono"].ToString(),
                                Activa = true,
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
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener tiendas activas", ex);
                }
            }

            return tiendas;
        }

        //Metodo para obtener todos los videojuegos fisicos
        public List<VideojuegoEntidad> ObtenerVideojuegosFisicos()
        {
            var videojuegos = new List<VideojuegoEntidad>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                var query = @"SELECT v.Id, v.Nombre, v.Desarrollador, v.Lanzamiento,
                            t.Id AS TipoId, t.Nombre AS TipoNombre
                            FROM Videojuego v
                            INNER JOIN TipoVideojuego t ON v.Id_TipoVideojuego = t.Id
                            WHERE v.Fisico = 1
                            ORDER BY v.Nombre";

                var cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            videojuegos.Add(new VideojuegoEntidad
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Nombre = reader["Nombre"].ToString(),
                                Desarrollador = reader["Desarrollador"].ToString(),
                                Lanzamiento = Convert.ToInt32(reader["Lanzamiento"]),
                                Fisico = true,
                                TipoVideojuego = new TipoVideojuegoEntidad
                                {
                                    Id = Convert.ToInt32(reader["TipoId"]),
                                    Nombre = reader["TipoNombre"].ToString()
                                }
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener videojuegos físicos", ex);
                }
            }

            return videojuegos;
        }

        //Metodo para asocias los videojuegos fisicos con las diferentes tiendas
        public bool AsociarVideojuegoTienda(int idTienda, int idVideojuego, int existencias)
        {
            using (var conn = new SqlConnection(_cadenaConexion))
            {
                var query = @"IF EXISTS (SELECT 1 FROM VideojuegosXTienda 
                            WHERE Id_Tienda = @IdTienda AND Id_Videojuego = @IdVideojuego)
                            BEGIN
                                UPDATE VideojuegosXTienda 
                                SET Existencias = Existencias + @Existencias
                                WHERE Id_Tienda = @IdTienda AND Id_Videojuego = @IdVideojuego
                            END
                            ELSE
                            BEGIN
                                INSERT INTO VideojuegosXTienda 
                                (Id_Tienda, Id_Videojuego, Existencias)
                                VALUES (@IdTienda, @IdVideojuego, @Existencias)
                            END";

                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@IdTienda", idTienda);
                cmd.Parameters.AddWithValue("@IdVideojuego", idVideojuego);
                cmd.Parameters.AddWithValue("@Existencias", existencias);

                try
                {
                    conn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al asociar videojuego a tienda", ex);
                }
            }
        }

        //Metodo para Obtener todo el inventario
        public List<VideojuegosXTiendaEntidad> ObtenerInventarioCompleto()
        {
            var inventario = new List<VideojuegosXTiendaEntidad>();

            using (var conn = new SqlConnection(_cadenaConexion))
            {
                var query = @"SELECT t.Id AS TiendaId, t.Nombre AS TiendaNombre, t.Direccion,
                            v.Id AS VideojuegoId, v.Nombre AS VideojuegoNombre,
                            tv.Nombre AS TipoNombre, vxt.Existencias
                            FROM VideojuegosXTienda vxt
                            INNER JOIN Tienda t ON vxt.Id_Tienda = t.Id
                            INNER JOIN Videojuego v ON vxt.Id_Videojuego = v.Id
                            INNER JOIN TipoVideojuego tv ON v.Id_TipoVideojuego = tv.Id
                            ORDER BY t.Nombre, v.Nombre";

                var cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            inventario.Add(new VideojuegosXTiendaEntidad
                            {
                                Tienda = new TiendaEntidad
                                {
                                    Id = Convert.ToInt32(reader["TiendaId"]),
                                    Nombre = reader["TiendaNombre"].ToString(),
                                    Direccion = reader["Direccion"].ToString()
                                },
                                Videojuego = new VideojuegoEntidad
                                {
                                    Id = Convert.ToInt32(reader["VideojuegoId"]),
                                    Nombre = reader["VideojuegoNombre"].ToString(),
                                    TipoVideojuego = new TipoVideojuegoEntidad
                                    {
                                        Nombre = reader["TipoNombre"].ToString()
                                    }
                                },
                                Existencias = Convert.ToInt32(reader["Existencias"])
                            });
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al obtener inventario completo", ex);
                }
            }

            return inventario;
        }

        #endregion
    }

}