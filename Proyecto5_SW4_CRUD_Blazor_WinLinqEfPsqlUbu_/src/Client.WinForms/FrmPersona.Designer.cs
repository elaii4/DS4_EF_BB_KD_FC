namespace CrudWindowsFormsLinq
{
    partial class FrmPersona
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
            this.pnlPrincipal = new System.Windows.Forms.Panel();
            this.btnCancelar = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.txtEdad = new System.Windows.Forms.TextBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblEdad = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.pnlEncabezado = new System.Windows.Forms.Panel();
            this.pbIcono = new System.Windows.Forms.PictureBox();
            this.lblSubtitulo2 = new System.Windows.Forms.Label();
            this.lblSubtitulo1 = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pnlPrincipal.SuspendLayout();
            this.pnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcono)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPrincipal
            // 
            this.pnlPrincipal.BackColor = System.Drawing.Color.White;
            this.pnlPrincipal.Controls.Add(this.btnCancelar);
            this.pnlPrincipal.Controls.Add(this.btnGuardar);
            this.pnlPrincipal.Controls.Add(this.txtEdad);
            this.pnlPrincipal.Controls.Add(this.txtName);
            this.pnlPrincipal.Controls.Add(this.lblEdad);
            this.pnlPrincipal.Controls.Add(this.lblNombre);
            this.pnlPrincipal.Controls.Add(this.pnlEncabezado);
            this.pnlPrincipal.Location = new System.Drawing.Point(1, 0);
            this.pnlPrincipal.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlPrincipal.Name = "pnlPrincipal";
            this.pnlPrincipal.Size = new System.Drawing.Size(343, 497);
            this.pnlPrincipal.TabIndex = 1;
            this.pnlPrincipal.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlPrincipal_Paint);
            // 
            // btnCancelar
            // 
            this.btnCancelar.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancelar.Location = new System.Drawing.Point(195, 392);
            this.btnCancelar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(135, 45);
            this.btnCancelar.TabIndex = 3;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancelar.UseVisualStyleBackColor = false;
            this.btnCancelar.Click += new System.EventHandler(this.btnCancelar_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.btnGuardar.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGuardar.ForeColor = System.Drawing.Color.White;
            this.btnGuardar.Location = new System.Drawing.Point(36, 392);
            this.btnGuardar.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(135, 45);
            this.btnGuardar.TabIndex = 2;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGuardar.UseVisualStyleBackColor = false;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // txtEdad
            // 
            this.txtEdad.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtEdad.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEdad.Location = new System.Drawing.Point(36, 302);
            this.txtEdad.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtEdad.Name = "txtEdad";
            this.txtEdad.Size = new System.Drawing.Size(295, 32);
            this.txtEdad.TabIndex = 1;
            this.txtEdad.TextChanged += new System.EventHandler(this.txtEdad_TextChanged);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtName.Font = new System.Drawing.Font("Segoe UI", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtName.Location = new System.Drawing.Point(36, 191);
            this.txtName.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtName.MaxLength = 100;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(295, 32);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // lblEdad
            // 
            this.lblEdad.AutoSize = true;
            this.lblEdad.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEdad.Location = new System.Drawing.Point(17, 273);
            this.lblEdad.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblEdad.Name = "lblEdad";
            this.lblEdad.Size = new System.Drawing.Size(46, 19);
            this.lblEdad.TabIndex = 5;
            this.lblEdad.Text = "Edad:";
            this.lblEdad.Click += new System.EventHandler(this.lblEdad_Click);
            // 
            // lblNombre
            // 
            this.lblNombre.AutoSize = true;
            this.lblNombre.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNombre.Location = new System.Drawing.Point(17, 161);
            this.lblNombre.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNombre.Name = "lblNombre";
            this.lblNombre.Size = new System.Drawing.Size(69, 19);
            this.lblNombre.TabIndex = 4;
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.Click += new System.EventHandler(this.lblNombre_Click);
            // 
            // pnlEncabezado
            // 
            this.pnlEncabezado.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.pnlEncabezado.Controls.Add(this.pbIcono);
            this.pnlEncabezado.Controls.Add(this.lblSubtitulo2);
            this.pnlEncabezado.Controls.Add(this.lblSubtitulo1);
            this.pnlEncabezado.Controls.Add(this.lblTitulo);
            this.pnlEncabezado.Location = new System.Drawing.Point(-2, 2);
            this.pnlEncabezado.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlEncabezado.Name = "pnlEncabezado";
            this.pnlEncabezado.Size = new System.Drawing.Size(403, 120);
            this.pnlEncabezado.TabIndex = 6;
            this.pnlEncabezado.Paint += new System.Windows.Forms.PaintEventHandler(this.pnlEncabezado_Paint);
            // 
            // pbIcono
            // 
            this.pbIcono.Image = global::CrudWindowsFormsLinq.Properties.Resources.personaicono;
            this.pbIcono.Location = new System.Drawing.Point(38, 39);
            this.pbIcono.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbIcono.Name = "pbIcono";
            this.pbIcono.Size = new System.Drawing.Size(51, 54);
            this.pbIcono.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbIcono.TabIndex = 3;
            this.pbIcono.TabStop = false;
            this.pbIcono.Click += new System.EventHandler(this.pbIcono_Click);
            // 
            // lblSubtitulo2
            // 
            this.lblSubtitulo2.AutoSize = true;
            this.lblSubtitulo2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo2.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSubtitulo2.Location = new System.Drawing.Point(124, 87);
            this.lblSubtitulo2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubtitulo2.Name = "lblSubtitulo2";
            this.lblSubtitulo2.Size = new System.Drawing.Size(103, 15);
            this.lblSubtitulo2.TabIndex = 2;
            this.lblSubtitulo2.Text = "un nuevo registro.";
            this.lblSubtitulo2.Click += new System.EventHandler(this.lblSubtitulo2_Click);
            // 
            // lblSubtitulo1
            // 
            this.lblSubtitulo1.AutoSize = true;
            this.lblSubtitulo1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo1.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSubtitulo1.Location = new System.Drawing.Point(124, 71);
            this.lblSubtitulo1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSubtitulo1.Name = "lblSubtitulo1";
            this.lblSubtitulo1.Size = new System.Drawing.Size(177, 15);
            this.lblSubtitulo1.TabIndex = 1;
            this.lblSubtitulo1.Text = "Completa los datos para agregar";
            this.lblSubtitulo1.Click += new System.EventHandler(this.lblSubtitulo1_Click);
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI Black", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.ForeColor = System.Drawing.SystemColors.Control;
            this.lblTitulo.Location = new System.Drawing.Point(124, 25);
            this.lblTitulo.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(175, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Nueva persona";
            this.lblTitulo.Click += new System.EventHandler(this.lblTitulo_Click);
            // 
            // FrmPersona
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(342, 490);
            this.Controls.Add(this.pnlPrincipal);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "FrmPersona";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FrmPersona";
            this.pnlPrincipal.ResumeLayout(false);
            this.pnlPrincipal.PerformLayout();
            this.pnlEncabezado.ResumeLayout(false);
            this.pnlEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIcono)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPrincipal;
        private System.Windows.Forms.Button btnCancelar;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.TextBox txtEdad;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblEdad;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Panel pnlEncabezado;
        private System.Windows.Forms.PictureBox pbIcono;
        private System.Windows.Forms.Label lblSubtitulo2;
        private System.Windows.Forms.Label lblSubtitulo1;
        private System.Windows.Forms.Label lblTitulo;
    }
}