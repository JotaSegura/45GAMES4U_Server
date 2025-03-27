using ServerApp.Forms;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
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

        private void btnInventario_Click(object sender, EventArgs e)
        {
            var form = new FormInventario();
            form.ShowDialog();
        }

        private void btnServidor_Click(object sender, EventArgs e)
        {
            var form = new frmPrincipalServidor();
            form.ShowDialog();
        }
    }
}