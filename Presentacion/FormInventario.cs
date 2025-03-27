using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Entities;
using ServerApp;

namespace ServerApp.Forms
{
    public partial class FormInventario : Form
    {
        private readonly AccesoDatos _accesoDatos;

        public FormInventario()
        {
            InitializeComponent();
            _accesoDatos = new AccesoDatos();
            ConfigurarControles();
        }

        private void ConfigurarControles()
        {
            // Configuración básica del ComboBox
            cmbTiendas.DropDownStyle = ComboBoxStyle.DropDownList;

            // Configurar DataGridViews
            dgvVideojuegos.AutoGenerateColumns = false;
            dgvVideojuegos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvVideojuegos.MultiSelect = true;

            dgvInventario.AutoGenerateColumns = false;
            dgvInventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Configurar columnas del DataGridView de Videojuegos
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "colId",
                DataPropertyName = "Id",
                HeaderText = "ID",
                Width = 50
            });
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Nombre",
                HeaderText = "Nombre",
                Width = 150
            });
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoNombre",
                HeaderText = "Tipo",
                Width = 100
            });
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Desarrollador",
                HeaderText = "Desarrollador",
                Width = 120
            });
            dgvVideojuegos.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Lanzamiento",
                HeaderText = "Año",
                Width = 60
            });

            // Configurar columnas del DataGridView de Inventario
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TiendaId",
                HeaderText = "ID Tienda",
                Width = 70
            });
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TiendaNombre",
                HeaderText = "Tienda",
                Width = 120
            });
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VideojuegoId",
                HeaderText = "ID Juego",
                Width = 70
            });
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "VideojuegoNombre",
                HeaderText = "Videojuego",
                Width = 120
            });
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TipoVideojuego",
                HeaderText = "Tipo",
                Width = 100
            });
            dgvInventario.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Existencias",
                HeaderText = "Existencias",
                Width = 80
            });

            // Solo números en las existencias
            txtExistencias.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        private void FormInventario_Load(object sender, EventArgs e)
        {
            CargarDatosIniciales();
        }

        private void CargarDatosIniciales()
        {
            try
            {
                Cursor = Cursors.WaitCursor;

                // 1. Cargar tiendas activas
                CargarTiendas();

                // 2. Cargar videojuegos físicos
                CargarVideojuegosFisicos();

                // 3. Cargar inventario completo
                CargarInventario();
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar datos iniciales", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private void CargarTiendas()
        {
            try
            {
                // Limpiar el ComboBox completamente
                cmbTiendas.DataSource = null;
                cmbTiendas.Items.Clear();

                // Obtener tiendas activas
                var tiendas = _accesoDatos.ObtenerTiendasActivas();

                if (tiendas == null || tiendas.Count == 0)
                {
                    MessageBox.Show("No se encontraron tiendas activas.",
                                  "Información",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Information);
                    return;
                }

                // Asignar al ComboBox
                cmbTiendas.DataSource = tiendas;
                cmbTiendas.DisplayMember = "Nombre";
                cmbTiendas.ValueMember = "Id";

                // Forzar actualización visual
                cmbTiendas.Refresh();

                // Verificar que los datos se asignaron correctamente
                if (cmbTiendas.Items.Count == 0)
                {
                    MessageBox.Show("El ComboBox no recibió los datos correctamente.",
                                  "Error",
                                  MessageBoxButtons.OK,
                                  MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar tiendas", ex);
            }
        }

        private void CargarVideojuegosFisicos()
        {
            try
            {
                var videojuegos = _accesoDatos.ObtenerVideojuegosFisicos();

                // Crear lista anónima para mostrar en el DataGridView
                var datosParaMostrar = videojuegos.Select(v => new
                {
                    v.Id,
                    v.Nombre,
                    TipoNombre = v.TipoVideojuego.Nombre,
                    v.Desarrollador,
                    v.Lanzamiento
                }).ToList();

                dgvVideojuegos.DataSource = datosParaMostrar;
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar videojuegos", ex);
            }
        }

        private void CargarInventario()
        {
            try
            {
                var inventario = _accesoDatos.ObtenerInventarioCompleto();

                // Crear lista anónima para mostrar en el DataGridView
                var datosParaMostrar = inventario.Select(i => new
                {
                    TiendaId = i.Tienda.Id,
                    TiendaNombre = i.Tienda.Nombre,
                    VideojuegoId = i.Videojuego.Id,
                    VideojuegoNombre = i.Videojuego.Nombre,
                    TipoVideojuego = i.Videojuego.TipoVideojuego.Nombre,
                    i.Existencias
                }).ToList();

                dgvInventario.DataSource = datosParaMostrar;
            }
            catch (Exception ex)
            {
                MostrarError("Error al cargar inventario", ex);
            }
        }

        private void btnAsociar_Click(object sender, EventArgs e)
        {

            if (!ValidarDatosAsociacion()) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                int idTienda = (int)cmbTiendas.SelectedValue;
                int existencias = int.Parse(txtExistencias.Text);
                int asociacionesExitosas = 0;

                foreach (DataGridViewRow row in dgvVideojuegos.SelectedRows)
                {
                    int idVideojuego = (int)row.Cells["colId"].Value;

                    if (_accesoDatos.AsociarVideojuegoTienda(idTienda, idVideojuego, existencias))
                    {
                        asociacionesExitosas++;
                    }
                }

                if (asociacionesExitosas > 0)
                {
                    MostrarMensaje($"Se asociaron {asociacionesExitosas} videojuegos correctamente.",
                                 MessageBoxIcon.Information);
                    LimpiarSeleccion();
                    CargarInventario();
                }
                else
                {
                    MostrarMensaje("No se pudo realizar ninguna asociación.",
                                 MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MostrarError("Error al asociar videojuegos", ex);
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        private bool ValidarDatosAsociacion()
        {
            if (cmbTiendas.SelectedIndex == -1)
            {
                MostrarMensaje("Seleccione una tienda válida.", MessageBoxIcon.Warning);
                return false;
            }

            if (dgvVideojuegos.SelectedRows.Count == 0)
            {
                MostrarMensaje("Seleccione al menos un videojuego.", MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(txtExistencias.Text, out int cantidad) || cantidad <= 0)
            {
                MostrarMensaje("Ingrese una cantidad válida mayor a cero.", MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void LimpiarSeleccion()
        {
            txtExistencias.Clear();
            dgvVideojuegos.ClearSelection();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatosIniciales();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarSeleccion();
        }

        private void MostrarError(string mensaje, Exception ex)
        {
            MessageBox.Show($"{mensaje}: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void MostrarMensaje(string mensaje, MessageBoxIcon icono)
        {
            MessageBox.Show(mensaje, "Inventario", MessageBoxButtons.OK, icono);
        }

        // Método para diagnóstico del ComboBox
        private void VerificarComboBox()
        {
            string mensaje = $"Items en ComboBox: {cmbTiendas.Items.Count}\n";
            mensaje += $"DataSource: {(cmbTiendas.DataSource == null ? "Null" : "Asignado")}\n";
            mensaje += $"DisplayMember: {cmbTiendas.DisplayMember}\n";
            mensaje += $"ValueMember: {cmbTiendas.ValueMember}\n";

            if (cmbTiendas.DataSource is List<TiendaEntidad> tiendas)
            {
                mensaje += $"\nTiendas cargadas: {tiendas.Count}\n";
                foreach (var tienda in tiendas.Take(3))
                {
                    mensaje += $"- {tienda.Id}: {tienda.Nombre}\n";
                }
                if (tiendas.Count > 3) mensaje += "...\n";
            }

            MessageBox.Show(mensaje, "Estado del ComboBox", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}