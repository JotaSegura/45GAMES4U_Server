namespace ServerApp
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnIniciarServidor = new System.Windows.Forms.Button();
            this.lblClientesConectados = new System.Windows.Forms.Label();
            this.txtBitacora = new System.Windows.Forms.TextBox();
            this.btnTiposVideojuegos = new System.Windows.Forms.Button();
            this.btnVideojuego = new System.Windows.Forms.Button();
            this.btnAdministradores = new System.Windows.Forms.Button();
            this.btnTiendas = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnInventario = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnIniciarServidor
            // 
            this.btnIniciarServidor.Location = new System.Drawing.Point(193, 8);
            this.btnIniciarServidor.Name = "btnIniciarServidor";
            this.btnIniciarServidor.Size = new System.Drawing.Size(254, 23);
            this.btnIniciarServidor.TabIndex = 0;
            this.btnIniciarServidor.Text = "IniciarServidor";
            this.btnIniciarServidor.UseVisualStyleBackColor = true;
            // 
            // lblClientesConectados
            // 
            this.lblClientesConectados.AutoSize = true;
            this.lblClientesConectados.Location = new System.Drawing.Point(13, 13);
            this.lblClientesConectados.Name = "lblClientesConectados";
            this.lblClientesConectados.Size = new System.Drawing.Size(101, 13);
            this.lblClientesConectados.TabIndex = 1;
            this.lblClientesConectados.Text = "ClientesConectados";
            // 
            // txtBitacora
            // 
            this.txtBitacora.Location = new System.Drawing.Point(16, 52);
            this.txtBitacora.Multiline = true;
            this.txtBitacora.Name = "txtBitacora";
            this.txtBitacora.ReadOnly = true;
            this.txtBitacora.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBitacora.Size = new System.Drawing.Size(772, 176);
            this.txtBitacora.TabIndex = 2;
            // 
            // btnTiposVideojuegos
            // 
            this.btnTiposVideojuegos.Location = new System.Drawing.Point(16, 255);
            this.btnTiposVideojuegos.Name = "btnTiposVideojuegos";
            this.btnTiposVideojuegos.Size = new System.Drawing.Size(252, 23);
            this.btnTiposVideojuegos.TabIndex = 3;
            this.btnTiposVideojuegos.Text = "Agregar y Consultar Tipo de Videojuegos";
            this.btnTiposVideojuegos.UseVisualStyleBackColor = true;
            this.btnTiposVideojuegos.Click += new System.EventHandler(this.btnTiposVideojuegos_Click);
            // 
            // btnVideojuego
            // 
            this.btnVideojuego.Location = new System.Drawing.Point(16, 284);
            this.btnVideojuego.Name = "btnVideojuego";
            this.btnVideojuego.Size = new System.Drawing.Size(253, 23);
            this.btnVideojuego.TabIndex = 4;
            this.btnVideojuego.Text = "Agregar y Consultar Videojuegos";
            this.btnVideojuego.UseVisualStyleBackColor = true;
            this.btnVideojuego.Click += new System.EventHandler(this.btnVideojuego_Click);
            // 
            // btnAdministradores
            // 
            this.btnAdministradores.Location = new System.Drawing.Point(17, 313);
            this.btnAdministradores.Name = "btnAdministradores";
            this.btnAdministradores.Size = new System.Drawing.Size(252, 23);
            this.btnAdministradores.TabIndex = 5;
            this.btnAdministradores.Text = "Agregar y Consultar Administradores";
            this.btnAdministradores.UseVisualStyleBackColor = true;
            this.btnAdministradores.Click += new System.EventHandler(this.btnAdministradores_Click);
            // 
            // btnTiendas
            // 
            this.btnTiendas.Location = new System.Drawing.Point(17, 342);
            this.btnTiendas.Name = "btnTiendas";
            this.btnTiendas.Size = new System.Drawing.Size(252, 23);
            this.btnTiendas.TabIndex = 6;
            this.btnTiendas.Text = "Agregar y Consultar Tiendas";
            this.btnTiendas.UseVisualStyleBackColor = true;
            this.btnTiendas.Click += new System.EventHandler(this.btnTiendas_Click);
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(17, 372);
            this.btnClientes.Name = "btnClientes";
            this.btnClientes.Size = new System.Drawing.Size(252, 23);
            this.btnClientes.TabIndex = 7;
            this.btnClientes.Text = "Agregar y Consultar Clientes";
            this.btnClientes.UseVisualStyleBackColor = true;
            this.btnClientes.Click += new System.EventHandler(this.btnClientes_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 235);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Ingreso de Datos ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Bitacora";
            // 
            // btnInventario
            // 
            this.btnInventario.Location = new System.Drawing.Point(20, 402);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(248, 23);
            this.btnInventario.TabIndex = 10;
            this.btnInventario.Text = "Agregar y Consultar Inventario";
            this.btnInventario.UseVisualStyleBackColor = true;
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnInventario);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClientes);
            this.Controls.Add(this.btnTiendas);
            this.Controls.Add(this.btnAdministradores);
            this.Controls.Add(this.btnVideojuego);
            this.Controls.Add(this.btnTiposVideojuegos);
            this.Controls.Add(this.txtBitacora);
            this.Controls.Add(this.lblClientesConectados);
            this.Controls.Add(this.btnIniciarServidor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnIniciarServidor;
        private System.Windows.Forms.Label lblClientesConectados;
        private System.Windows.Forms.TextBox txtBitacora;
        private System.Windows.Forms.Button btnTiposVideojuegos;
        private System.Windows.Forms.Button btnVideojuego;
        private System.Windows.Forms.Button btnAdministradores;
        private System.Windows.Forms.Button btnTiendas;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnInventario;
    }
}