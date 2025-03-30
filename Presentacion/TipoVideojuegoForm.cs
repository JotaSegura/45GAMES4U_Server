/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using System;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class TipoVideojuegoForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        // Constructor del formulario
        public TipoVideojuegoForm()
        {
            InitializeComponent();
            // Cargar los tipos de videojuegos al inicializar el formulario
            CargarTipos();
        }

        // Método para cargar los tipos de videojuegos en el DataGridView
        private void CargarTipos()
        {
            dgvTipos.DataSource = _accesoDatos.ObtenerTiposVideojuegos();
        }

        // Evento click del botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos del formulario
                ValidarCampos();

                // Crear una nueva instancia de TipoVideojuegoEntidad con los datos del formulario
                var tipo = new TipoVideojuegoEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim()
                };

                // Insertar el nuevo tipo de videojuego en la base de datos
                if (_accesoDatos.InsertarTipoVideojuego(tipo))
                {
                    // Mostrar mensaje de éxito
                    MessageBox.Show("Tipo de videojuego registrado exitosamente");
                    // Limpiar los campos del formulario
                    LimpiarCampos();
                    // Recargar los tipos de videojuegos en el DataGridView
                    CargarTipos();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error en caso de excepción
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para validar los campos del formulario
        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
                throw new Exception("La descripción es requerida");
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDescripcion.Clear();
        }
    }
}