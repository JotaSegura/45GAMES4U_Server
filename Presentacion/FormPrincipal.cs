/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using ServerApp.Forms;
using System;
using System.Threading;
using System.Windows.Forms;

namespace ServerApp
{
    public partial class FormPrincipal : Form
    {
        public FormPrincipal()
        {
            InitializeComponent();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
        }

        // Evento para el botón de Tipos de Videojuegos
        private void btnTiposVideojuegos_Click(object sender, EventArgs e)
        {
            var form = new TipoVideojuegoForm();
            form.ShowDialog(); // Muestra como diálogo modal
        }

        // Evento para el botón de Videojuegos
        private void btnVideojuego_Click(object sender, EventArgs e)
        {
            var form = new VideojuegoForm();
            form.ShowDialog();
        }

        // Evento para el botón de Administradores
        private void btnAdministradores_Click(object sender, EventArgs e)
        {
            var form = new AdministradorForm();
            form.ShowDialog();
        }

        // Evento para el botón de Tiendas
        private void btnTiendas_Click(object sender, EventArgs e)
        {
            var form = new TiendaForm();
            form.ShowDialog();
        }

        // Evento para el botón de Clientes
        private void btnClientes_Click(object sender, EventArgs e)
        {
            var form = new ClienteForm();
            form.ShowDialog();
        }

        // Evento para el botón de Inventario
        private void btnInventario_Click(object sender, EventArgs e)
        {
            var form = new InventarioForm();
            form.ShowDialog();
        }

        // Evento para el botón de Servidor
        private void btnServidor_Click(object sender, EventArgs e)
        {
            var form = new ServidorForm();
            form.ShowDialog();
        }
    }
}