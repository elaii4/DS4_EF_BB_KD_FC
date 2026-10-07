namespace Proyecto2_
{
    partial class FormConfig
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblProvider = new System.Windows.Forms.Label();
            this.cmbProvider = new System.Windows.Forms.ComboBox();
            this.lblProblem = new System.Windows.Forms.Label();
            this.txtProblem = new System.Windows.Forms.TextBox();
            this.lblInstructions = new System.Windows.Forms.Label();
            this.rtbInstructions = new System.Windows.Forms.RichTextBox();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblProviderStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(30, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(280, 46);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Configura Agente";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.lblSubtitle.Location = new System.Drawing.Point(35, 70);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(325, 23);
            this.lblSubtitle.TabIndex = 11;
            this.lblSubtitle.Text = "Define cómo el orientador ayudará a detectar phishing.";
            // 
            // lblProvider
            // 
            this.lblProvider.AutoSize = true;
            this.lblProvider.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProvider.ForeColor = System.Drawing.Color.White;
            this.lblProvider.Location = new System.Drawing.Point(35, 110);
            this.lblProvider.Name = "lblProvider";
            this.lblProvider.Size = new System.Drawing.Size(89, 23);
            this.lblProvider.TabIndex = 1;
            this.lblProvider.Text = "Proveedor";
            // 
            // cmbProvider
            // 
            this.cmbProvider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.cmbProvider.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProvider.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbProvider.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cmbProvider.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.cmbProvider.FormattingEnabled = true;
            this.cmbProvider.Items.AddRange(new object[] {
            "OpenAI",
            "Claude",
            "Gemini",
            "Grok"});
            this.cmbProvider.Location = new System.Drawing.Point(35, 136);
            this.cmbProvider.Name = "cmbProvider";
            this.cmbProvider.Size = new System.Drawing.Size(400, 31);
            this.cmbProvider.TabIndex = 2;
            // 
            // lblProviderStatus
            // 
            this.lblProviderStatus.AutoSize = true;
            this.lblProviderStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProviderStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(197)))), ((int)(((byte)(94)))));
            this.lblProviderStatus.Location = new System.Drawing.Point(445, 140);
            this.lblProviderStatus.Name = "lblProviderStatus";
            this.lblProviderStatus.Size = new System.Drawing.Size(120, 23);
            this.lblProviderStatus.TabIndex = 12;
            this.lblProviderStatus.Text = "🟢 Conectado";
            // 
            // lblProblem
            // 
            this.lblProblem.AutoSize = true;
            this.lblProblem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblProblem.ForeColor = System.Drawing.Color.White;
            this.lblProblem.Location = new System.Drawing.Point(35, 180);
            this.lblProblem.Name = "lblProblem";
            this.lblProblem.Size = new System.Drawing.Size(270, 23);
            this.lblProblem.TabIndex = 3;
            this.lblProblem.Text = "Problema que resolverá el agente";
            // 
            // txtProblem
            // 
            this.txtProblem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.txtProblem.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtProblem.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtProblem.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txtProblem.Location = new System.Drawing.Point(35, 206);
            this.txtProblem.Multiline = true;
            this.txtProblem.Name = "txtProblem";
            this.txtProblem.Size = new System.Drawing.Size(400, 60);
            this.txtProblem.TabIndex = 4;
            this.txtProblem.Text = "Orientar a los usuarios para identificar señales de riesgo de phishing en correos electrónicos sospechosos.";
            // 
            // lblInstructions
            // 
            this.lblInstructions.AutoSize = true;
            this.lblInstructions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblInstructions.ForeColor = System.Drawing.Color.White;
            this.lblInstructions.Location = new System.Drawing.Point(35, 280);
            this.lblInstructions.Name = "lblInstructions";
            this.lblInstructions.Size = new System.Drawing.Size(193, 23);
            this.lblInstructions.TabIndex = 5;
            this.lblInstructions.Text = "Instrucciones del agente";
            // 
            // rtbInstructions
            // 
            this.rtbInstructions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.rtbInstructions.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbInstructions.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.rtbInstructions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.rtbInstructions.Location = new System.Drawing.Point(35, 306);
            this.rtbInstructions.Name = "rtbInstructions";
            this.rtbInstructions.Size = new System.Drawing.Size(400, 150);
            this.rtbInstructions.TabIndex = 6;
            this.rtbInstructions.Text = "Eres un Orientador de señales de phishing.\nAyuda al usuario a analizar correos electrónicos sospechosos identificando indicadores de riesgo como remitentes extraños, enlaces engañosos, urgencia injustificada y archivos adjuntos peligrosos.\nHaz preguntas sobre el contenido del correo sin pedirles que hagan clic en nada.\nProporciona una lista de indicadores de riesgo detectados.\nNo abras enlaces ni verifiques su contenido real.\nNo inventes datos.";
            // 
            // btnCreate
            // 
            this.btnCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCreate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.btnCreate.Location = new System.Drawing.Point(620, 410);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(200, 45);
            this.btnCreate.TabIndex = 8;
            this.btnCreate.Text = "Crear agente";
            this.btnCreate.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(163)))), ((int)(((byte)(184)))));
            this.btnCancel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(41)))), ((int)(((byte)(59)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(203)))), ((int)(((byte)(213)))), ((int)(((byte)(225)))));
            this.btnCancel.Location = new System.Drawing.Point(470, 410);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(130, 45);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancelar";
            this.btnCancel.UseVisualStyleBackColor = false;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point);
            this.lblStatus.ForeColor = System.Drawing.Color.LightGray;
            this.lblStatus.Location = new System.Drawing.Point(35, 475);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(158, 20);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Agente no configurado";
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(23)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(860, 520);
            this.Controls.Add(this.lblProviderStatus);
            this.Controls.Add(this.lblSubtitle);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.rtbInstructions);
            this.Controls.Add(this.lblInstructions);
            this.Controls.Add(this.txtProblem);
            this.Controls.Add(this.lblProblem);
            this.Controls.Add(this.cmbProvider);
            this.Controls.Add(this.lblProvider);
            this.Controls.Add(this.lblTitle);
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Configurar Agente";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.ComboBox cmbProvider;
        private System.Windows.Forms.Label lblProblem;
        private System.Windows.Forms.TextBox txtProblem;
        private System.Windows.Forms.Label lblInstructions;
        private System.Windows.Forms.RichTextBox rtbInstructions;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblProviderStatus;
        private System.Windows.Forms.Label lblStatus;
    }
}
