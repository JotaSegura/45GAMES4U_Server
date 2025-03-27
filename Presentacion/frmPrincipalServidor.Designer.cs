namespace ServerApp
{
    // frmPrincipalServidor.Designer.cs (partial)
    partial class frmPrincipalServidor
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
            this.txtBitacora = new System.Windows.Forms.TextBox();
            this.btnIniciar = new System.Windows.Forms.Button();
            this.btnDetener = new System.Windows.Forms.Button();
            this.lblClientesConectados = new System.Windows.Forms.Label();
            this.btnLimpiarBitacora = new System.Windows.Forms.Button();
            this.SuspendLayout();

            // txtBitacora
            this.txtBitacora.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBitacora.Location = new System.Drawing.Point(12, 41);
            this.txtBitacora.Multiline = true;
            this.txtBitacora.Name = "txtBitacora";
            this.txtBitacora.ReadOnly = true;
            this.txtBitacora.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtBitacora.Size = new System.Drawing.Size(760, 408);
            this.txtBitacora.TabIndex = 0;

            // btnIniciar
            this.btnIniciar.Location = new System.Drawing.Point(12, 12);
            this.btnIniciar.Name = "btnIniciar";
            this.btnIniciar.Size = new System.Drawing.Size(75, 23);
            this.btnIniciar.TabIndex = 1;
            this.btnIniciar.Text = "Iniciar";
            this.btnIniciar.UseVisualStyleBackColor = true;
            this.btnIniciar.Click += new System.EventHandler(this.btnIniciar_Click);

            // btnDetener
            this.btnDetener.Enabled = false;
            this.btnDetener.Location = new System.Drawing.Point(93, 12);
            this.btnDetener.Name = "btnDetener";
            this.btnDetener.Size = new System.Drawing.Size(75, 23);
            this.btnDetener.TabIndex = 2;
            this.btnDetener.Text = "Detener";
            this.btnDetener.UseVisualStyleBackColor = true;
            this.btnDetener.Click += new System.EventHandler(this.btnDetener_Click);

            // lblClientesConectados
            this.lblClientesConectados.AutoSize = true;
            this.lblClientesConectados.Location = new System.Drawing.Point(174, 17);
            this.lblClientesConectados.Name = "lblClientesConectados";
            this.lblClientesConectados.Size = new System.Drawing.Size(109, 13);
            this.lblClientesConectados.TabIndex = 3;
            this.lblClientesConectados.Text = "Clientes conectados: 0";

            // btnLimpiarBitacora
            this.btnLimpiarBitacora.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLimpiarBitacora.Location = new System.Drawing.Point(697, 12);
            this.btnLimpiarBitacora.Name = "btnLimpiarBitacora";
            this.btnLimpiarBitacora.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiarBitacora.TabIndex = 4;
            this.btnLimpiarBitacora.Text = "Limpiar";
            this.btnLimpiarBitacora.UseVisualStyleBackColor = true;
            this.btnLimpiarBitacora.Click += new System.EventHandler(this.btnLimpiarBitacora_Click);

            // frmPrincipalServidor
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 461);
            this.Controls.Add(this.btnLimpiarBitacora);
            this.Controls.Add(this.lblClientesConectados);
            this.Controls.Add(this.btnDetener);
            this.Controls.Add(this.btnIniciar);
            this.Controls.Add(this.txtBitacora);
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "frmPrincipalServidor";
            this.Text = "Servidor 45GAMES4U";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPrincipalServidor_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TextBox txtBitacora;
        private System.Windows.Forms.Button btnIniciar;
        private System.Windows.Forms.Button btnDetener;
        private System.Windows.Forms.Label lblClientesConectados;
        private System.Windows.Forms.Button btnLimpiarBitacora;
    }
}