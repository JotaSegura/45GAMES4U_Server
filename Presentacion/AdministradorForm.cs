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
    public partial class AdministradorForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        // Constructor del formulario
        public AdministradorForm()
        {
            InitializeComponent();
            // Configurar controles del formulario
            ConfigurarControles();
            // Cargar la lista de administradores en el DataGridView
            CargarAdministradores();
        }

        // Método para configurar los controles del formulario
        private void ConfigurarControles()
        {
            // Configurar DataGridView
            dgvAdministradores.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();

            // Configurar DateTimePickers
            dtpFechaNacimiento.Format = DateTimePickerFormat.Short;
            dtpFechaContratacion.Format = DateTimePickerFormat.Short;
        }

        // Método para configurar las columnas del DataGridView
        private void ConfigurarColumnasGrid()
        {
            dgvAdministradores.Columns.Clear();

            // Columna para la identificación del administrador
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "Identificacion",
                HeaderText = "ID"
            });

            // Columna para el nombre completo del administrador
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "NombreCompleto",
                HeaderText = "Nombre Completo",
                Width = 200
            });

            // Columna para la fecha de nacimiento del administrador
            dgvAdministradores.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "FechaNacimiento",
                HeaderText = "Fecha Nacimiento",
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    Format = "d"  // Formato corto de fecha
                }
            });

            // Columna para la fecha de contratación del administrador
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

        // Método para cargar la lista de administradores en el DataGridView
        private void CargarAdministradores()
        {
            var admins = _accesoDatos.ObtenerAdministradores();

            // Agregar propiedad calculada para nombre completo
            admins.ForEach(a => a.NombreCompleto = $"{a.Nombre} {a.PrimerApellido} {a.SegundoApellido}");

            dgvAdministradores.DataSource = admins;
        }

        // Evento click del botón guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos del formulario llamando al método ValidarCampos
                ValidarCampos();

                // Crear una nueva instancia de AdministradorEntidad con los datos del formulario
                var admin = new AdministradorEntidad
                {
                    Identificacion = (int)long.Parse(txtIdentificacion.Text),
                    Nombre = txtNombre.Text.Trim(),
                    PrimerApellido = txtPrimerApellido.Text.Trim(),
                    SegundoApellido = txtSegundoApellido.Text.Trim(),
                    FechaNacimiento = dtpFechaNacimiento.Value.Date,
                    FechaContratacion = dtpFechaContratacion.Value.Date
                };

                // Verificar si la identificación ya está registrada
                if (_accesoDatos.ExisteAdministrador(admin.Identificacion))
                    throw new Exception("Esta identificación ya está registrada");

                // Verificar si el administrador es mayor de edad
                if (!EsMayorDeEdad(admin.FechaNacimiento))
                    throw new Exception("El administrador debe ser mayor de edad");

                // Verificar si la fecha de contratación no es futura
                if (admin.FechaContratacion > DateTime.Today)
                    throw new Exception("La fecha de contratación no puede ser futura");

                // Insertar el nuevo administrador en la base de datos
                if (_accesoDatos.InsertarAdministrador(admin))
                {
                    MessageBox.Show("Administrador registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    // Limpiar los campos del formulario
                    LimpiarCampos();
                    // Recargar la lista de administradores en el DataGridView
                    CargarAdministradores();
                }
            }
            catch (Exception ex)
            {
                // Mostrar mensaje de error en caso de excepción
                MessageBox.Show($"Error: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Método para verificar si una fecha de nacimiento corresponde a una persona mayor de edad
        private bool EsMayorDeEdad(DateTime fechaNacimiento)
        {
            int edad = DateTime.Today.Year - fechaNacimiento.Year;
            if (fechaNacimiento.Date > DateTime.Today.AddYears(-edad)) edad--;
            return edad >= 18;
        }

        // Método para validar los campos del formulario
        private void ValidarCampos()
        {
            if (!long.TryParse(txtIdentificacion.Text, out _) || txtIdentificacion.Text.Length < 9)
                throw new Exception("Identificación inválida (debe tener al menos 9 dígitos y unicamente ser digitos)");

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
            dtpFechaContratacion.Value = DateTime.Today;
            txtIdentificacion.Focus();
        }


    }
}