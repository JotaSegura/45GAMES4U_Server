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
            this.SuspendLayout();
            // 
            // btnIniciarServidor
            // 
            this.btnIniciarServidor.Location = new System.Drawing.Point(193, 28);
            this.btnIniciarServidor.Name = "btnIniciarServidor";
            this.btnIniciarServidor.Size = new System.Drawing.Size(87, 23);
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
            this.txtBitacora.Location = new System.Drawing.Point(16, 30);
            this.txtBitacora.Multiline = true;
            this.txtBitacora.Name = "txtBitacora";
            this.txtBitacora.ReadOnly = true;
            this.txtBitacora.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBitacora.Size = new System.Drawing.Size(171, 348);
            this.txtBitacora.TabIndex = 2;
            // 
            // btnTiposVideojuegos
            // 
            this.btnTiposVideojuegos.Location = new System.Drawing.Point(194, 80);
            this.btnTiposVideojuegos.Name = "btnTiposVideojuegos";
            this.btnTiposVideojuegos.Size = new System.Drawing.Size(252, 23);
            this.btnTiposVideojuegos.TabIndex = 3;
            this.btnTiposVideojuegos.Text = "Agregar y Consultar Tipo de Videojuegos";
            this.btnTiposVideojuegos.UseVisualStyleBackColor = true;
            this.btnTiposVideojuegos.Click += new System.EventHandler(this.btnTiposVideojuegos_Click);
            // 
            // btnVideojuego
            // 
            this.btnVideojuego.Location = new System.Drawing.Point(193, 130);
            this.btnVideojuego.Name = "btnVideojuego";
            this.btnVideojuego.Size = new System.Drawing.Size(253, 23);
            this.btnVideojuego.TabIndex = 4;
            this.btnVideojuego.Text = "Agregar y Consultar Videojuegos";
            this.btnVideojuego.UseVisualStyleBackColor = true;
            this.btnVideojuego.Click += new System.EventHandler(this.btnVideojuego_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
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
    }
}