using System;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class Form1 : Form
    {
        private ServidorTCP _servidor;

        public Form1()
        {
            InitializeComponent();
            btnIniciarServidor.Click += new EventHandler(btnIniciarServidor_Click);//Asociar el evento click al boton guardar
            _servidor = new ServidorTCP();
            _servidor.OnMensajeBitacora += ActualizarBitacora;
        }

        private void btnIniciarServidor_Click(object sender, EventArgs e)
        {
            Thread hiloServidor = new Thread(_servidor.Iniciar);
            hiloServidor.Start();
            btnIniciarServidor.Enabled = false;
        }

        private void ActualizarBitacora(string mensaje)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string>(ActualizarBitacora), mensaje);
                return;
            }

            txtBitacora.AppendText($"{DateTime.Now}: {mensaje}{Environment.NewLine}");
            lblClientesConectados.Text = $"Clientes conectados: {_servidor.ObtenerCantidadClientes()}";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _servidor.Detener();
            base.OnFormClosing(e);
        }

        private void btnTiposVideojuegos_Click(object sender, EventArgs e)
        {
            var form = new TipoVideojuegoForm();
            form.ShowDialog(); // Muestra como diálogo modal
        }

        private void btnVideojuego_Click(object sender, EventArgs e)
        {
            var form = new VideojuegoForm();
            form.ShowDialog();
        }

        private void btnAdministradores_Click(object sender, EventArgs e)
        {
            var form = new AdministradorForm();
            form.ShowDialog();
        }

        private void btnTiendas_Click(object sender, EventArgs e)
        {
            var form = new TiendaForm();
            form.ShowDialog();
        }

        private void btnClientes_Click(object sender, EventArgs e)
        {
            var form = new ClienteForm();
            form.ShowDialog();
        }
    }
}