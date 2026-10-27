namespace CrudWindowsFormsLinq
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlPrincipal = new System.Windows.Forms.Panel();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.btnBuscar = new System.Windows.Forms.Button();
            this.btnActualizar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.dgvPeople = new System.Windows.Forms.DataGridView();
            this.lblSubtitulo = new System.Windows.Forms.Label();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.pbBorde = new System.Windows.Forms.PictureBox();
            this.pnlMenu = new System.Windows.Forms.Panel();
            this.tbNombreLINQ = new System.Windows.Forms.TextBox();
            this.lblCRUD = new System.Windows.Forms.Label();
            this.pbUsuariosP = new System.Windows.Forms.PictureBox();
            this.lblSistema = new System.Windows.Forms.Label();
            this.pbDB = new System.Windows.Forms.PictureBox();
            this.pnlPrincipal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBorde)).BeginInit();
            this.pnlMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUsuariosP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDB)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlPrincipal
            // 
            this.pnlPrincipal.BackColor = System.Drawing.Color.White;
            this.pnlPrincipal.Controls.Add(this.txtBuscar);
            this.pnlPrincipal.Controls.Add(this.btnBuscar);
            this.pnlPrincipal.Controls.Add(this.btnActualizar);
            this.pnlPrincipal.Controls.Add(this.btnEliminar);
            this.pnlPrincipal.Controls.Add(this.btnEditar);
            this.pnlPrincipal.Controls.Add(this.btnNuevo);
            this.pnlPrincipal.Controls.Add(this.dgvPeople);
            this.pnlPrincipal.Controls.Add(this.lblSubtitulo);
            this.pnlPrincipal.Controls.Add(this.lblTitulo);
            this.pnlPrincipal.Controls.Add(this.pbBorde);
            this.pnlPrincipal.Controls.Add(this.pnlMenu);
            this.pnlPrincipal.Location = new System.Drawing.Point(0, -2);
            this.pnlPrincipal.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlPrincipal.Name = "pnlPrincipal";
            this.pnlPrincipal.Size = new System.Drawing.Size(1197, 736);
            this.pnlPrincipal.TabIndex = 1;
            // 
            // txtBuscar
            // 
            this.txtBuscar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtBuscar.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtBuscar.Location = new System.Drawing.Point(349, 126);
            this.txtBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBuscar.Multiline = true;
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(641, 35);
            this.txtBuscar.TabIndex = 10;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // btnBuscar
            // 
            this.btnBuscar.BackColor = System.Drawing.Color.CornflowerBlue;
            this.btnBuscar.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBuscar.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnBuscar.Location = new System.Drawing.Point(997, 119);
            this.btnBuscar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBuscar.Name = "btnBuscar";
            this.btnBuscar.Size = new System.Drawing.Size(112, 48);
            this.btnBuscar.TabIndex = 9;
            this.btnBuscar.Text = "Buscar";
            this.btnBuscar.UseVisualStyleBackColor = false;
            this.btnBuscar.Click += new System.EventHandler(this.btnBuscar_Click);
            // 
            // btnActualizar
            // 
            this.btnActualizar.BackColor = System.Drawing.Color.Green;
            this.btnActualizar.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnActualizar.ForeColor = System.Drawing.Color.White;
            this.btnActualizar.Image = global::CrudWindowsFormsLinq.Properties.Resources.actualizaricono;
            this.btnActualizar.Location = new System.Drawing.Point(945, 640);
            this.btnActualizar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnActualizar.Name = "btnActualizar";
            this.btnActualizar.Size = new System.Drawing.Size(171, 52);
            this.btnActualizar.TabIndex = 8;
            this.btnActualizar.Text = "Actualizar";
            this.btnActualizar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnActualizar.UseVisualStyleBackColor = false;
            this.btnActualizar.Click += new System.EventHandler(this.btnActualizar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.btnEliminar.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEliminar.ForeColor = System.Drawing.Color.White;
            this.btnEliminar.Image = global::CrudWindowsFormsLinq.Properties.Resources.eliminaricono;
            this.btnEliminar.Location = new System.Drawing.Point(749, 640);
            this.btnEliminar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(171, 52);
            this.btnEliminar.TabIndex = 7;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEliminar.UseVisualStyleBackColor = false;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.BackColor = System.Drawing.Color.Silver;
            this.btnEditar.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEditar.Image = ((System.Drawing.Image)(resources.GetObject("btnEditar.Image")));
            this.btnEditar.Location = new System.Drawing.Point(553, 640);
            this.btnEditar.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(171, 52);
            this.btnEditar.TabIndex = 6;
            this.btnEditar.Text = "Editar";
            this.btnEditar.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnEditar.UseVisualStyleBackColor = false;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnNuevo
            // 
            this.btnNuevo.BackColor = System.Drawing.Color.Teal;
            this.btnNuevo.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNuevo.ForeColor = System.Drawing.Color.White;
            this.btnNuevo.Image = ((System.Drawing.Image)(resources.GetObject("btnNuevo.Image")));
            this.btnNuevo.Location = new System.Drawing.Point(357, 640);
            this.btnNuevo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(171, 52);
            this.btnNuevo.TabIndex = 5;
            this.btnNuevo.Text = "Nuevo";
            this.btnNuevo.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNuevo.UseVisualStyleBackColor = false;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            // 
            // dgvPeople
            // 
            this.dgvPeople.AllowUserToAddRows = false;
            this.dgvPeople.AllowUserToDeleteRows = false;
            this.dgvPeople.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPeople.BackgroundColor = System.Drawing.SystemColors.ButtonHighlight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Navy;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPeople.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPeople.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPeople.EnableHeadersVisualStyles = false;
            this.dgvPeople.Location = new System.Drawing.Point(351, 172);
            this.dgvPeople.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvPeople.MultiSelect = false;
            this.dgvPeople.Name = "dgvPeople";
            this.dgvPeople.ReadOnly = true;
            this.dgvPeople.RowHeadersWidth = 51;
            this.dgvPeople.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPeople.Size = new System.Drawing.Size(759, 450);
            this.dgvPeople.TabIndex = 4;
            this.dgvPeople.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPeople_CellContentClick);
            // 
            // lblSubtitulo
            // 
            this.lblSubtitulo.AutoSize = true;
            this.lblSubtitulo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSubtitulo.Location = new System.Drawing.Point(347, 76);
            this.lblSubtitulo.Name = "lblSubtitulo";
            this.lblSubtitulo.Size = new System.Drawing.Size(539, 28);
            this.lblSubtitulo.TabIndex = 3;
            this.lblSubtitulo.Text = "Gestiona la información de forma rápida, sencilla y segura";
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Palatino Linotype", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitulo.Location = new System.Drawing.Point(336, 16);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(535, 55);
            this.lblTitulo.TabIndex = 2;
            this.lblTitulo.Text = "Administración de personas";
            // 
            // pbBorde
            // 
            this.pbBorde.Location = new System.Drawing.Point(1097, 0);
            this.pbBorde.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbBorde.Name = "pbBorde";
            this.pbBorde.Size = new System.Drawing.Size(97, 103);
            this.pbBorde.TabIndex = 1;
            this.pbBorde.TabStop = false;
            // 
            // pnlMenu
            // 
            this.pnlMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.pnlMenu.Controls.Add(this.tbNombreLINQ);
            this.pnlMenu.Controls.Add(this.lblCRUD);
            this.pnlMenu.Controls.Add(this.pbUsuariosP);
            this.pnlMenu.Controls.Add(this.lblSistema);
            this.pnlMenu.Controls.Add(this.pbDB);
            this.pnlMenu.Location = new System.Drawing.Point(3, 0);
            this.pnlMenu.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pnlMenu.Name = "pnlMenu";
            this.pnlMenu.Size = new System.Drawing.Size(285, 736);
            this.pnlMenu.TabIndex = 0;
            // 
            // tbNombreLINQ
            // 
            this.tbNombreLINQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(64)))));
            this.tbNombreLINQ.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbNombreLINQ.Cursor = System.Windows.Forms.Cursors.No;
            this.tbNombreLINQ.Font = new System.Drawing.Font("Rockwell", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbNombreLINQ.ForeColor = System.Drawing.Color.White;
            this.tbNombreLINQ.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.tbNombreLINQ.Location = new System.Drawing.Point(91, 286);
            this.tbNombreLINQ.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbNombreLINQ.Multiline = true;
            this.tbNombreLINQ.Name = "tbNombreLINQ";
            this.tbNombreLINQ.ReadOnly = true;
            this.tbNombreLINQ.Size = new System.Drawing.Size(133, 63);
            this.tbNombreLINQ.TabIndex = 4;
            this.tbNombreLINQ.Text = "con LINQ to SQL";
            this.tbNombreLINQ.TextChanged += new System.EventHandler(this.tbNombreLINQ_TextChanged);
            // 
            // lblCRUD
            // 
            this.lblCRUD.AutoSize = true;
            this.lblCRUD.Font = new System.Drawing.Font("Segoe UI Semibold", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCRUD.ForeColor = System.Drawing.SystemColors.Control;
            this.lblCRUD.Location = new System.Drawing.Point(108, 251);
            this.lblCRUD.Name = "lblCRUD";
            this.lblCRUD.Size = new System.Drawing.Size(75, 31);
            this.lblCRUD.TabIndex = 3;
            this.lblCRUD.Text = "CRUD";
            // 
            // pbUsuariosP
            // 
            this.pbUsuariosP.Image = global::CrudWindowsFormsLinq.Properties.Resources.personaicono;
            this.pbUsuariosP.Location = new System.Drawing.Point(91, 94);
            this.pbUsuariosP.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbUsuariosP.Name = "pbUsuariosP";
            this.pbUsuariosP.Size = new System.Drawing.Size(93, 82);
            this.pbUsuariosP.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbUsuariosP.TabIndex = 2;
            this.pbUsuariosP.TabStop = false;
            // 
            // lblSistema
            // 
            this.lblSistema.AutoSize = true;
            this.lblSistema.Font = new System.Drawing.Font("Rockwell", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSistema.ForeColor = System.Drawing.SystemColors.Control;
            this.lblSistema.Location = new System.Drawing.Point(77, 218);
            this.lblSistema.Name = "lblSistema";
            this.lblSistema.Size = new System.Drawing.Size(124, 33);
            this.lblSistema.TabIndex = 1;
            this.lblSistema.Text = "Sistema";
            // 
            // pbDB
            // 
            this.pbDB.Image = global::CrudWindowsFormsLinq.Properties.Resources.LINQtoSQLimagen;
            this.pbDB.Location = new System.Drawing.Point(69, 370);
            this.pbDB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pbDB.Name = "pbDB";
            this.pbDB.Size = new System.Drawing.Size(163, 151);
            this.pbDB.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDB.TabIndex = 0;
            this.pbDB.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 729);
            this.Controls.Add(this.pnlPrincipal);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlPrincipal.ResumeLayout(false);
            this.pnlPrincipal.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbBorde)).EndInit();
            this.pnlMenu.ResumeLayout(false);
            this.pnlMenu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbUsuariosP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbDB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlPrincipal;
        private System.Windows.Forms.Button btnActualizar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.DataGridView dgvPeople;
        private System.Windows.Forms.Label lblSubtitulo;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.PictureBox pbBorde;
        private System.Windows.Forms.Panel pnlMenu;
        private System.Windows.Forms.Label lblCRUD;
        private System.Windows.Forms.PictureBox pbUsuariosP;
        private System.Windows.Forms.Label lblSistema;
        private System.Windows.Forms.PictureBox pbDB;
        private System.Windows.Forms.TextBox txtBuscar;
        private System.Windows.Forms.Button btnBuscar;
        private System.Windows.Forms.TextBox tbNombreLINQ;
    }
}

