namespace Proyecto2_
{
    partial class FormSession
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSession));
            lblTitle = new Label();
            lblAgentName = new Label();
            lblProvider = new Label();
            lblProblem = new Label();
            lblArea = new Label();
            lblTool = new Label();
            lblStatus = new Label();
            btnNewSession = new Button();
            btnConfig = new Button();
            pbAgentIcon = new PictureBox();
            lblSubtitle = new Label();
            ((System.ComponentModel.ISupportInitialize)pbAgentIcon).BeginInit();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.AutoSize = true;
            lblTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            lblTitle.ForeColor = Color.White;
            lblTitle.Location = new Point(30, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(122, 54);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Kodu";
            lblTitle.Click += lblTitle_Click;
            // 
            // lblAgentName
            // 
            lblAgentName.AutoSize = true;
            lblAgentName.Font = new Font("Segoe UI", 12F);
            lblAgentName.ForeColor = Color.FromArgb(203, 213, 225);
            lblAgentName.Location = new Point(35, 110);
            lblAgentName.Name = "lblAgentName";
            lblAgentName.Size = new Size(286, 28);
            lblAgentName.TabIndex = 2;
            lblAgentName.Text = "Agente: Orientador de Phishing";
            // 
            // lblProvider
            // 
            lblProvider.AutoSize = true;
            lblProvider.Font = new Font("Segoe UI", 12F);
            lblProvider.ForeColor = Color.FromArgb(203, 213, 225);
            lblProvider.Location = new Point(35, 150);
            lblProvider.Name = "lblProvider";
            lblProvider.Size = new Size(174, 28);
            lblProvider.TabIndex = 3;
            lblProvider.Text = "Proveedor: Gemini";
            // 
            // lblProblem
            // 
            lblProblem.AutoSize = true;
            lblProblem.Font = new Font("Segoe UI", 12F);
            lblProblem.ForeColor = Color.FromArgb(203, 213, 225);
            lblProblem.Location = new Point(35, 190);
            lblProblem.Name = "lblProblem";
            lblProblem.Size = new Size(316, 28);
            lblProblem.TabIndex = 4;
            lblProblem.Text = "Especialidad: Seguridad de correos";
            // 
            // lblArea
            // 
            lblArea.AutoSize = true;
            lblArea.Font = new Font("Segoe UI", 12F);
            lblArea.ForeColor = Color.FromArgb(203, 213, 225);
            lblArea.Location = new Point(35, 230);
            lblArea.Name = "lblArea";
            lblArea.Size = new Size(135, 28);
            lblArea.TabIndex = 10;
            lblArea.Text = "Área: Phishing";
            // 
            // lblTool
            // 
            lblTool.AutoSize = true;
            lblTool.Font = new Font("Segoe UI", 12F);
            lblTool.ForeColor = Color.FromArgb(203, 213, 225);
            lblTool.Location = new Point(35, 270);
            lblTool.Name = "lblTool";
            lblTool.Size = new Size(301, 28);
            lblTool.TabIndex = 5;
            lblTool.Text = "Herramienta: Lista de indicadores";
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblStatus.ForeColor = Color.FromArgb(100, 200, 100);
            lblStatus.Location = new Point(35, 320);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(210, 28);
            lblStatus.TabIndex = 6;
            lblStatus.Text = "Estado: Sesión activa";
            // 
            // btnNewSession
            // 
            btnNewSession.BackColor = Color.FromArgb(255, 255, 255);
            btnNewSession.Cursor = Cursors.Hand;
            btnNewSession.FlatAppearance.BorderSize = 0;
            btnNewSession.FlatStyle = FlatStyle.Flat;
            btnNewSession.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnNewSession.ForeColor = Color.FromArgb(15, 23, 42);
            btnNewSession.Location = new Point(40, 380);
            btnNewSession.Name = "btnNewSession";
            btnNewSession.Size = new Size(160, 45);
            btnNewSession.TabIndex = 7;
            btnNewSession.Text = "Nueva sesión";
            btnNewSession.UseVisualStyleBackColor = false;
            // 
            // btnConfig
            // 
            btnConfig.BackColor = Color.Transparent;
            btnConfig.Cursor = Cursors.Hand;
            btnConfig.FlatAppearance.BorderColor = Color.FromArgb(148, 163, 184);
            btnConfig.FlatAppearance.MouseOverBackColor = Color.FromArgb(30, 41, 59);
            btnConfig.FlatStyle = FlatStyle.Flat;
            btnConfig.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnConfig.ForeColor = Color.FromArgb(203, 213, 225);
            btnConfig.Location = new Point(220, 380);
            btnConfig.Name = "btnConfig";
            btnConfig.Size = new Size(160, 45);
            btnConfig.TabIndex = 8;
            btnConfig.Text = "Configuración";
            btnConfig.UseVisualStyleBackColor = false;
            btnConfig.Click += btnConfig_Click_1;
            // 
            // pbAgentIcon
            // 
            pbAgentIcon.BackColor = Color.Transparent;
            pbAgentIcon.Image = (Image)resources.GetObject("pbAgentIcon.Image");
            pbAgentIcon.Location = new Point(300, 20);
            pbAgentIcon.Name = "pbAgentIcon";
            pbAgentIcon.Size = new Size(60, 60);
            pbAgentIcon.SizeMode = PictureBoxSizeMode.Zoom;
            pbAgentIcon.TabIndex = 1;
            pbAgentIcon.TabStop = false;
            // 
            // lblSubtitle
            // 
            lblSubtitle.AutoSize = true;
            lblSubtitle.Font = new Font("Segoe UI", 10F);
            lblSubtitle.ForeColor = Color.FromArgb(148, 163, 184);
            lblSubtitle.Location = new Point(35, 74);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(108, 23);
            lblSubtitle.TabIndex = 9;
            lblSubtitle.Text = "Sesión activa";
            // 
            // FormSession
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(15, 23, 42);
            ClientSize = new Size(420, 460);
            Controls.Add(btnConfig);
            Controls.Add(btnNewSession);
            Controls.Add(lblStatus);
            Controls.Add(lblTool);
            Controls.Add(lblArea);
            Controls.Add(lblProblem);
            Controls.Add(lblProvider);
            Controls.Add(lblAgentName);
            Controls.Add(pbAgentIcon);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Name = "FormSession";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Sesión Activa";
            ((System.ComponentModel.ISupportInitialize)pbAgentIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.PictureBox pbAgentIcon;
        private System.Windows.Forms.Label lblAgentName;
        private System.Windows.Forms.Label lblProvider;
        private System.Windows.Forms.Label lblProblem;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Label lblTool;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnNewSession;
        private System.Windows.Forms.Button btnConfig;
    }
}
