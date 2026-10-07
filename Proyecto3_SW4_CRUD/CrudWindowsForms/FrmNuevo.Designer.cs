using System.Drawing;
using System.Windows.Forms;

namespace CrudWindowsForms
{
    partial class FrmNuevo
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
            pnlPrincipal = new Panel();
            btnCancelar = new Button();
            btnGuardar = new Button();
            txtEdad = new TextBox();
            txtName = new TextBox();
            lblEdad = new Label();
            lblNombre = new Label();
            pnlEncabezado = new Panel();
            pbIcono = new PictureBox();
            lblSubtitulo2 = new Label();
            lblSubtitulo1 = new Label();
            lblTitulo = new Label();
            pnlPrincipal.SuspendLayout();
            pnlEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcono).BeginInit();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.White;
            pnlPrincipal.Controls.Add(btnCancelar);
            pnlPrincipal.Controls.Add(btnGuardar);
            pnlPrincipal.Controls.Add(txtEdad);
            pnlPrincipal.Controls.Add(txtName);
            pnlPrincipal.Controls.Add(lblEdad);
            pnlPrincipal.Controls.Add(lblNombre);
            pnlPrincipal.Controls.Add(pnlEncabezado);
            pnlPrincipal.Location = new Point(-1, 1);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Size = new Size(457, 612);
            pnlPrincipal.TabIndex = 0;
            // 
            // btnCancelar
            // 
            btnCancelar.BackColor = SystemColors.InactiveCaption;
            btnCancelar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnCancelar.Image = Properties.Resources.Cancelar;
            btnCancelar.Location = new Point(260, 483);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(180, 55);
            btnCancelar.TabIndex = 3;
            btnCancelar.Text = "Cancelar";
            btnCancelar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnCancelar.UseVisualStyleBackColor = false;
            btnCancelar.Click += btnCancelar_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.BackColor = Color.FromArgb(0, 0, 64);
            btnGuardar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnGuardar.ForeColor = Color.White;
            btnGuardar.Image = Properties.Resources.guardar;
            btnGuardar.Location = new Point(48, 483);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(180, 55);
            btnGuardar.TabIndex = 2;
            btnGuardar.Text = "Guardar";
            btnGuardar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnGuardar.UseVisualStyleBackColor = false;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // txtEdad
            // 
            txtEdad.BackColor = SystemColors.InactiveCaption;
            txtEdad.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtEdad.Location = new Point(48, 372);
            txtEdad.Name = "txtEdad";
            txtEdad.Size = new Size(392, 38);
            txtEdad.TabIndex = 1;
            // 
            // txtName
            // 
            txtName.BackColor = SystemColors.InactiveCaption;
            txtName.Font = new Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txtName.Location = new Point(48, 235);
            txtName.MaxLength = 100;
            txtName.Name = "txtName";
            txtName.Size = new Size(392, 38);
            txtName.TabIndex = 0;
            // 
            // lblEdad
            // 
            lblEdad.AutoSize = true;
            lblEdad.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblEdad.Location = new Point(23, 336);
            lblEdad.Name = "lblEdad";
            lblEdad.Size = new Size(55, 23);
            lblEdad.TabIndex = 5;
            lblEdad.Text = "Edad:";
            // 
            // lblNombre
            // 
            lblNombre.AutoSize = true;
            lblNombre.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblNombre.Location = new Point(23, 198);
            lblNombre.Name = "lblNombre";
            lblNombre.Size = new Size(81, 23);
            lblNombre.TabIndex = 4;
            lblNombre.Text = "Nombre:";
            // 
            // pnlEncabezado
            // 
            pnlEncabezado.BackColor = Color.FromArgb(0, 0, 64);
            pnlEncabezado.Controls.Add(pbIcono);
            pnlEncabezado.Controls.Add(lblSubtitulo2);
            pnlEncabezado.Controls.Add(lblSubtitulo1);
            pnlEncabezado.Controls.Add(lblTitulo);
            pnlEncabezado.Location = new Point(-3, 3);
            pnlEncabezado.Name = "pnlEncabezado";
            pnlEncabezado.Size = new Size(537, 148);
            pnlEncabezado.TabIndex = 6;
            // 
            // pbIcono
            // 
            pbIcono.Image = Properties.Resources.Usuario;
            pbIcono.Location = new Point(51, 48);
            pbIcono.Name = "pbIcono";
            pbIcono.Size = new Size(68, 66);
            pbIcono.TabIndex = 3;
            pbIcono.TabStop = false;
            // 
            // lblSubtitulo2
            // 
            lblSubtitulo2.AutoSize = true;
            lblSubtitulo2.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtitulo2.ForeColor = SystemColors.Control;
            lblSubtitulo2.Location = new Point(166, 107);
            lblSubtitulo2.Name = "lblSubtitulo2";
            lblSubtitulo2.Size = new Size(135, 20);
            lblSubtitulo2.TabIndex = 2;
            lblSubtitulo2.Text = "un nuevo registro.";
            // 
            // lblSubtitulo1
            // 
            lblSubtitulo1.AutoSize = true;
            lblSubtitulo1.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtitulo1.ForeColor = SystemColors.Control;
            lblSubtitulo1.Location = new Point(166, 87);
            lblSubtitulo1.Name = "lblSubtitulo1";
            lblSubtitulo1.Size = new Size(235, 20);
            lblSubtitulo1.TabIndex = 1;
            lblSubtitulo1.Text = "Completa los datos para agregar";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Segoe UI Black", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.ForeColor = SystemColors.Control;
            lblTitulo.Location = new Point(166, 31);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(226, 38);
            lblTitulo.TabIndex = 0;
            lblTitulo.Text = "Nueva persona";
            // 
            // FrmNuevo
            // 
            AcceptButton = btnGuardar;
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            BackColor = Color.White;
            CancelButton = btnCancelar;
            ClientSize = new Size(458, 614);
            Controls.Add(pnlPrincipal);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmNuevo";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nueva persona";
            Load += FrmNuevo_Load;
            pnlPrincipal.ResumeLayout(false);
            pnlPrincipal.PerformLayout();
            pnlEncabezado.ResumeLayout(false);
            pnlEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbIcono).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPrincipal;
        private Panel pnlEncabezado;
        private Label lblSubtitulo1;
        private Label lblTitulo;
        private Label lblSubtitulo2;
        private PictureBox pbIcono;
        private Button btnCancelar;
        private Button btnGuardar;
        private TextBox txtEdad;
        private TextBox txtName;
        private Label lblEdad;
        private Label lblNombre;
    }
}
