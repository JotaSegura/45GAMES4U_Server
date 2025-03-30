/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using System;
using System.Linq;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class TiendaForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        // Constructor del formulario TiendaForm
        public TiendaForm()
        {
            InitializeComponent();
            ConfigurarControles();
            CargarDatosIniciales();
        }

        // Método para configurar los controles del formulario
        private void ConfigurarControles()
        {
            // Configurar ComboBox de Estado con opciones "Activa" e "Inactiva"
            cmbEstado.Items.AddRange(new[] { "Activa", "Inactiva" });

            // Configurar DataGridView para mostrar las tiendas
            dgvTiendas.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

        // Método para configurar las columnas del DataGridView
        private void ConfigurarColumnasGrid()
        {
            dgvTiendas.Columns.Clear();

            // Columna para el nombre de la tienda
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });

            // Columna para la dirección de la tienda
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Direccion",
                HeaderText = "Dirección",
                Width = 200
            });

            // Columna para el teléfono de la tienda
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono"
            });

            // Columna para el nombre completo del administrador de la tienda
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Administrador",
                Width = 200
            });

            // Columna para el estado de la tienda (Activa/Inactiva)
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Estado"
            });

            // Manejar el formato de las celdas del DataGridView
            dgvTiendas.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvTiendas.Rows.Count)
                {
                    var tienda = dgvTiendas.Rows[e.RowIndex].DataBoundItem as TiendaEntidad;

                    if (tienda != null)
                    {
                        // Formatear la celda del administrador con el nombre completo
                        if (dgvTiendas.Columns[e.ColumnIndex].HeaderText == "Administrador")
                        {
                            e.Value = $"{tienda.Administrador.Nombre} {tienda.Administrador.PrimerApellido} {tienda.Administrador.SegundoApellido}";
                        }
                        // Formatear la celda del estado con "Activa" o "Inactiva"
                        else if (dgvTiendas.Columns[e.ColumnIndex].HeaderText == "Estado")
                        {
                            e.Value = tienda.Activa ? "Activa" : "Inactiva";
                        }
                    }
                }
            };
        }

        // Método para cargar los datos iniciales en el formulario
        private void CargarDatosIniciales()
        {
            try
            {
                // Obtener la lista de administradores y forzar la evaluación inmediata
                var administradores = _accesoDatos.ObtenerAdministradores().ToList();

                // Configurar ComboBox de Administrador con la lista de administradores
                cmbAdministrador.DataSource = administradores;
                cmbAdministrador.DisplayMember = "NombreCompleto";
                cmbAdministrador.ValueMember = "Identificacion";

                // Forzar actualización del ComboBox
                cmbAdministrador.Refresh();

                // Cargar la lista de tiendas en el DataGridView
                dgvTiendas.DataSource = _accesoDatos.ObtenerTiendas();
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error en caso de excepción
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Evento click del botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos del formulario
                ValidarCampos();

                // Crear una nueva instancia de TiendaEntidad con los datos del formulario
                var tienda = new TiendaEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Activa = cmbEstado.SelectedIndex == 0, // 0=Activa, 1=Inactiva
                    Administrador = (AdministradorEntidad)cmbAdministrador.SelectedItem
                };

                // Insertar la nueva tienda en la base de datos
                if (_accesoDatos.InsertarTienda(tienda))
                {
                    // Mostrar mensaje de éxito y limpiar los campos del formulario
                    MessageBox.Show("Tienda registrada exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    // Actualizar la lista de tiendas en el DataGridView
                    dgvTiendas.DataSource = _accesoDatos.ObtenerTiendas();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error en caso de excepción
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para validar los campos del formulario
        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                throw new Exception("La dirección es requerida");

            if (!long.TryParse(txtTelefono.Text, out _) || txtTelefono.Text.Length <= 7)
                throw new Exception("Telefono inválido (debe tener al menos 8 dígitos y deben de ser unicamente digitos)");

            if (cmbAdministrador.SelectedIndex == -1)
                throw new Exception("Debe seleccionar un administrador");
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            cmbEstado.SelectedIndex = 0;
            cmbAdministrador.SelectedIndex = 0;
            txtNombre.Focus();
        }

        // Evento Load del formulario TiendaForm
        private void TiendaForm_Load(object sender, EventArgs e)
        {
        }
    }
}