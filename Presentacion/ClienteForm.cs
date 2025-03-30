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
    public partial class ClienteForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        // Constructor del formulario ClienteForm
        public ClienteForm()
        {
            InitializeComponent();

            // Configurar los controles del formulario
            ConfigurarControles();
            // Cargar la lista de clientes en el DataGridView
            CargarClientes();
        }

        // Método para configurar los controles del formulario
        private void ConfigurarControles()
        {
            // Configurar ComboBox para seleccionar si el jugador está en línea
            cmbJugadorEnLinea.Items.AddRange(new[] { "Sí", "No" });

            // Configurar DateTimePicker para seleccionar la fecha de nacimiento
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.ShowUpDown = false;

            // Configurar DataGridView para mostrar la lista de clientes
            dgvClientes.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

        // Método para configurar las columnas del DataGridView
        private void ConfigurarColumnasGrid()
        {
            // Limpiar las columnas existentes
            dgvClientes.Columns.Clear();

            // Agregar columna para la identificación del cliente
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Identificacion",
                HeaderText = "Identificación"
            });

            // Agregar columna para el nombre completo del cliente
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 200
            });

            // Agregar columna para la fecha de nacimiento del cliente
            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaNacimiento",
                HeaderText = "Fecha Nacimiento",
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "d" }
            });

            // Agregar columna para indicar si el cliente es un jugador en línea
            DataGridViewTextBoxColumn colJugador = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Jugador en Línea"
            };
            dgvClientes.Columns.Add(colJugador);

            // Formatear las celdas del DataGridView
            dgvClientes.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvClientes.Rows.Count)
                {
                    var cliente = dgvClientes.Rows[e.RowIndex].DataBoundItem as ClienteEntidad;

                    if (cliente != null && dgvClientes.Columns[e.ColumnIndex].HeaderText == "Jugador en Línea")
                    {
                        e.Value = cliente.JugadorEnLinea ? "Sí" : "No";
                    }
                }
            };
        }

        // Método para cargar la lista de clientes en el DataGridView
        private void CargarClientes()
        {
            dgvClientes.DataSource = _accesoDatos.ObtenerClientes();
        }

        // Evento click del botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos del formulario
                ValidarCampos();

                // Crear una nueva instancia de ClienteEntidad con los datos del formulario
                var cliente = new ClienteEntidad
                {
                    Identificacion = (int)long.Parse(txtIdentificacion.Text),
                    Nombre = txtNombre.Text.Trim(),
                    PrimerApellido = txtPrimerApellido.Text.Trim(),
                    SegundoApellido = txtSegundoApellido.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value.Date,
                    JugadorEnLinea = cmbJugadorEnLinea.SelectedIndex == 0 // 0=Sí, 1=No
                };

                // Insertar el cliente en la base de datos
                if (_accesoDatos.InsertarCliente(cliente))
                {
                    MessageBox.Show("Cliente registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Limpiar los campos del formulario
                    LimpiarCampos();
                    // Recargar la lista de clientes en el DataGridView
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para validar los campos del formulario
        private void ValidarCampos()
        {
            if (!long.TryParse(txtIdentificacion.Text, out _) || txtIdentificacion.Text.Length < 9)
                throw new Exception("Identificación inválida (debe tener al menos 9 dígitos y se deben digitar unicamente numeros)");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text))
                throw new Exception("El primer apellido es requerido");
            if (string.IsNullOrWhiteSpace(txtSegundoApellido.Text))
                throw new Exception("El segundo apellido es requerido");
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            txtIdentificacion.Clear();
            txtNombre.Clear();
            txtPrimerApellido.Clear();
            txtSegundoApellido.Clear();
            dtpFechaNacimiento.Value = DateTime.Today;
            cmbJugadorEnLinea.SelectedIndex = 0;
            txtIdentificacion.Focus();
        }
    }
}