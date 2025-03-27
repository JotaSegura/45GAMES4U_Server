// frmPrincipalServidor.cs
using System;
using System.Windows.Forms;
using System.Configuration;


namespace ServerApp
{
    public partial class frmPrincipalServidor : Form
    {
        private readonly ServidorTCP _servidor;

        public frmPrincipalServidor()
        {
            InitializeComponent();
            string cadenaConexion = ConfigurationManager.ConnectionStrings["Games4UConnection"].ConnectionString;
            _servidor = new ServidorTCP(cadenaConexion);

            _servidor.OnMensajeBitacora += ActualizarBitacora;
            _servidor.OnConexionesActualizadas += ActualizarContadorConexiones;
        }

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            btnIniciar.Enabled = false;
            btnDetener.Enabled = true;
            _servidor.Iniciar();
        }

        private void btnDetener_Click(object sender, EventArgs e)
        {
            btnIniciar.Enabled = true;
            btnDetener.Enabled = false;
            _servidor.Detener();
        }

        private void ActualizarBitacora(string mensaje)
        {
            if (txtBitacora.InvokeRequired)
            {
                txtBitacora.Invoke(new Action<string>(ActualizarBitacora), mensaje);
                return;
            }

            txtBitacora.AppendText($"[{DateTime.Now:HH:mm:ss}] {mensaje}{Environment.NewLine}");
            txtBitacora.ScrollToCaret();
        }

        private void ActualizarContadorConexiones(int cantidad)
        {
            if (lblClientesConectados.InvokeRequired)
            {
                lblClientesConectados.Invoke(new Action<int>(ActualizarContadorConexiones), cantidad);
                return;
            }

            lblClientesConectados.Text = $"Clientes conectados: {cantidad}";
        }

        private void frmPrincipalServidor_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_servidor != null)
            {
                _servidor.Detener();
            }
        }

        private void btnLimpiarBitacora_Click(object sender, EventArgs e)
        {
            txtBitacora.Clear();
        }
    }
}