/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using Entities;
using ServerApp;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class ServidorForm : Form
    {
        // Dirección IP del servidor
        private const string IP_SERVIDOR = "127.0.0.1";
        // Puerto en el que el servidor escuchará las conexiones
        private const int PUERTO_SERVIDOR = 14100;
        // Objeto TcpListener para escuchar las conexiones entrantes
        private TcpListener _servidor;
        // Hilo para ejecutar el servidor sin bloquear la interfaz de usuario
        private Thread _hiloServidor;

        public ServidorForm()
        {
            InitializeComponent();
        }

        // Evento que se ejecuta al hacer clic en el botón "Iniciar Servidor"
        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            // Iniciar servidor en un hilo separado para no bloquear la interfaz
            _hiloServidor = new Thread(IniciarServidor);
            _hiloServidor.Start();
            // Deshabilitar el botón de iniciar y habilitar el de detener
            btnIniciarServidor.Enabled = false;
            btnDetenerServidor.Enabled = true;
            // Registrar en la bitácora que el servidor ha sido iniciado
            LogBitacora("Servidor iniciado.");
        }

        // Evento que se ejecuta al hacer clic en el botón "Detener Servidor"
        private void btnDetenerServidor_Click(object sender, EventArgs e)
        {
            // Detener servidor
            _servidor?.Stop();
            // Abortamos el hilo del servidor
            _hiloServidor?.Abort();
            // Habilitar el botón de iniciar y deshabilitar el de detener
            btnIniciarServidor.Enabled = true;
            btnDetenerServidor.Enabled = false;
            // Registrar en la bitácora que el servidor ha sido detenido
            LogBitacora("Servidor detenido.");
        }

        // Método que inicia el servidor y comienza a escuchar conexiones entrantes
        private void IniciarServidor()
        {
            // Crear un TcpListener para escuchar en la IP y puerto especificados
            _servidor = new TcpListener(IPAddress.Parse(IP_SERVIDOR), PUERTO_SERVIDOR);
            _servidor.Start();
            // Registrar en la bitácora que el servidor está escuchando
            LogBitacora("Servidor escuchando en el puerto 14100...");

            while (true)
            {
                try
                {
                    // Aceptar una conexión entrante
                    TcpClient cliente = _servidor.AcceptTcpClient();
                    // Procesar la conexión en un hilo del pool de hilos
                    ThreadPool.QueueUserWorkItem(AtenderCliente, cliente);
                }
                catch (Exception ex)
                {
                    // Registrar en la bitácora cualquier error al aceptar la conexión
                    LogBitacora($"Error al aceptar la conexión: {ex.Message}");
                    break;
                }
            }
        }

        // Método que atiende a un cliente conectado
        private void AtenderCliente(object obj)
        {
            TcpClient cliente = (TcpClient)obj;
            NetworkStream stream = cliente.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            try
            {
                // Leer el mensaje enviado por el cliente
                string mensaje = reader.ReadLine();
                if (mensaje.StartsWith("VALIDAR_CLIENTE"))
                {
                    // Extraer la identificación del cliente del mensaje
                    string[] parts = mensaje.Split('|');
                    long identificacion = long.Parse(parts[1]);

                    // Obtener los datos del cliente desde la base de datos
                    Cliente clienteData = AccesoDatos.ObtenerClientePorIdentificacion(identificacion);

                    if (clienteData != null)
                    {
                        // Enviar los datos del cliente al cliente conectado
                        writer.WriteLine($"CLIENTE_EXISTE|{clienteData.Nombre}|{clienteData.PrimerApellido}|{clienteData.SegundoApellido}");
                        // Registrar en la bitácora que el cliente ha sido verificado
                        LogBitacora($"Cliente con identificación {identificacion} verificado.");
                    }
                    else
                    {
                        // Informar al cliente que no existe en la base de datos
                        writer.WriteLine("NO_EXISTE");
                        // Registrar en la bitácora que el cliente no fue encontrado
                        LogBitacora($"Cliente con identificación {identificacion} no encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Registrar en la bitácora cualquier error al procesar la solicitud del cliente
                LogBitacora($"Error al procesar solicitud del cliente: {ex.Message}");
            }
            finally
            {
                // Cerrar la conexión con el cliente
                cliente.Close();
            }
        }

        // Método para registrar mensajes en la bitácora de la interfaz de usuario
        private void LogBitacora(string mensaje)
        {
            if (txtBitacora.IsHandleCreated)
            {
                Invoke(new Action(() =>
                {
                    // Agregar el mensaje a la bitácora con la fecha y hora actuales
                    txtBitacora.AppendText($"[{DateTime.Now}] {mensaje}\r\n");
                }));
            }
        }
    }
}

