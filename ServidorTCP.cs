using System.Text.Json;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using Entities;
using Servidor45GAMES4U;

namespace ServerApp
{
    public class ServidorTCP
    {
        private TcpListener _listener;
        private readonly int _puerto = 14100;
        private readonly IPAddress _ip = IPAddress.Parse("127.0.0.1");
        private bool _estaEjecutando = false;
        private readonly object _lockClientes = new object();
        private List<ClienteConectado> _clientesConectados = new List<ClienteConectado>();
        private const int MAX_CONEXIONES = 5;
        private readonly ServicioReservas _servicioReservas;
        private readonly ServicioClientes _servicioClientes;
 

        public event Action<string> OnMensajeBitacora;
        public event Action<int> OnConexionesActualizadas;

        public ServidorTCP(string cadenaConexion)
        {
            _servicioReservas = new ServicioReservas(cadenaConexion);
            _servicioClientes = new ServicioClientes(cadenaConexion);
        }

        public void Iniciar()
        {
            try
            {
                _listener = new TcpListener(_ip, _puerto);
                _listener.Start();
                _estaEjecutando = true;

                Thread hiloAceptador = new Thread(AceptarConexiones);
                hiloAceptador.IsBackground = true;
                hiloAceptador.Start();

                OnMensajeBitacora?.Invoke($"Servidor iniciado en {_ip}:{_puerto}");
                OnMensajeBitacora?.Invoke($"Esperando conexiones... (Máximo {MAX_CONEXIONES} simultáneas)");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al iniciar servidor: {ex.Message}");
            }
        }

        private void AceptarConexiones()
        {
            while (_estaEjecutando)
            {
                try
                {
                    if (_clientesConectados.Count < MAX_CONEXIONES)
                    {
                        TcpClient cliente = _listener.AcceptTcpClient();
                        ClienteConectado clienteConectado = new ClienteConectado(cliente);

                        lock (_lockClientes)
                        {
                            _clientesConectados.Add(clienteConectado);
                            OnConexionesActualizadas?.Invoke(_clientesConectados.Count);
                        }

                        clienteConectado.OnMensajeRecibido += ManejarMensaje;
                        clienteConectado.OnDesconectado += ManejarDesconexion;

                        Thread hiloCliente = new Thread(clienteConectado.Procesar);
                        hiloCliente.IsBackground = true;
                        hiloCliente.Start();

                        OnMensajeBitacora?.Invoke($"Cliente conectado: {cliente.Client.RemoteEndPoint}");
                        OnMensajeBitacora?.Invoke($"Clientes conectados: {_clientesConectados.Count}/{MAX_CONEXIONES}");
                    }
                    else
                    {
                        Thread.Sleep(1000); // Esperar antes de intentar aceptar otra conexión
                    }
                }
                catch (SocketException) when (!_estaEjecutando)
                {
                    // Servidor detenido normalmente
                }
                catch (Exception ex)
                {
                    OnMensajeBitacora?.Invoke($"Error al aceptar conexión: {ex.Message}");
                }
            }
        }

        private void ManejarMensaje(ClienteConectado cliente, string mensaje)
        {
            try
            {
                string[] partes = mensaje.Split('|');
                string comando = partes[0].ToUpper();
                string respuesta;

                switch (comando)
                {
                    case "VALIDAR_CLIENTE":
                        if (partes.Length == 2 && int.TryParse(partes[1], out int idCliente))
                        {
                            var clienteValidado = _servicioClientes.ValidarCliente(idCliente);

                            if (clienteValidado != null)
                            {
                                respuesta = $"RESPUESTA_VALIDACION|OK|{clienteValidado.Nombre}|{clienteValidado.PrimerApellido}|{clienteValidado.SegundoApellido}";
                            }
                            else
                            {
                                respuesta = "RESPUESTA_VALIDACION|ERROR|Cliente no encontrado";
                            }
                        }
                        else
                        {
                            respuesta = "ERROR|Formato de validación incorrecto";
                        }
                        break;

                    case "CONSULTAR_RESERVAS":
                        if (partes.Length == 2 && int.TryParse(partes[1], out int idClienteConsulta))
                        {
                            var reservas = _servicioClientes.ObtenerReservasCliente(idClienteConsulta);
                            respuesta = $"LISTA_RESERVAS|{JsonSerializer.Serialize(reservas)}";
                        }
                        else
                        {
                            respuesta = "ERROR|ID de cliente inválido";
                        }
                        break;

                    // ... (otros casos existentes)

                    default:
                        respuesta = "ERROR|Comando no reconocido";
                        break;
                }

                cliente.EnviarMensaje(respuesta);
                OnMensajeBitacora?.Invoke($"Respuesta a {cliente.IdCliente}: {respuesta.Split('|')[0]}");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al procesar mensaje: {ex.Message}");
                cliente.EnviarMensaje($"ERROR|Excepción: {ex.Message}");
            }
        }

        private void ManejarDesconexion(ClienteConectado cliente)
        {
            lock (_lockClientes)
            {
                if (_clientesConectados.Remove(cliente))
                {
                    OnConexionesActualizadas?.Invoke(_clientesConectados.Count);
                    OnMensajeBitacora?.Invoke($"Cliente desconectado: {cliente.IdCliente}");
                    OnMensajeBitacora?.Invoke($"Clientes conectados: {_clientesConectados.Count}/{MAX_CONEXIONES}");
                }
            }
        }

        public void Detener()
        {
            try
            {
                _estaEjecutando = false;
                _listener.Stop();

                lock (_lockClientes)
                {
                    foreach (var cliente in _clientesConectados.ToList())
                    {
                        cliente.Desconectar();
                    }
                    _clientesConectados.Clear();
                    OnConexionesActualizadas?.Invoke(0);
                }

                OnMensajeBitacora?.Invoke("Servidor detenido correctamente");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al detener servidor: {ex.Message}");
            }
        }
    }

    public class ClienteConectado
    {
        private readonly TcpClient _cliente;
        private NetworkStream _stream;
        public string IdCliente { get; private set; }

        public event Action<ClienteConectado, string> OnMensajeRecibido;
        public event Action<ClienteConectado> OnDesconectado;

        public ClienteConectado(TcpClient cliente)
        {
            _cliente = cliente;
            _stream = cliente.GetStream();
            IdCliente = cliente.Client.RemoteEndPoint.ToString();
        }

        public void Procesar()
        {
            try
            {
                byte[] buffer = new byte[4096];
                int bytesLeidos;

                while ((bytesLeidos = _stream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    string mensaje = System.Text.Encoding.UTF8.GetString(buffer, 0, bytesLeidos);
                    OnMensajeRecibido?.Invoke(this, mensaje);
                }
            }
            catch (Exception)
            {
                // Cliente desconectado
            }
            finally
            {
                OnDesconectado?.Invoke(this);
                Desconectar();
            }
        }



        public void EnviarMensaje(string mensaje)
        {
            try
            {
                byte[] buffer = System.Text.Encoding.UTF8.GetBytes(mensaje);
                _stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception)
            {
                // Error al enviar mensaje
            }
        }

        public void Desconectar()
        {
            try
            {
                _stream?.Close();
                _cliente?.Close();
            }
            catch (Exception)
            {
                // Error al desconectar
            }
        }
    }
}