namespace ServerApp.Forms
{
    partial class InventarioForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbTiendas = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvVideojuegos = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.txtExistencias = new System.Windows.Forms.TextBox();
            this.btnAsociar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.dgvInventario = new System.Windows.Forms.DataGridView();
            this.grpAsociacion = new System.Windows.Forms.GroupBox();
            this.grpInventario = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideojuegos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).BeginInit();
            this.grpAsociacion.SuspendLayout();
            this.grpInventario.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbTiendas
            // 
            this.cmbTiendas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTiendas.FormattingEnabled = true;
            this.cmbTiendas.Location = new System.Drawing.Point(8, 41);
            this.cmbTiendas.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.cmbTiendas.Name = "cmbTiendas";
            this.cmbTiendas.Size = new System.Drawing.Size(332, 29);
            this.cmbTiendas.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 21);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(143, 21);
            this.label1.TabIndex = 1;
            this.label1.Text = "Tienda (Activas):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 86);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(171, 21);
            this.label2.TabIndex = 2;
            this.label2.Text = "Videojuegos (Físicos):";
            // 
            // dgvVideojuegos
            // 
            this.dgvVideojuegos.AllowUserToAddRows = false;
            this.dgvVideojuegos.AllowUserToDeleteRows = false;
            this.dgvVideojuegos.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVideojuegos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVideojuegos.Location = new System.Drawing.Point(8, 107);
            this.dgvVideojuegos.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvVideojuegos.Name = "dgvVideojuegos";
            this.dgvVideojuegos.ReadOnly = true;
            this.dgvVideojuegos.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVideojuegos.Size = new System.Drawing.Size(989, 197);
            this.dgvVideojuegos.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 321);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Existencias";
            // 
            // txtExistencias
            // 
            this.txtExistencias.Location = new System.Drawing.Point(8, 341);
            this.txtExistencias.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtExistencias.Name = "txtExistencias";
            this.txtExistencias.Size = new System.Drawing.Size(132, 27);
            this.txtExistencias.TabIndex = 5;
            // 
            // btnAsociar
            // 
            this.btnAsociar.Location = new System.Drawing.Point(8, 375);
            this.btnAsociar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnAsociar.Name = "btnAsociar";
            this.btnAsociar.Size = new System.Drawing.Size(133, 39);
            this.btnAsociar.TabIndex = 6;
            this.btnAsociar.Text = "Asociar";
            this.btnAsociar.UseVisualStyleBackColor = true;
            this.btnAsociar.Click += new System.EventHandler(this.btnAsociar_Click);
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(149, 341);
            this.btnLimpiar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 30);
            this.btnLimpiar.TabIndex = 7;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.Location = new System.Drawing.Point(608, 375);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(133, 39);
            this.btnActualizar.TabIndex = 8;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.UseVisualStyleBackColor = true;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // dgvInventario
            // 
            this.dgvInventario.AllowUserToAddRows = false;
            this.dgvInventario.AllowUserToDeleteRows = false;
            this.dgvInventario.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvInventario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInventario.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInventario.Location = new System.Drawing.Point(4, 24);
            this.dgvInventario.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvInventario.Name = "dgvInventario";
            this.dgvInventario.ReadOnly = true;
            this.dgvInventario.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInventario.Size = new System.Drawing.Size(997, 254);
            this.dgvInventario.TabIndex = 0;
            // 
            // grpAsociacion
            // 
            this.grpAsociacion.Controls.Add(this.cmbTiendas);
            this.grpAsociacion.Controls.Add(this.label1);
            this.grpAsociacion.Controls.Add(this.btnActualizar);
            this.grpAsociacion.Controls.Add(this.label2);
            this.grpAsociacion.Controls.Add(this.btnLimpiar);
            this.grpAsociacion.Controls.Add(this.dgvVideojuegos);
            this.grpAsociacion.Controls.Add(this.btnAsociar);
            this.grpAsociacion.Controls.Add(this.label3);
            this.grpAsociacion.Controls.Add(this.txtExistencias);
            this.grpAsociacion.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.grpAsociacion.Location = new System.Drawing.Point(16, 73);
            this.grpAsociacion.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpAsociacion.Name = "grpAsociacion";
            this.grpAsociacion.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpAsociacion.Size = new System.Drawing.Size(1005, 431);
            this.grpAsociacion.TabIndex = 9;
            this.grpAsociacion.TabStop = false;
            this.grpAsociacion.Text = "Asociar Videojuegos a Tienda";
            // 
            // grpInventario
            // 
            this.grpInventario.Controls.Add(this.dgvInventario);
            this.grpInventario.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.grpInventario.Location = new System.Drawing.Point(16, 512);
            this.grpInventario.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpInventario.Name = "grpInventario";
            this.grpInventario.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grpInventario.Size = new System.Drawing.Size(1005, 282);
            this.grpInventario.TabIndex = 10;
            this.grpInventario.TabStop = false;
            this.grpInventario.Text = "Inventario Actual";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 14F);
            this.label4.Location = new System.Drawing.Point(485, 17);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(212, 22);
            this.label4.TabIndex = 11;
            this.label4.Text = "Registro de Inventario";
            // 
            // InventarioForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 854);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.grpInventario);
            this.Controls.Add(this.grpAsociacion);
            this.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "InventarioForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Gestión de Inventario";
            this.Load += new System.EventHandler(this.FormInventario_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvVideojuegos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInventario)).EndInit();
            this.grpAsociacion.ResumeLayout(false);
            this.grpAsociacion.PerformLayout();
            this.grpInventario.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ComboBox cmbTiendas;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvVideojuegos;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExistencias;
        private System.Windows.Forms.Button btnAsociar;
        private System.Windows.Forms.Button btnLimpiar;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.DataGridView dgvInventario;
        private System.Windows.Forms.GroupBox grpAsociacion;
        private System.Windows.Forms.GroupBox grpInventario;
        private System.Windows.Forms.Label label4;
    }
}