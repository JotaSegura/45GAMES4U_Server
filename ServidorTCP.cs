using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ServerApp
{
    public class ServidorTCP
    {
        private TcpListener _servidor;
        private List<TcpClient> _clientes = new List<TcpClient>();
        private bool _ejecutando = false;
        private readonly object _lockClientes = new object();
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public event Action<string> OnMensajeBitacora;

        public void Iniciar()
        {
            try
            {
                _servidor = new TcpListener(IPAddress.Any, 14100);
                _servidor.Start();
                _ejecutando = true;

                var endPoint = (IPEndPoint)_servidor.LocalEndpoint;
                OnMensajeBitacora?.Invoke($"Servidor iniciado en {endPoint.Address}:{endPoint.Port}");

                Thread hiloEscucha = new Thread(EscucharConexiones);
                hiloEscucha.IsBackground = true;
                hiloEscucha.Start();
            }
            catch (SocketException ex)
            {
                OnMensajeBitacora?.Invoke($"Error de socket: {ex.ErrorCode} - {ex.Message}");
                if (ex.ErrorCode == 10048)
                {
                    OnMensajeBitacora?.Invoke("El puerto 14100 está siendo usado por otra aplicación");
                }
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error crítico al iniciar: {ex.GetType().Name} - {ex.Message}");
            }
        }

        private void EscucharConexiones()
        {
            while (_ejecutando)
            {
                try
                {
                    if (_servidor.Pending() || !_servidor.Server.IsBound)
                    {
                        Thread.Sleep(100);
                        continue;
                    }

                    TcpClient cliente = _servidor.AcceptTcpClient();
                    lock (_lockClientes)
                    {
                        if (_clientes.Count < 5) // Límite de 5 conexiones
                        {
                            _clientes.Add(cliente);
                            OnMensajeBitacora?.Invoke($"Cliente conectado ({cliente.Client.RemoteEndPoint}). Total: {_clientes.Count}");

                            Thread hiloCliente = new Thread(() => ManejarCliente(cliente));
                            hiloCliente.IsBackground = true;
                            hiloCliente.Start();
                        }
                        else
                        {
                            RechazarConexion(cliente);
                        }
                    }
                }
                catch (InvalidOperationException ex)
                {
                    OnMensajeBitacora?.Invoke($"Error de operación: {ex.Message}");
                }
                catch (SocketException ex) when (ex.ErrorCode == 10004)
                {
                    // Error normal cuando se detiene el servidor
                }
                catch (Exception ex)
                {
                    OnMensajeBitacora?.Invoke($"Error en escucha: {ex.GetType().Name} - {ex.Message}");
                    Thread.Sleep(1000);
                }
            }
        }

        private void RechazarConexion(TcpClient cliente)
        {
            try
            {
                var stream = cliente.GetStream();
                byte[] rechazo = Encoding.ASCII.GetBytes("ERROR|Servidor ocupado. Intente más tarde");
                stream.Write(rechazo, 0, rechazo.Length);
                cliente.Close();
                OnMensajeBitacora?.Invoke("Conexión rechazada (límite alcanzado)");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al rechazar conexión: {ex.Message}");
            }
        }

        private void ManejarCliente(TcpClient cliente)
        {
            NetworkStream stream = null;
            try
            {
                stream = cliente.GetStream();
                byte[] buffer = new byte[1024];

                while (_ejecutando && cliente.Connected)
                {
                    int bytesLeidos = stream.Read(buffer, 0, buffer.Length);
                    if (bytesLeidos == 0) break;

                    string mensaje = Encoding.ASCII.GetString(buffer, 0, bytesLeidos);
                    OnMensajeBitacora?.Invoke($"Mensaje de {cliente.Client.RemoteEndPoint}: {mensaje}");

                    string respuesta = ProcesarMensaje(mensaje);
                    byte[] datosRespuesta = Encoding.ASCII.GetBytes(respuesta);
                    stream.Write(datosRespuesta, 0, datosRespuesta.Length);
                }
            }
            catch (IOException ex)
            {
                OnMensajeBitacora?.Invoke($"Cliente desconectado abruptamente: {ex.Message}");
            }
            catch (SocketException ex)
            {
                OnMensajeBitacora?.Invoke($"Error de conexión con cliente: {ex.SocketErrorCode}");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al manejar cliente: {ex.GetType().Name} - {ex.Message}");
            }
            finally
            {
                try
                {
                    stream?.Close();
                    lock (_lockClientes)
                    {
                        _clientes.Remove(cliente);
                        OnMensajeBitacora?.Invoke($"Cliente desconectado. Total: {_clientes.Count}");
                    }
                    cliente.Close();
                }
                catch (Exception ex)
                {
                    OnMensajeBitacora?.Invoke($"Error al limpiar recursos: {ex.Message}");
                }
            }
        }

        private string ProcesarMensaje(string mensaje)
        {
            try
            {
                string[] partes = mensaje.Split('|');
                if (partes.Length == 0) return "ERROR|Formato inválido";

                switch (partes[0].ToUpper())
                {
                    case "VALIDAR_CLIENTE":
                        if (partes.Length < 2 || !int.TryParse(partes[1], out int identificacion))
                            return "ERROR|Identificación inválida";

                        var cliente = _accesoDatos.ValidarCliente(identificacion);
                        return cliente != null
                            ? $"CLIENTE_VALIDO|{cliente.Nombre}|{cliente.PrimerApellido}|{cliente.SegundoApellido}"
                            : "ERROR|Cliente no encontrado";

                    // Agregar más comandos aquí...

                    default:
                        return "ERROR|Comando no reconocido";
                }
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al procesar mensaje: {ex.Message}");
                return "ERROR|Error interno del servidor";
            }
        }

        public void Detener()
        {
            try
            {
                _ejecutando = false;

                lock (_lockClientes)
                {
                    foreach (var cliente in _clientes)
                    {
                        try
                        {
                            cliente.Close();
                        }
                        catch { /* Ignorar errores de cierre */ }
                    }
                    _clientes.Clear();
                }

                _servidor?.Stop();
                OnMensajeBitacora?.Invoke("Servidor detenido correctamente");
            }
            catch (Exception ex)
            {
                OnMensajeBitacora?.Invoke($"Error al detener servidor: {ex.Message}");
            }
        }

        public int ObtenerCantidadClientes()
        {
            lock (_lockClientes)
            {
                return _clientes.Count;
            }
        }
    }
}