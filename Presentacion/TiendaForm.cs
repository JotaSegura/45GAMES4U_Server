using System;
using System.Linq;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class TiendaForm : Form
    {
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public TiendaForm()
        {
            InitializeComponent();
            btnGuardar.Click += new EventHandler(btnGuardar_Click);//Asociar el evento click al boton guardar
            ConfigurarControles();
            CargarDatosIniciales();
        }

        private void ConfigurarControles()
        {
            // Configurar ComboBox de Estado
            cmbEstado.Items.AddRange(new[] { "Activa", "Inactiva" });
            cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbEstado.SelectedIndex = 0;

            // Configurar DataGridView
            dgvTiendas.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

        private void ConfigurarColumnasGrid()
        {
            dgvTiendas.Columns.Clear();

            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });

            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Direccion",
                HeaderText = "Dirección",
                Width = 200
            });

            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Telefono",
                HeaderText = "Teléfono"
            });

            // Columna para Administrador (nombre completo)
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Administrador",
                Width = 200
            });

            // Columna para Estado (Activa/Inactiva)
            dgvTiendas.Columns.Add(new DataGridViewTextBoxColumn()
            {
                HeaderText = "Estado"
            });

            // Manejar el formato de las celdas
            dgvTiendas.CellFormatting += (sender, e) =>
            {
                if (e.RowIndex >= 0 && e.RowIndex < dgvTiendas.Rows.Count)
                {
                    var tienda = dgvTiendas.Rows[e.RowIndex].DataBoundItem as TiendaEntidad;

                    if (tienda != null)
                    {
                        if (dgvTiendas.Columns[e.ColumnIndex].HeaderText == "Administrador")
                        {
                            e.Value = $"{tienda.Administrador.Nombre} {tienda.Administrador.PrimerApellido} {tienda.Administrador.SegundoApellido}";
                        }
                        else if (dgvTiendas.Columns[e.ColumnIndex].HeaderText == "Estado")
                        {
                            e.Value = tienda.Activa ? "Activa" : "Inactiva";
                        }
                    }
                }
            };
        }

        private void CargarDatosIniciales()
            {
            try
            {
                // Obtener administradores y forzar la evaluación inmediata
                var administradores = _accesoDatos.ObtenerAdministradoresParaCombo().ToList();

                // Configurar ComboBox
                cmbAdministrador.DataSource = administradores;
                cmbAdministrador.DisplayMember = "NombreCompleto";
                cmbAdministrador.ValueMember = "Identificacion";

                // Forzar actualización
                cmbAdministrador.Refresh();

                // Cargar tiendas
                dgvTiendas.DataSource = _accesoDatos.ObtenerTiendas();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar datos: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                var tienda = new TiendaEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Direccion = txtDireccion.Text.Trim(),
                    Telefono = txtTelefono.Text.Trim(),
                    Activa = cmbEstado.SelectedIndex == 0, // 0=Activa, 1=Inactiva
                    Administrador = (AdministradorEntidad)cmbAdministrador.SelectedItem
                };

                if (_accesoDatos.InsertarTienda(tienda))
                {
                    MessageBox.Show("Tienda registrada exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    dgvTiendas.DataSource = _accesoDatos.ObtenerTiendas();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ValidarCampos()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtDireccion.Text))
                throw new Exception("La dirección es requerida");

            if (string.IsNullOrWhiteSpace(txtTelefono.Text) || !EsTelefonoValido(txtTelefono.Text))
                throw new Exception("Teléfono inválido (use formato 1234567890)");

            if (cmbAdministrador.SelectedIndex == -1)
                throw new Exception("Debe seleccionar un administrador");
        }

        private bool EsTelefonoValido(string telefono)
        {
            return telefono.All(char.IsDigit) && telefono.Length >= 8;
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDireccion.Clear();
            txtTelefono.Clear();
            cmbEstado.SelectedIndex = 0;
            cmbAdministrador.SelectedIndex = 0;
            txtNombre.Focus();
        }

        private void txtTelefono_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }

        private void TiendaForm_Load(object sender, EventArgs e)
        {

        }
    }
}