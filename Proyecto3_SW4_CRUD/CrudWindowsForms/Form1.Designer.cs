using System.Drawing;
using System.Windows.Forms;

namespace CrudWindowsForms
{
    partial class Form1
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            pnlPrincipal = new Panel();
            pnlMenu = new Panel();
            lblCRUD = new Label();
            pbUsuariosP = new PictureBox();
            lblSistema = new Label();
            pbDB = new PictureBox();
            pbBorde = new PictureBox();
            lblTitulo = new Label();
            lblSubtitulo = new Label();
            dgvPeople = new DataGridView();
            btnNuevo = new Button();
            btnEditar = new Button();
            btnEliminar = new Button();
            btnActualizar = new Button();
            pnlPrincipal.SuspendLayout();
            pnlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbUsuariosP).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbDB).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pbBorde).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPeople).BeginInit();
            SuspendLayout();
            // 
            // pnlPrincipal
            // 
            pnlPrincipal.BackColor = Color.White;
            pnlPrincipal.Controls.Add(btnActualizar);
            pnlPrincipal.Controls.Add(btnEliminar);
            pnlPrincipal.Controls.Add(btnEditar);
            pnlPrincipal.Controls.Add(btnNuevo);
            pnlPrincipal.Controls.Add(dgvPeople);
            pnlPrincipal.Controls.Add(lblSubtitulo);
            pnlPrincipal.Controls.Add(lblTitulo);
            pnlPrincipal.Controls.Add(pbBorde);
            pnlPrincipal.Controls.Add(pnlMenu);
            pnlPrincipal.Location = new Point(1, 1);
            pnlPrincipal.Name = "pnlPrincipal";
            pnlPrincipal.Size = new Size(1197, 736);
            pnlPrincipal.TabIndex = 0;
            // 
            // pnlMenu
            // 
            pnlMenu.BackColor = Color.FromArgb(0, 0, 64);
            pnlMenu.Controls.Add(lblCRUD);
            pnlMenu.Controls.Add(pbUsuariosP);
            pnlMenu.Controls.Add(lblSistema);
            pnlMenu.Controls.Add(pbDB);
            pnlMenu.Location = new Point(3, 0);
            pnlMenu.Name = "pnlMenu";
            pnlMenu.Size = new Size(286, 736);
            pnlMenu.TabIndex = 0;
            // 
            // pbDB
            // 
            pbDB.Image = Properties.Resources.BD2;
            pbDB.Location = new Point(69, 370);
            pbDB.Name = "pbDB";
            pbDB.Size = new Size(162, 151);
            pbDB.TabIndex = 0;
            pbDB.TabStop = false;
            // 
            // pbBorde
            // 
            pbBorde.Image = Properties.Resources.Borde;
            pbBorde.Location = new Point(1097, 0);
            pbBorde.Name = "pbBorde";
            pbBorde.Size = new Size(97, 104);
            pbBorde.TabIndex = 1;
            pbBorde.TabStop = false;
            // 
            // lblSistema
            // 
            lblSistema.AutoSize = true;
            lblSistema.Font = new Font("Rockwell", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSistema.ForeColor = SystemColors.Control;
            lblSistema.Location = new Point(78, 218);
            lblSistema.Name = "lblSistema";
            lblSistema.Size = new Size(124, 33);
            lblSistema.TabIndex = 1;
            lblSistema.Text = "Sistema";
            // 
            // pbUsuariosP
            // 
            pbUsuariosP.Image = Properties.Resources.Usuarios;
            pbUsuariosP.Location = new Point(90, 94);
            pbUsuariosP.Name = "pbUsuariosP";
            pbUsuariosP.Size = new Size(93, 82);
            pbUsuariosP.TabIndex = 2;
            pbUsuariosP.TabStop = false;
            // 
            // lblCRUD
            // 
            lblCRUD.AutoSize = true;
            lblCRUD.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCRUD.ForeColor = SystemColors.Control;
            lblCRUD.Location = new Point(108, 251);
            lblCRUD.Name = "lblCRUD";
            lblCRUD.Size = new Size(75, 31);
            lblCRUD.TabIndex = 3;
            lblCRUD.Text = "CRUD";
            // 
            // lblTitulo
            // 
            lblTitulo.AutoSize = true;
            lblTitulo.Font = new Font("Palatino Linotype", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTitulo.Location = new Point(336, 16);
            lblTitulo.Name = "lblTitulo";
            lblTitulo.Size = new Size(535, 55);
            lblTitulo.TabIndex = 2;
            lblTitulo.Text = "Administración de personas";
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblSubtitulo.Location = new Point(346, 76);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(539, 28);
            lblSubtitulo.TabIndex = 3;
            lblSubtitulo.Text = "Gestiona la información de forma rápida, sencilla y segura";
            // 
            // dgvPeople
            // 
            dgvPeople.AllowUserToAddRows = false;
            dgvPeople.AllowUserToDeleteRows = false;
            dgvPeople.AutoGenerateColumns = true;
            dgvPeople.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPeople.BackgroundColor = SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.Navy;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPeople.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPeople.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPeople.EnableHeadersVisualStyles = false;
            dgvPeople.Location = new Point(357, 150);
            dgvPeople.MultiSelect = false;
            dgvPeople.Name = "dgvPeople";
            dgvPeople.ReadOnly = true;
            dgvPeople.RowHeadersWidth = 51;
            dgvPeople.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPeople.Size = new Size(759, 450);
            dgvPeople.TabIndex = 4;
            // 
            // btnNuevo
            // 
            btnNuevo.BackColor = Color.Teal;
            btnNuevo.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnNuevo.ForeColor = Color.White;
            btnNuevo.Image = Properties.Resources.Nuevo;
            btnNuevo.Location = new Point(357, 640);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(170, 52);
            btnNuevo.TabIndex = 5;
            btnNuevo.Text = "Nuevo";
            btnNuevo.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnNuevo.UseVisualStyleBackColor = false;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnEditar
            // 
            btnEditar.BackColor = Color.Silver;
            btnEditar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEditar.Image = Properties.Resources.Editar;
            btnEditar.Location = new Point(553, 640);
            btnEditar.Name = "btnEditar";
            btnEditar.Size = new Size(170, 52);
            btnEditar.TabIndex = 6;
            btnEditar.Text = "Editar";
            btnEditar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.Click += btnEditar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.BackColor = Color.FromArgb(192, 0, 0);
            btnEliminar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnEliminar.ForeColor = Color.White;
            btnEliminar.Image = Properties.Resources.Eliminar;
            btnEliminar.Location = new Point(749, 640);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(170, 52);
            btnEliminar.TabIndex = 7;
            btnEliminar.Text = "Eliminar";
            btnEliminar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnActualizar
            // 
            btnActualizar.BackColor = Color.Green;
            btnActualizar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnActualizar.ForeColor = Color.White;
            btnActualizar.Image = Properties.Resources.ActualizarBtn;
            btnActualizar.Location = new Point(945, 640);
            btnActualizar.Name = "btnActualizar";
            btnActualizar.Size = new Size(170, 52);
            btnActualizar.TabIndex = 8;
            btnActualizar.Text = "Actualizar";
            btnActualizar.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnActualizar.UseVisualStyleBackColor = false;
            btnActualizar.Click += btnActualizar_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(120F, 120F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(1197, 738);
            Controls.Add(pnlPrincipal);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Administración de personas";
            Load += Form1_Load;
            pnlPrincipal.ResumeLayout(false);
            pnlPrincipal.PerformLayout();
            pnlMenu.ResumeLayout(false);
            pnlMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbUsuariosP).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbDB).EndInit();
            ((System.ComponentModel.ISupportInitialize)pbBorde).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPeople).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlPrincipal;
        private Panel pnlMenu;
        private PictureBox pbBorde;
        private Label lblSistema;
        private PictureBox pbDB;
        private PictureBox pbUsuariosP;
        private Label lblTitulo;
        private Label lblCRUD;
        private Label lblSubtitulo;
        private DataGridView dgvPeople;
        private Button btnNuevo;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnActualizar;
    }
}
