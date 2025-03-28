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
        private const string IP_SERVIDOR = "127.0.0.1";
        private const int PUERTO_SERVIDOR = 14100;
        private TcpListener _servidor;
        private Thread _hiloServidor;

        public ServidorForm()
        {
            InitializeComponent();
        }

        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            // Iniciar servidor en un hilo separado para no bloquear la interfaz
            _hiloServidor = new Thread(IniciarServidor);
            _hiloServidor.Start();
            btnIniciarServidor.Enabled = false;
            btnDetenerServidor.Enabled = true;
            LogBitacora("Servidor iniciado.");
        }

        private void btnDetenerServidor_Click(object sender, EventArgs e)
        {
            // Detener servidor
            _servidor?.Stop();
            _hiloServidor?.Abort();
            btnIniciarServidor.Enabled = true;
            btnDetenerServidor.Enabled = false;
            LogBitacora("Servidor detenido.");
        }

        private void IniciarServidor()
        {
            _servidor = new TcpListener(IPAddress.Parse(IP_SERVIDOR), PUERTO_SERVIDOR);
            _servidor.Start();
            LogBitacora("Servidor escuchando en el puerto 14100...");

            while (true)
            {
                try
                {
                    TcpClient cliente = _servidor.AcceptTcpClient();
                    ThreadPool.QueueUserWorkItem(AtenderCliente, cliente);
                }
                catch (Exception ex)
                {
                    LogBitacora($"Error al aceptar la conexión: {ex.Message}");
                    break;
                }
            }
        }

        private void AtenderCliente(object obj)
        {
            TcpClient cliente = (TcpClient)obj;
            NetworkStream stream = cliente.GetStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8) { AutoFlush = true };

            try
            {
                string mensaje = reader.ReadLine();
                if (mensaje.StartsWith("VALIDAR_CLIENTE"))
                {
                    string[] parts = mensaje.Split('|');
                    string identificacion = parts[1];  // Ahora la identificación es un string

                    bool clienteExiste = AccesoDatos.VerificarClienteEnServidor(identificacion);  // Pasa el string

                    if (clienteExiste)
                    {
                        var clienteDatos = AccesoDatos.ObtenerClientePorIdentificacion(identificacion);
                        writer.WriteLine("CLIENTE_EXISTE");
                        LogBitacora($"Cliente con identificación {identificacion} verificado.");
                    }
                    else
                    {
                        writer.WriteLine("NO_EXISTE");
                        LogBitacora($"Cliente con identificación {identificacion} no encontrado.");
                    }
                }
            }
            catch (Exception ex)
            {
                LogBitacora($"Error al procesar solicitud del cliente: {ex.Message}");
            }
            finally
            {
                cliente.Close();
            }
        }


        private void LogBitacora(string mensaje)
        {
            if (txtBitacora.IsHandleCreated)
            {
                Invoke(new Action(() =>
                {
                    txtBitacora.AppendText($"[{DateTime.Now}] {mensaje}\r\n");
                }));
            }
            else
            {
                
            }
        }
    }
}

