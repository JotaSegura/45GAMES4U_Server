/**
  * Uned Primer Cuatrimestre 2025
  * Proyecto 2: Servidor TCP y conectividad con base de datos
  * Estudiante: Jaroth Segura Valverde
  * Fecha de Entrega: 6 de Abril 2025
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Entities;
using ServerApp;

namespace ServerApp.Forms
{
    public partial class InventarioForm : Form
    {
        // Instancia de la clase AccesoDatos para interactuar con la base de datos
        private AccesoDatos _accesoDatos = new AccesoDatos();

        // Constructor del formulario
        public InventarioForm()
        {
            InitializeComponent();
            ConfigurarControles();
        }

        // Método para configurar los controles del formulario
        private void ConfigurarControles()
        {
            // Configurar DataGridViews
            dgvVideojuegos.AutoGenerateColumns = false;
            dgvInventario.AutoGenerateColumns = false;

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

            // Configurar el TextBox para que solo acepte números
            txtExistencias.KeyPress += (s, e) =>
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                {
                    e.Handled = true;
                }
            };
        }

        // Evento que se ejecuta al cargar el formulario
        private void FormInventario_Load(object sender, EventArgs e)
        {
            CargarDatosIniciales();
        }

        // Método para cargar los datos iniciales en el formulario
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

        // Método para cargar las tiendas activas en el ComboBox
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

        // Método para cargar los videojuegos físicos en el DataGridView
        private void CargarVideojuegosFisicos()
        {
            try
            {
                var videojuegos = _accesoDatos.ObtenerVideojuegosFisicos();

                // Crear lista para mostrar en el DataGridView
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

        // Método para cargar el inventario completo en el DataGridView
        private void CargarInventario()
        {
            try
            {
                var inventario = _accesoDatos.ObtenerInventarioCompleto();

                // Crear lista para mostrar en el DataGridView
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

        // Evento que se ejecuta al hacer clic en el botón "Asociar"
        private void btnAsociar_Click(object sender, EventArgs e)
        {
            // Validar los datos antes de proceder
            if (!ValidarDatosAsociacion()) return;

            try
            {
                Cursor = Cursors.WaitCursor;

                int idTienda = (int)cmbTiendas.SelectedValue;
                int existencias = int.Parse(txtExistencias.Text);
                int asociacionesExitosas = 0;

                // Asociar cada videojuego seleccionado con la tienda
                foreach (DataGridViewRow row in dgvVideojuegos.SelectedRows)
                {
                    int idVideojuego = (int)row.Cells["colId"].Value;

                    if (_accesoDatos.AsociarVideojuegoTienda(idTienda, idVideojuego, existencias))
                    {
                        asociacionesExitosas++;
                    }
                }

                // Mostrar mensaje de éxito o error según el resultado
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

        // Método para validar los datos antes de asociar videojuegos con tiendas
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

        // Método para limpiar la selección de videojuegos y el TextBox de existencias
        private void LimpiarSeleccion()
        {
            txtExistencias.Clear();
            dgvVideojuegos.ClearSelection();
        }

        // Evento que se ejecuta al hacer clic en el botón "Actualizar"
        private void btnActualizar_Click(object sender, EventArgs e)
        {
            CargarDatosIniciales();
        }

        // Evento que se ejecuta al hacer clic en el botón "Limpiar"
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarSeleccion();
        }

        // Método para mostrar un mensaje de error en un MessageBox
        private void MostrarError(string mensaje, Exception ex)
        {
            MessageBox.Show($"{mensaje}: {ex.Message}", "Error",
                          MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Método para mostrar un mensaje en un MessageBox
        private void MostrarMensaje(string mensaje, MessageBoxIcon icono)
        {
            MessageBox.Show(mensaje, "Inventario", MessageBoxButtons.OK, icono);
        }
    }
}