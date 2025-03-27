using System;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class TipoVideojuegoForm : Form
    {
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public TipoVideojuegoForm()
        {
            InitializeComponent();
            btnGuardar.Click += new EventHandler(btnGuardar_Click);//Asociar el evento click al boton guardar
            CargarTipos();
        }

        private void CargarTipos()
        {
            dgvTipos.DataSource = _accesoDatos.ObtenerTiposVideojuegos();
            dgvTipos.Columns["Id"].Visible = false; // Ocultar columna ID
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                var tipo = new TipoVideojuegoEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                if (_accesoDatos.InsertarTipoVideojuego(tipo))
                {
                    MessageBox.Show("Tipo de videojuego registrado exitosamente");
                    LimpiarCampos();
                    CargarTipos();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                throw new Exception("La descripción es requerida");
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
        }

    }
}