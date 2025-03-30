using System.Windows.Forms;

namespace ServerApp
{
    partial class ServidorForm
    {
        private System.ComponentModel.IContainer components = null;
        private Button btnIniciarServidor;
        private Button btnDetenerServidor;
        private RichTextBox txtBitacora;

        private void InitializeComponent()
        {
            this.btnIniciarServidor = new System.Windows.Forms.Button();
            this.btnDetenerServidor = new System.Windows.Forms.Button();
            this.txtBitacora = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnIniciarServidor
            // 
            this.btnIniciarServidor.Location = new System.Drawing.Point(12, 12);
            this.btnIniciarServidor.Name = "btnIniciarServidor";
            this.btnIniciarServidor.Size = new System.Drawing.Size(175, 23);
            this.btnIniciarServidor.TabIndex = 0;
            this.btnIniciarServidor.Text = "Iniciar Servidor";
            this.btnIniciarServidor.UseVisualStyleBackColor = true;
            this.btnIniciarServidor.Click += new System.EventHandler(this.btnIniciarServidor_Click);
            // 
            // btnDetenerServidor
            // 
            this.btnDetenerServidor.Enabled = false;
            this.btnDetenerServidor.Location = new System.Drawing.Point(12, 41);
            this.btnDetenerServidor.Name = "btnDetenerServidor";
            this.btnDetenerServidor.Size = new System.Drawing.Size(175, 23);
            this.btnDetenerServidor.TabIndex = 1;
            this.btnDetenerServidor.Text = "Detener Servidor";
            this.btnDetenerServidor.UseVisualStyleBackColor = true;
            this.btnDetenerServidor.Click += new System.EventHandler(this.btnDetenerServidor_Click);
            // 
            // txtBitacora
            // 
            this.txtBitacora.Location = new System.Drawing.Point(12, 70);
            this.txtBitacora.Name = "txtBitacora";
            this.txtBitacora.ReadOnly = true;
            this.txtBitacora.Size = new System.Drawing.Size(460, 200);
            this.txtBitacora.TabIndex = 2;
            this.txtBitacora.Text = "";
            // 
            // ServidorForm
            // 
            this.ClientSize = new System.Drawing.Size(511, 299);
            this.Controls.Add(this.txtBitacora);
            this.Controls.Add(this.btnDetenerServidor);
            this.Controls.Add(this.btnIniciarServidor);
            this.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.Name = "ServidorForm";
            this.Text = "Servidor TCP";
            this.ResumeLayout(false);

        }
    }
}
