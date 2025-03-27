// ServicioClientes.cs
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Entities;

namespace Servidor45GAMES4U
{
    public class ServicioClientes
    {
        private readonly string _cadenaConexion;

        public ServicioClientes(string cadenaConexion)
        {
            _cadenaConexion = cadenaConexion;
        }

        public ClienteEntidad ValidarCliente(int identificacion)
        {
            ClienteEntidad cliente = null;

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                string consulta = @"SELECT Identificacion, Nombre, PrimerApellido, SegundoApellido 
                                   FROM Cliente 
                                   WHERE Identificacion = @Identificacion";

                using (SqlCommand comando = new SqlCommand(consulta, conexion))
                {
                    comando.Parameters.AddWithValue("@Identificacion", identificacion);
                    conexion.Open();

                    using (SqlDataReader reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            cliente = new ClienteEntidad
                            {
                                Identificacion = reader.GetInt32(0),
                                Nombre = reader.GetString(1),
                                PrimerApellido = reader.GetString(2),
                                SegundoApellido = reader.GetString(3)
                            };
                        }
                    }
                }
            }

            return cliente;
        }

        public List<ReservaEntidad> ObtenerReservasCliente(int idCliente)
        {
            var reservas = new List<ReservaEntidad>();

            using (SqlConnection conexion = new SqlConnection(_cadenaConexion))
            {
                string consulta = @"SELECT r.Id, t.Nombre AS Tienda, v.Nombre AS Videojuego, 
                                           r.FechaReserva, r.Cantidad
                                    FROM Reserva r
                                    JOIN Tienda t ON r.Id_Tienda = t.Id
                                    JOIN Videojuego v ON r.Id_Videojuego = v.Id
                                    WHERE r.Id_Cliente = @IdCliente
                                    ORDER BY r.FechaReserva DESC";

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
            }

            return reservas;
        }
    }
}