using System;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class AdministradorForm : Form
    {
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public AdministradorForm()
        {
            InitializeComponent();
            btnGuardar.Click += new EventHandler(btnGuardar_Click);//Asociar el evento click al boton guardar
            ConfigurarControles();
            CargarAdministradores();
        }

        private void ConfigurarControles()
        {
            // Configurar DataGridView
            dgvAdministradores.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();

            // Configurar DateTimePickers
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaContratacion.Format = DateTimePickerFormat.Short;
        }

        private void ConfigurarColumnasGrid()
        {
            dgvAdministradores.Columns.Clear();

            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Identificacion",
                HeaderText = "ID"
            });

            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 200
            });

            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaNacimiento",
                HeaderText = "Fecha Nacimiento",
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "d"  // Formato corto de fecha
                }
            });

            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaContratacion",
                HeaderText = "Fecha Contratación",
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "d"  // Formato corto de fecha
                }
            });
        }

        private void CargarAdministradores()
        {
            var admins = _accesoDatos.ObtenerAdministradores();

            // Agregar propiedad calculada para nombre completo
            admins.ForEach(a => a.NombreCompleto = $"{a.Nombre} {a.PrimerApellido} {a.SegundoApellido}");

            dgvAdministradores.DataSource = admins;
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                var admin = new AdministradorEntidad
                {
                    Identificacion = (int)long.Parse(txtIdentificacion.Text),
                    Nombre = txtNombre.Text.Trim(),
                    PrimerApellido = txtPrimerApellido.Text.Trim(),
                    SegundoApellido = txtSegundoApellido.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value.Date,
                    FechaContratacion = dtpFechaContratacion.Value.Date
                };

                if (_accesoDatos.ExisteAdministrador(admin.Identificacion))
                    throw new Exception("Esta identificación ya está registrada");

                if (!EsMayorDeEdad(admin.FechaNacimiento))
                    throw new Exception("El administrador debe ser mayor de edad");

                if (admin.FechaContratacion > DateTime.Today)
                    throw new Exception("La fecha de contratación no puede ser futura");

                if (_accesoDatos.InsertarAdministrador(admin))
                {
                    MessageBox.Show("Administrador registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    CargarAdministradores();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool EsMayorDeEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
            return edad >= 18;
        }

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
            dtpFechaContratacion.Value = DateTime.Today;
            txtIdentificacion.Focus();
        }

        private void txtIdentificacion_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Permitir solo números
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                e.Handled = true;
        }
    }
}