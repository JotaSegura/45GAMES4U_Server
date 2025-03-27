using System;
using System.Linq;
using System.Windows.Forms;
using Entities;

namespace ServerApp
{
    public partial class VideojuegoForm : Form
    {
        private AccesoDatos _accesoDatos = new AccesoDatos();

        public VideojuegoForm()
        {
            InitializeComponent();
            btnGuardar.Click += new EventHandler(btnGuardar_Click);//Asociar el evento click al boton guardar
            ConfigurarControles();
            CargarDatosIniciales();
        }

        private void ConfigurarControles()
        {
            // Configuración del ComboBox de Formato
            cmbFormato.Items.AddRange(new[] { "Físico", "Virtual" });
            cmbFormato.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbFormato.SelectedIndex = 0;

            // Configuración del DataGridView
            dgvVideojuegos.AutoGenerateColumns = false;
            ConfigurarColumnasGrid();
        }

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

            // Manejar el evento CellFormatting para mostrar el tipo
            dgvVideojuegos.CellFormatting += (sender, e) =>
            {
                if (dgvVideojuegos.Columns[e.ColumnIndex].Name == "colTipo" && e.Value == null)
                {
                    var videojuego = (VideojuegoEntidad)dgvVideojuegos.Rows[e.RowIndex].DataBoundItem;
                    e.Value = videojuego?.TipoVideojuego?.Nombre;
                }

                if (dgvVideojuegos.Columns[e.ColumnIndex].Name == "colFormato" && e.Value == null)
                {
                    var videojuego = (VideojuegoEntidad)dgvVideojuegos.Rows[e.RowIndex].DataBoundItem;
                    e.Value = videojuego?.Fisico == true ? "Físico" : "Virtual";
                }
            };
        }

        private void CargarDatosIniciales()
        {
            // Cargar tipos de videojuego
            cmbTipo.DataSource = _accesoDatos.ObtenerTiposVideojuegos();
            cmbTipo.DisplayMember = "Nombre";
            cmbTipo.ValueMember = "Id";

            // Cargar videojuegos existentes
            dgvVideojuegos.DataSource = _accesoDatos.ObtenerVideojuegos();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                ValidarCampos();

                if (ExisteVideojuego(txtNombre.Text.Trim()))
                    throw new Exception("Este videojuego ya existe");

                var videojuego = new VideojuegoEntidad
                {
                    Nombre = txtNombre.Text.Trim(),
                    Desarrollador = txtDesarrollador.Text.Trim(),
                    Lanzamiento = int.Parse(txtLanzamiento.Text),
                    Fisico = cmbFormato.SelectedIndex == 0, // 0=Físico, 1=Virtual
                    TipoVideojuego = (TipoVideojuegoEntidad)cmbTipo.SelectedItem
                };

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

        private bool ExisteVideojuego(string nombre)
        {
            var videojuegos = _accesoDatos.ObtenerVideojuegos();
            return videojuegos.Any(v => v.Nombre.Equals(nombre, StringComparison.OrdinalIgnoreCase));
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            txtDesarrollador.Clear();
            txtLanzamiento.Clear();
            cmbFormato.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
            txtNombre.Focus();
        }

        // Clase para mostrar "Físico"/"Virtual" en el DataGridView
        private class FormatoConverter : IFormatProvider, ICustomFormatter
        {
            public object GetFormat(Type formatType) => formatType == typeof(ICustomFormatter) ? this : null;

            public string Format(string format, object arg, IFormatProvider formatProvider)
            {
                return arg is bool ? ((bool)arg ? "Físico" : "Virtual") : arg.ToString();
            }
        }
    }
}