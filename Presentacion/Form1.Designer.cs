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
            this.btnServidor = new System.Windows.Forms.Button();
            this.btnTiposVideojuegos = new System.Windows.Forms.Button();
            this.btnVideojuego = new System.Windows.Forms.Button();
            this.btnAdministradores = new System.Windows.Forms.Button();
            this.btnTiendas = new System.Windows.Forms.Button();
            this.btnClientes = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInventario = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnServidor
            // 
            this.btnServidor.Location = new System.Drawing.Point(344, 46);
            this.btnServidor.Name = "btnServidor";
            this.btnServidor.Size = new System.Drawing.Size(254, 23);
            this.btnServidor.TabIndex = 0;
            this.btnServidor.Text = "Configuracion Servidor";
            this.btnServidor.UseVisualStyleBackColor = true;
            this.btnServidor.Click += new System.EventHandler(this.btnServidor_Click);
            // 
            // btnTiposVideojuegos
            // 
            this.btnTiposVideojuegos.Location = new System.Drawing.Point(12, 46);
            this.btnTiposVideojuegos.Name = "btnTiposVideojuegos";
            this.btnTiposVideojuegos.Size = new System.Drawing.Size(252, 23);
            this.btnTiposVideojuegos.TabIndex = 3;
            this.btnTiposVideojuegos.Text = "Agregar y Consultar Tipo de Videojuegos";
            this.btnTiposVideojuegos.UseVisualStyleBackColor = true;
            this.btnTiposVideojuegos.Click += new System.EventHandler(this.btnTiposVideojuegos_Click);
            // 
            // btnVideojuego
            // 
            this.btnVideojuego.Location = new System.Drawing.Point(11, 75);
            this.btnVideojuego.Name = "btnVideojuego";
            this.btnVideojuego.Size = new System.Drawing.Size(253, 23);
            this.btnVideojuego.TabIndex = 4;
            this.btnVideojuego.Text = "Agregar y Consultar Videojuegos";
            this.btnVideojuego.UseVisualStyleBackColor = true;
            this.btnVideojuego.Click += new System.EventHandler(this.btnVideojuego_Click);
            // 
            // btnAdministradores
            // 
            this.btnAdministradores.Location = new System.Drawing.Point(11, 104);
            this.btnAdministradores.Name = "btnAdministradores";
            this.btnAdministradores.Size = new System.Drawing.Size(252, 23);
            this.btnAdministradores.TabIndex = 5;
            this.btnAdministradores.Text = "Agregar y Consultar Administradores";
            this.btnAdministradores.UseVisualStyleBackColor = true;
            this.btnAdministradores.Click += new System.EventHandler(this.btnAdministradores_Click);
            // 
            // btnTiendas
            // 
            this.btnTiendas.Location = new System.Drawing.Point(11, 133);
            this.btnTiendas.Name = "btnTiendas";
            this.btnTiendas.Size = new System.Drawing.Size(252, 23);
            this.btnTiendas.TabIndex = 6;
            this.btnTiendas.Text = "Agregar y Consultar Tiendas";
            this.btnTiendas.UseVisualStyleBackColor = true;
            this.btnTiendas.Click += new System.EventHandler(this.btnTiendas_Click);
            // 
            // btnClientes
            // 
            this.btnClientes.Location = new System.Drawing.Point(11, 162);
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
            this.label1.Location = new System.Drawing.Point(17, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Ingreso de Datos ";
            // 
            // btnInventario
            // 
            this.btnInventario.Location = new System.Drawing.Point(15, 191);
            this.btnInventario.Name = "btnInventario";
            this.btnInventario.Size = new System.Drawing.Size(248, 23);
            this.btnInventario.TabIndex = 10;
            this.btnInventario.Text = "Agregar y Consultar Inventario";
            this.btnInventario.UseVisualStyleBackColor = true;
            this.btnInventario.Click += new System.EventHandler(this.btnInventario_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(344, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 11;
            this.label2.Text = "Servidor";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnInventario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClientes);
            this.Controls.Add(this.btnTiendas);
            this.Controls.Add(this.btnAdministradores);
            this.Controls.Add(this.btnVideojuego);
            this.Controls.Add(this.btnTiposVideojuegos);
            this.Controls.Add(this.btnServidor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnServidor;
        private System.Windows.Forms.Button btnTiposVideojuegos;
        private System.Windows.Forms.Button btnVideojuego;
        private System.Windows.Forms.Button btnAdministradores;
        private System.Windows.Forms.Button btnTiendas;
        private System.Windows.Forms.Button btnClientes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInventario;
        private System.Windows.Forms.Label label2;
    }
}