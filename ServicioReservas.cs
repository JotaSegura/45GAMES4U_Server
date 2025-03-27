// ServicioReservas.cs
using Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading;

namespace Servidor45GAMES4U
{
    public class ServicioReservas
    {
        private readonly string _cadenaConexion;
        private readonly object _lockReservas = new object();

        public ServicioReservas(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public ResultadoReserva ProcesarReserva(ReservaEntidad reserva)
        {
            // Bloqueo para manejar concurrencia
            lock (_lockReservas)
            {
                using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
                {
                    conexion.Open();

                    // 1. Verificar existencias
                    int existencias = ObtenerExistencias(conexion, reserva.IdTienda, reserva.IdVideojuego);

                    if (existencias < reserva.Cantidad)
                    {
                        return new ResultadoReserva
                        {
                            Exito = false,
                            Mensaje = "No hay suficientes existencias para realizar la reserva.",
                            ExistenciasDisponibles = existencias
                        };
                    }

                    // 2. Crear transacción
                    using (SqlTransaction transaccion = conexion.BeginTransaction(IsolationLevel.Serializable))
                    {
                        try
                        {
                            // 3. Insertar reserva
                            int idReserva = InsertarReserva(conexion, transaccion, reserva);

                            // 4. Actualizar existencias
                            ActualizarExistencias(conexion, transaccion, reserva.IdTienda, reserva.IdVideojuego, reserva.Cantidad);

                            // 5. Confirmar transacción
                            transaccion.Commit();

                            return new ResultadoReserva
                            {
                                Exito = true,
                                Mensaje = "Reserva realizada con éxito.",
                                IdReserva = idReserva,
                                ExistenciasDisponibles = existencias - reserva.Cantidad
                            };
                        }
                        catch (Exception ex)
                        {
                            transaccion.Rollback();
                            return new ResultadoReserva
                            {
                                Exito = false,
                                Mensaje = $"Error al procesar reserva: {ex.Message}"
                            };
                        }
                    }
                }
            }
        }

        private int ObtenerExistencias(SqlConnection conexion, int idTienda, int idVideojuego)
        {
            string consulta = "SELECT Existencias FROM VideojuegosXTienda WHERE Id_Tienda = @IdTienda AND Id_Videojuego = @IdVideojuego";

            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.AddWithValue("@IdTienda", idTienda);
                comando.Parameters.AddWithValue("@IdVideojuego", idVideojuego);

                object resultado = comando.ExecuteScalar();
                return resultado != null ? Convert.ToInt32(resultado) : 0;
            }
        }

        private int InsertarReserva(SqlConnection conexion, SqlTransaction transaccion, ReservaEntidad reserva)
        {
            string consulta = @"INSERT INTO Reserva (Id_Tienda, Id_Videojuego, Id_Cliente, FechaReserva, Cantidad)
                               VALUES (@IdTienda, @IdVideojuego, @IdCliente, @FechaReserva, @Cantidad);
                               SELECT SCOPE_IDENTITY();";

            using (SqlCommand comando = new SqlCommand(consulta, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdTienda", reserva.IdTienda);
                comando.Parameters.AddWithValue("@IdVideojuego", reserva.IdVideojuego);
                comando.Parameters.AddWithValue("@IdCliente", reserva.IdCliente);
                comando.Parameters.AddWithValue("@FechaReserva", reserva.FechaReserva);
                comando.Parameters.AddWithValue("@Cantidad", reserva.Cantidad);

                // Obtener el ID generado
                decimal idGenerado = (decimal)comando.ExecuteScalar();
                return (int)idGenerado;
            }
        }

        private void ActualizarExistencias(SqlConnection conexion, SqlTransaction transaccion, int idTienda, int idVideojuego, int cantidad)
        {
            string consulta = @"UPDATE VideojuegosXTienda 
                               SET Existencias = Existencias - @Cantidad
                               WHERE Id_Tienda = @IdTienda AND Id_Videojuego = @IdVideojuego
                               AND Existencias >= @Cantidad"; // Condición adicional para seguridad

            using (SqlCommand comando = new SqlCommand(consulta, conexion, transaccion))
            {
                comando.Parameters.AddWithValue("@IdTienda", idTienda);
                comando.Parameters.AddWithValue("@IdVideojuego", idVideojuego);
                comando.Parameters.AddWithValue("@Cantidad", cantidad);

                int filasAfectadas = comando.ExecuteNonQuery();

                if (filasAfectadas == 0)
                {
                    throw new Exception("No se pudo actualizar las existencias. Verifique la disponibilidad.");
                }
            }
        }

        public List<ReservaEntidad> ObtenerReservasCliente(int idCliente)
        {
            var reservas = new List<ReservaEntidad>();

            string consulta = @"SELECT r.Id, t.Nombre AS Tienda, v.Nombre AS Videojuego, 
                                      r.FechaReserva, r.Cantidad
                               FROM Reserva r
                               JOIN Tienda t ON r.Id_Tienda = t.Id
                               JOIN Videojuego v ON r.Id_Videojuego = v.Id
                               WHERE r.Id_Cliente = @IdCliente
                               ORDER BY r.FechaReserva DESC";

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            using (SqlCommand comando = new SqlCommand(consulta, conexion))
            {
                comando.Parameters.AddWithValue("@IdCliente", idCliente);
                conexion.Open();

                using (SqlDataReader reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservas.Add(new ReservaEntidad
                        {
                            Id = reader.GetInt32(0),
                            Tienda = new TiendaEntidad { Nombre = reader.GetString(1) },
                            Videojuego = new VideojuegoEntidad { Nombre = reader.GetString(2) },
                            FechaReserva = reader.GetDateTime(3),
                            Cantidad = reader.GetInt32(4)
                        });
                    }
                }
            }

            return reservas;
        }
    }

    public class ResultadoReserva
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; }
        public int IdReserva { get; set; }
        public int ExistenciasDisponibles { get; set; }
    }
}