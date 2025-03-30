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
    public partial class VideojuegoForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public VideojuegoForm()
        {
            InitializeComponent();
            ConfigurarControles();
            CargarDatosIniciales();
        }

        // Método para configurar los controles del formulario
        private void ConfigurarControles()
        {
            // Configuración del ComboBox de Formato
            cmbFormato.Items.AddRange(new[] { "Físico", "Virtual" });

            // Configuración del DataGridView
            dgvVideojuegos.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

        // Método para configurar las columnas del DataGridView
        private void ConfigurarColumnasGrid()
        {
            dgvVideojuegos.Columns.Clear();

            // Columna Nombre
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colNombre",
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });

            // Columna Desarrollador
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colDesarrollador",
                DataPropertyName = "Desarrollador",
                HeaderText = "Desarrollador",
                Width = 150
            });

            // Columna Lanzamiento
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "colLanzamiento",
                DataPropertyName = "Lanzamiento",
                HeaderText = "Año",
                Width = 60
            });

            // Columna Tipo (Solución para propiedad anidada)
            DataGridViewTextBoxColumn colTipo = new DataGridViewTextBoxColumn()
            {
                Name = "colTipo",
                HeaderText = "Tipo",
                Width = 120
            };
            dgvVideojuegos.Columns.Add(colTipo);

            // Columna Formato
            DataGridViewTextBoxColumn colFormato = new DataGridViewTextBoxColumn()
            {
                Name = "colFormato",
                HeaderText = "Formato",
                Width = 80
            };
            dgvVideojuegos.Columns.Add(colFormato);

            // Manejar el evento CellFormatting para mostrar el tipo y formato
            dgvVideojuegos.CellFormatting += (sender, e) =>
            {
                // Mostrar el nombre del tipo de videojuego
                if (dgvVideojuegos.Columns[e.ColumnIndex].Name == "colTipo" && e.Value == null)
                {
                    var videojuego = (VideojuegoEntidad)dgvVideojuegos.Rows[e.RowIndex].DataBoundItem;
                    e.Value = videojuego?.TipoVideojuego?.Nombre;
                }

                // Mostrar el formato del videojuego (Físico o Virtual)
                if (dgvVideojuegos.Columns[e.ColumnIndex].Name == "colFormato" && e.Value == null)
                {
                    var videojuego = (VideojuegoEntidad)dgvVideojuegos.Rows[e.RowIndex].DataBoundItem;
                    e.Value = videojuego?.Fisico == true ? "Físico" : "Virtual";
                }
            };
        }

        // Método para cargar los datos iniciales en el formulario
        private void CargarDatosIniciales()
        {
            // Cargar tipos de videojuego en el ComboBox
            cmbTipo.DataSource = _accesoDatos.ObtenerTiposVideojuegos();
            cmbTipo.DisplayMember = "Nombre";
            cmbTipo.ValueMember = "Id";

            // Cargar videojuegos existentes en el DataGridView
            dgvVideojuegos.DataSource = _accesoDatos.ObtenerVideojuegos();
        }

        // Evento click del botón Guardar
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                // Validar los campos del formulario
                ValidarCampos();

                // Verificar si el videojuego ya existe
                if (ExisteVideojuego(txtNombre.Text.Trim()))
                    throw new Exception("Este videojuego ya existe");

                // Crear una nueva instancia de VideojuegoEntidad con los datos del formulario
                var videojuego = new VideojuegoEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Desarrollador = txtDesarrollador.Text.Trim(),
                    Lanzamiento = int.Parse(txtLanzamiento.Text),
                    Fisico = cmbFormato.SelectedIndex == 0, // 0=Físico, 1=Virtual
                    TipoVideojuego = (TipoVideojuegoEntidad)cmbTipo.SelectedItem
                };

                // Insertar el videojuego en la base de datos
                if (_accesoDatos.InsertarVideojuego(videojuego))
                {
                    MessageBox.Show("Videojuego registrado exitosamente", "Éxito",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LimpiarCampos();
                    dgvVideojuegos.DataSource = _accesoDatos.ObtenerVideojuegos();
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
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
                throw new Exception("El nombre es requerido");

            if (string.IsNullOrWhiteSpace(txtDesarrollador.Text))
                throw new Exception("El desarrollador es requerido");

            if (!int.TryParse(txtLanzamiento.Text, out int año) || año < 1950 || año > DateTime.Now.Year)
                throw new Exception($"El año debe estar entre 1950 y {DateTime.Now.Year}");

            if (cmbTipo.SelectedIndex == -1)
                throw new Exception("Debe seleccionar un tipo de videojuego");
        }

        // Método para verificar si un videojuego ya existe
        private bool ExisteVideojuego(string nombre)
        {
            var videojuegos = _accesoDatos.ObtenerVideojuegos();
            return videojuegos.Any(v => v.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        // Método para limpiar los campos del formulario
        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDesarrollador.Clear();
            txtLanzamiento.Clear();
            cmbFormato.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
            txtNombre.Focus();
        }
    }
}