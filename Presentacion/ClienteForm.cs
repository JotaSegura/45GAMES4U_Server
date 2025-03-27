using System;
using System.Linq;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class ClienteForm : Form
    {
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public ClienteForm()
        {
            InitializeComponent();
            btnGuardar.Click += new EventHandler(btnGuardar_Click);//Asociar el evento click al boton guardar
            ConfigurarControles();
            CargarClientes();
        }

        private void ConfigurarControles()
        {
            // Configurar ComboBox
            cmbJugadorEnLinea.Items.AddRange(new[] { "Sí", "No" });
            cmbJugadorEnLinea.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbJugadorEnLinea.SelectedIndex = 0;

            // Configurar DateTimePicker
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaNacimiento.ShowUpDown = false;

            // Configurar DataGridView
            dgvClientes.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

        private void ConfigurarColumnasGrid()
        {
            dgvClientes.Columns.Clear();

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Identificacion",
                HeaderText = "Identificación"
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 200
            });

            dgvClientes.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaNacimiento",
                HeaderText = "Fecha Nacimiento",
                DefaultCellStyle = new DataGridViewCellStyle() { Format = "d" }
            });

            // Columna para Jugador en Línea
            DataGridViewTextBoxColumn colJugador = new DataGridViewTextBoxColumn()
            {
                HeaderText = "Jugador en Línea"
            };
            dgvClientes.Columns.Add(colJugador);

            // Formatear celdas
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

        private void CargarClientes()
        {
            dgvClientes.DataSource = _accesoDatos.ObtenerClientes()
                .OrderBy(c => c.Nombre)
                .ThenBy(c => c.PrimerApellido)
                .ToList();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                var cliente = new ClienteEntidad
                {
                    Identificacion = (int)long.Parse(txtIdentificacion.Text),
                    Nombre = txtNombre.Text.Trim(),
                    PrimerApellido = txtPrimerApellido.Text.Trim(),
                    SegundoApellido = txtSegundoApellido.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value.Date,
                    JugadorEnLinea = cmbJugadorEnLinea.SelectedIndex == 0 // 0=Sí, 1=No
                };

                if (_accesoDatos.InsertarCliente(cliente))
                {
                    MessageBox.Show("Cliente registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarClientes();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        //Validaciones de los campos
        private void ValidarCampos()
        {
            if (!long.TryParse(txtIdentificacion.Text, out _) || txtIdentificacion.Text.Length < 9)
                throw new Exception("Identificación inválida (debe tener al menos 9 dígitos)");

            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtPrimerApellido.Text))
                throw new Exception("El primer apellido es requerido");
            if (string.IsNullOrWhiteSpace(txtSegundoApellido.Text))
                throw new Exception("El segundo apellido es requerido");
        }

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
        //Previene que se ingresen letras en el campo de identificación
        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }


    }
}