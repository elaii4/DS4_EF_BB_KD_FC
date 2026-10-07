namespace Proyecto1_SW4
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pEncabezado = new Panel();
            pBoxImagen1 = new PictureBox();
            lblSubtitulo = new Label();
            lblTituloPrincipal = new Label();
            pPunto1 = new Panel();
            txBoxEnlace = new TextBox();
            lblEstado = new Label();
            btnConectar = new Button();
            lblURL = new Label();
            lblConectarAPI = new Label();
            pPunto2 = new Panel();
            pBoxCSV = new PictureBox();
            lblDescripcionCSV = new Label();
            lblCSV = new Label();
            lblDescripcionJSON = new Label();
            lblJSON = new Label();
            pBoxJSON = new PictureBox();
            rBtnCSV = new RadioButton();
            rBtnJSON = new RadioButton();
            lblTipoArchivo = new Label();
            pPunto3 = new Panel();
            btnDescargar = new Button();
            txBoxIDArchivo = new TextBox();
            lblIdArchivo = new Label();
            lblObDescargar = new Label();
            pPunto5 = new Panel();
            pDescarga = new Panel();
            lblPorcentaje = new Label();
            lblTiempo = new Label();
            lblDescargando = new Label();
            pbBarraPDescarga = new ProgressBar();
            pArchivo = new Panel();
            lblKB = new Label();
            lblTam = new Label();
            lblArchivo = new Label();
            lblArchivoDescargado = new Label();
            richTextBox1 = new RichTextBox();
            tabPestañas = new TabControl();
            tabVizualizacionPanel = new TabPage();
            tbPageVisualizacion = new TabPage();
            rtbConsolaLogs = new RichTextBox();
            tabPageLista = new TabPage();
            lstRegistros = new ListBox();
            tabPageDetalles = new TabPage();
            lvRegistros = new ListView();
            tabPageAgregar = new TabPage();
            btnAgregarNuevo = new Button();
            txtEmailNuevo = new TextBox();
            txtApellidoNuevo = new TextBox();
            txtNombreNuevo = new TextBox();
            lblEmailNuevo = new Label();
            lblApellidoNuevo = new Label();
            lblNombreNuevo = new Label();
            label12 = new Label();
            pPunto4 = new Panel();
            comBoxArchivos = new ComboBox();
            lblAbrirArchivo = new Label();
            lblVistaPrevia = new Label();
            btnImprimir = new Button();
            pEncabezado.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxImagen1).BeginInit();
            pPunto1.SuspendLayout();
            pPunto2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxCSV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pBoxJSON).BeginInit();
            pPunto3.SuspendLayout();
            pPunto5.SuspendLayout();
            pDescarga.SuspendLayout();
            pArchivo.SuspendLayout();
            tabPestañas.SuspendLayout();
            tabVizualizacionPanel.SuspendLayout();
            tbPageVisualizacion.SuspendLayout();
            tabPageLista.SuspendLayout();
            tabPageDetalles.SuspendLayout();
            tabPageAgregar.SuspendLayout();
            pPunto4.SuspendLayout();
            SuspendLayout();
            // 
            // pEncabezado
            // 
            pEncabezado.BackColor = Color.FromArgb(0, 64, 64);
            pEncabezado.Controls.Add(pBoxImagen1);
            pEncabezado.Controls.Add(lblSubtitulo);
            pEncabezado.Controls.Add(lblTituloPrincipal);
            pEncabezado.Dock = DockStyle.Top;
            pEncabezado.Location = new Point(0, 0);
            pEncabezado.Name = "pEncabezado";
            pEncabezado.Size = new Size(1323, 106);
            pEncabezado.TabIndex = 0;
            // 
            // pBoxImagen1
            // 
            pBoxImagen1.Image = Properties.Resources.descarga6;
            pBoxImagen1.Location = new Point(47, 11);
            pBoxImagen1.Name = "pBoxImagen1";
            pBoxImagen1.Size = new Size(87, 76);
            pBoxImagen1.TabIndex = 3;
            pBoxImagen1.TabStop = false;
            // 
            // lblSubtitulo
            // 
            lblSubtitulo.AutoSize = true;
            lblSubtitulo.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSubtitulo.ForeColor = SystemColors.ButtonFace;
            lblSubtitulo.Location = new Point(165, 52);
            lblSubtitulo.Name = "lblSubtitulo";
            lblSubtitulo.Size = new Size(698, 23);
            lblSubtitulo.TabIndex = 2;
            lblSubtitulo.Text = "Selecciona el tipo de archivo, obtén  los datos desde una API y visualiza antes de imprimir.";
            // 
            // lblTituloPrincipal
            // 
            lblTituloPrincipal.AutoSize = true;
            lblTituloPrincipal.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTituloPrincipal.ForeColor = SystemColors.ButtonFace;
            lblTituloPrincipal.Location = new Point(165, 11);
            lblTituloPrincipal.Name = "lblTituloPrincipal";
            lblTituloPrincipal.Size = new Size(391, 31);
            lblTituloPrincipal.TabIndex = 1;
            lblTituloPrincipal.Text = "Conexión y descarga desde una API";
            lblTituloPrincipal.Click += label2_Click;
            // 
            // pPunto1
            // 
            pPunto1.BackColor = SystemColors.GradientInactiveCaption;
            pPunto1.Controls.Add(txBoxEnlace);
            pPunto1.Controls.Add(lblEstado);
            pPunto1.Controls.Add(btnConectar);
            pPunto1.Controls.Add(lblURL);
            pPunto1.Controls.Add(lblConectarAPI);
            pPunto1.Location = new Point(14, 116);
            pPunto1.Name = "pPunto1";
            pPunto1.Size = new Size(347, 171);
            pPunto1.TabIndex = 1;
            // 
            // txBoxEnlace
            // 
            txBoxEnlace.Location = new Point(25, 82);
            txBoxEnlace.Name = "txBoxEnlace";
            txBoxEnlace.Size = new Size(278, 27);
            txBoxEnlace.TabIndex = 4;
            txBoxEnlace.Text = "https://dummyjson.com/users?limit=10";
            // 
            // lblEstado
            // 
            lblEstado.AutoSize = true;
            lblEstado.Location = new Point(178, 130);
            lblEstado.Name = "lblEstado";
            lblEstado.Size = new Size(81, 20);
            lblEstado.TabIndex = 3;
            lblEstado.Text = "Conectado";
            // 
            // btnConectar
            // 
            btnConectar.BackColor = Color.FromArgb(0, 64, 64);
            btnConectar.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnConectar.ForeColor = SystemColors.ControlLightLight;
            btnConectar.Location = new Point(35, 125);
            btnConectar.Name = "btnConectar";
            btnConectar.Size = new Size(110, 31);
            btnConectar.TabIndex = 2;
            btnConectar.Text = "Conectar";
            btnConectar.UseVisualStyleBackColor = false;
            btnConectar.Click += btnConectar_Click;
            // 
            // lblURL
            // 
            lblURL.AutoSize = true;
            lblURL.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblURL.Location = new Point(28, 47);
            lblURL.Name = "lblURL";
            lblURL.Size = new Size(101, 20);
            lblURL.TabIndex = 1;
            lblURL.Text = "URL de la API:";
            // 
            // lblConectarAPI
            // 
            lblConectarAPI.AutoSize = true;
            lblConectarAPI.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblConectarAPI.ForeColor = Color.FromArgb(0, 64, 64);
            lblConectarAPI.Location = new Point(22, 15);
            lblConectarAPI.Name = "lblConectarAPI";
            lblConectarAPI.Size = new Size(166, 23);
            lblConectarAPI.TabIndex = 0;
            lblConectarAPI.Text = "1. Conectar a la API";
            // 
            // pPunto2
            // 
            pPunto2.BackColor = SystemColors.GradientInactiveCaption;
            pPunto2.Controls.Add(pBoxCSV);
            pPunto2.Controls.Add(lblDescripcionCSV);
            pPunto2.Controls.Add(lblCSV);
            pPunto2.Controls.Add(lblDescripcionJSON);
            pPunto2.Controls.Add(lblJSON);
            pPunto2.Controls.Add(pBoxJSON);
            pPunto2.Controls.Add(rBtnCSV);
            pPunto2.Controls.Add(rBtnJSON);
            pPunto2.Controls.Add(lblTipoArchivo);
            pPunto2.Location = new Point(14, 304);
            pPunto2.Name = "pPunto2";
            pPunto2.Size = new Size(347, 243);
            pPunto2.TabIndex = 2;
            // 
            // pBoxCSV
            // 
            pBoxCSV.Image = Properties.Resources.csv;
            pBoxCSV.Location = new Point(53, 152);
            pBoxCSV.Name = "pBoxCSV";
            pBoxCSV.Size = new Size(60, 59);
            pBoxCSV.TabIndex = 9;
            pBoxCSV.TabStop = false;
            // 
            // lblDescripcionCSV
            // 
            lblDescripcionCSV.AutoSize = true;
            lblDescripcionCSV.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcionCSV.Location = new Point(119, 187);
            lblDescripcionCSV.Name = "lblDescripcionCSV";
            lblDescripcionCSV.Size = new Size(147, 17);
            lblDescripcionCSV.TabIndex = 8;
            lblDescripcionCSV.Text = "Archivo en formato CSV";
            // 
            // lblCSV
            // 
            lblCSV.AutoSize = true;
            lblCSV.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblCSV.Location = new Point(120, 161);
            lblCSV.Name = "lblCSV";
            lblCSV.Size = new Size(42, 23);
            lblCSV.TabIndex = 7;
            lblCSV.Text = "CSV";
            // 
            // lblDescripcionJSON
            // 
            lblDescripcionJSON.AutoSize = true;
            lblDescripcionJSON.Font = new Font("Segoe UI", 7.8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblDescripcionJSON.Location = new Point(120, 91);
            lblDescripcionJSON.Name = "lblDescripcionJSON";
            lblDescripcionJSON.Size = new Size(160, 17);
            lblDescripcionJSON.TabIndex = 6;
            lblDescripcionJSON.Text = "Archivo en formato JSON ";
            lblDescripcionJSON.Click += label18_Click;
            // 
            // lblJSON
            // 
            lblJSON.AutoSize = true;
            lblJSON.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblJSON.Location = new Point(119, 66);
            lblJSON.Name = "lblJSON";
            lblJSON.Size = new Size(54, 23);
            lblJSON.TabIndex = 5;
            lblJSON.Text = "JSON";
            // 
            // pBoxJSON
            // 
            pBoxJSON.Image = Properties.Resources.jsn1;
            pBoxJSON.Location = new Point(53, 62);
            pBoxJSON.Name = "pBoxJSON";
            pBoxJSON.Size = new Size(60, 59);
            pBoxJSON.TabIndex = 3;
            pBoxJSON.TabStop = false;
            // 
            // rBtnCSV
            // 
            rBtnCSV.AutoSize = true;
            rBtnCSV.Location = new Point(25, 175);
            rBtnCSV.Name = "rBtnCSV";
            rBtnCSV.Size = new Size(17, 16);
            rBtnCSV.TabIndex = 2;
            rBtnCSV.TabStop = true;
            rBtnCSV.UseVisualStyleBackColor = true;
            // 
            // rBtnJSON
            // 
            rBtnJSON.AutoSize = true;
            rBtnJSON.Location = new Point(25, 80);
            rBtnJSON.Name = "rBtnJSON";
            rBtnJSON.Size = new Size(17, 16);
            rBtnJSON.TabIndex = 1;
            rBtnJSON.TabStop = true;
            rBtnJSON.UseVisualStyleBackColor = true;
            // 
            // lblTipoArchivo
            // 
            lblTipoArchivo.AutoSize = true;
            lblTipoArchivo.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblTipoArchivo.ForeColor = Color.FromArgb(0, 64, 64);
            lblTipoArchivo.Location = new Point(22, 15);
            lblTipoArchivo.Name = "lblTipoArchivo";
            lblTipoArchivo.Size = new Size(265, 23);
            lblTipoArchivo.TabIndex = 0;
            lblTipoArchivo.Text = "2. Seleccionar el tipo de archivo";
            // 
            // pPunto3
            // 
            pPunto3.BackColor = SystemColors.GradientInactiveCaption;
            pPunto3.Controls.Add(btnDescargar);
            pPunto3.Controls.Add(txBoxIDArchivo);
            pPunto3.Controls.Add(lblIdArchivo);
            pPunto3.Controls.Add(lblObDescargar);
            pPunto3.Location = new Point(14, 571);
            pPunto3.Name = "pPunto3";
            pPunto3.Size = new Size(347, 175);
            pPunto3.TabIndex = 2;
            // 
            // btnDescargar
            // 
            btnDescargar.BackColor = Color.FromArgb(0, 64, 64);
            btnDescargar.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDescargar.ForeColor = SystemColors.ControlLight;
            btnDescargar.Location = new Point(111, 127);
            btnDescargar.Name = "btnDescargar";
            btnDescargar.Size = new Size(107, 36);
            btnDescargar.TabIndex = 3;
            btnDescargar.Text = "Descargar ";
            btnDescargar.UseVisualStyleBackColor = false;
            btnDescargar.Click += button2_Click;
            // 
            // txBoxIDArchivo
            // 
            txBoxIDArchivo.Location = new Point(25, 84);
            txBoxIDArchivo.Name = "txBoxIDArchivo";
            txBoxIDArchivo.Size = new Size(303, 27);
            txBoxIDArchivo.TabIndex = 2;
            txBoxIDArchivo.Text = "1234454";
            // 
            // lblIdArchivo
            // 
            lblIdArchivo.AutoSize = true;
            lblIdArchivo.Location = new Point(27, 47);
            lblIdArchivo.Name = "lblIdArchivo";
            lblIdArchivo.Size = new Size(196, 20);
            lblIdArchivo.TabIndex = 1;
            lblIdArchivo.Text = "ID del archivo (según la API)";
            // 
            // lblObDescargar
            // 
            lblObDescargar.AutoSize = true;
            lblObDescargar.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblObDescargar.ForeColor = Color.FromArgb(0, 64, 64);
            lblObDescargar.Location = new Point(23, 13);
            lblObDescargar.Name = "lblObDescargar";
            lblObDescargar.Size = new Size(193, 23);
            lblObDescargar.TabIndex = 0;
            lblObDescargar.Text = "3. Obtener y descargar";
            // 
            // pPunto5
            // 
            pPunto5.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pPunto5.BackColor = SystemColors.GradientInactiveCaption;
            pPunto5.Controls.Add(pDescarga);
            pPunto5.Controls.Add(pArchivo);
            pPunto5.Controls.Add(tabPestañas);
            pPunto5.Controls.Add(label12);
            pPunto5.Location = new Point(382, 113);
            pPunto5.Name = "pPunto5";
            pPunto5.Size = new Size(917, 765);
            pPunto5.TabIndex = 2;
            pPunto5.Paint += pPunto5_Paint;
            // 
            // pDescarga
            // 
            pDescarga.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pDescarga.Controls.Add(lblPorcentaje);
            pDescarga.Controls.Add(lblTiempo);
            pDescarga.Controls.Add(lblDescargando);
            pDescarga.Controls.Add(pbBarraPDescarga);
            pDescarga.Location = new Point(36, 650);
            pDescarga.Name = "pDescarga";
            pDescarga.Size = new Size(833, 100);
            pDescarga.TabIndex = 6;
            // 
            // lblPorcentaje
            // 
            lblPorcentaje.AutoSize = true;
            lblPorcentaje.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblPorcentaje.ForeColor = Color.FromArgb(0, 64, 64);
            lblPorcentaje.Location = new Point(768, 36);
            lblPorcentaje.Name = "lblPorcentaje";
            lblPorcentaje.Size = new Size(48, 25);
            lblPorcentaje.TabIndex = 6;
            lblPorcentaje.Text = "80%";
            // 
            // lblTiempo
            // 
            lblTiempo.AutoSize = true;
            lblTiempo.Font = new Font("Segoe UI", 7.20000029F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTiempo.Location = new Point(17, 70);
            lblTiempo.Name = "lblTiempo";
            lblTiempo.Size = new Size(117, 17);
            lblTiempo.TabIndex = 5;
            lblTiempo.Text = "Tiempo estimado: ";
            // 
            // lblDescargando
            // 
            lblDescargando.AutoSize = true;
            lblDescargando.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblDescargando.Location = new Point(13, 9);
            lblDescargando.Name = "lblDescargando";
            lblDescargando.Size = new Size(111, 20);
            lblDescargando.TabIndex = 4;
            lblDescargando.Text = "Descargando...";
            lblDescargando.Click += label17_Click;
            // 
            // pbBarraPDescarga
            // 
            pbBarraPDescarga.Location = new Point(12, 36);
            pbBarraPDescarga.Name = "pbBarraPDescarga";
            pbBarraPDescarga.Size = new Size(738, 25);
            pbBarraPDescarga.TabIndex = 3;
            // 
            // pArchivo
            // 
            pArchivo.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pArchivo.Controls.Add(lblKB);
            pArchivo.Controls.Add(lblTam);
            pArchivo.Controls.Add(lblArchivo);
            pArchivo.Controls.Add(lblArchivoDescargado);
            pArchivo.Location = new Point(35, 561);
            pArchivo.Name = "pArchivo";
            pArchivo.Size = new Size(834, 75);
            pArchivo.TabIndex = 2;
            // 
            // lblKB
            // 
            lblKB.AutoSize = true;
            lblKB.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblKB.Location = new Point(462, 36);
            lblKB.Name = "lblKB";
            lblKB.Size = new Size(49, 20);
            lblKB.TabIndex = 4;
            lblKB.Text = "1.2 KB";
            // 
            // lblTam
            // 
            lblTam.AutoSize = true;
            lblTam.Location = new Point(462, 12);
            lblTam.Name = "lblTam";
            lblTam.Size = new Size(64, 20);
            lblTam.TabIndex = 3;
            lblTam.Text = "Tamaño:";
            // 
            // lblArchivo
            // 
            lblArchivo.AutoSize = true;
            lblArchivo.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblArchivo.Location = new Point(134, 39);
            lblArchivo.Name = "lblArchivo";
            lblArchivo.Size = new Size(131, 23);
            lblArchivo.TabIndex = 2;
            lblArchivo.Text = "archivo json.123";
            // 
            // lblArchivoDescargado
            // 
            lblArchivoDescargado.AutoSize = true;
            lblArchivoDescargado.Location = new Point(130, 12);
            lblArchivoDescargado.Name = "lblArchivoDescargado";
            lblArchivoDescargado.Size = new Size(144, 20);
            lblArchivoDescargado.TabIndex = 1;
            lblArchivoDescargado.Text = "Archivo descargado:";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            richTextBox1.ReadOnly = true;
            richTextBox1.BackColor = SystemColors.Window;
            richTextBox1.Name = "richTextBox1";
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // tabPestañas
            // 
            tabPestañas.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabPestañas.Controls.Add(tabVizualizacionPanel);
            tabPestañas.Controls.Add(tbPageVisualizacion);
            tabPestañas.Controls.Add(tabPageLista);
            tabPestañas.Controls.Add(tabPageDetalles);
            tabPestañas.Controls.Add(tabPageAgregar);
            tabPestañas.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabPestañas.Location = new Point(32, 53);
            tabPestañas.Name = "tabPestañas";
            tabPestañas.SelectedIndex = 0;
            tabPestañas.Size = new Size(841, 499);
            tabPestañas.TabIndex = 1;
            // 
            // tabVizualizacionPanel
            // 
            tabVizualizacionPanel.BackColor = Color.Transparent;
            tabVizualizacionPanel.Controls.Add(richTextBox1);
            tabVizualizacionPanel.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tabVizualizacionPanel.Location = new Point(4, 29);
            tabVizualizacionPanel.Name = "tabVizualizacionPanel";
            tabVizualizacionPanel.Padding = new Padding(3);
            tabVizualizacionPanel.Size = new Size(833, 466);
            tabVizualizacionPanel.TabIndex = 0;
            tabVizualizacionPanel.Text = "Contenido";
            // 
            // tbPageVisualizacion
            // 
            tbPageVisualizacion.Controls.Add(rtbConsolaLogs);
            tbPageVisualizacion.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbPageVisualizacion.Location = new Point(4, 29);
            tbPageVisualizacion.Name = "tbPageVisualizacion";
            tbPageVisualizacion.Padding = new Padding(3);
            tbPageVisualizacion.Size = new Size(833, 466);
            tbPageVisualizacion.TabIndex = 1;
            tbPageVisualizacion.Text = "Información";
            tbPageVisualizacion.UseVisualStyleBackColor = true;
            // 
            // rtbConsolaLogs
            // 
            rtbConsolaLogs.BackColor = Color.Black;
            rtbConsolaLogs.Dock = DockStyle.Fill;
            rtbConsolaLogs.Font = new Font("Consolas", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            rtbConsolaLogs.ForeColor = Color.LimeGreen;
            rtbConsolaLogs.Location = new Point(3, 3);
            rtbConsolaLogs.Name = "rtbConsolaLogs";
            rtbConsolaLogs.ReadOnly = true;
            rtbConsolaLogs.Size = new Size(827, 460);
            rtbConsolaLogs.TabIndex = 0;
            rtbConsolaLogs.Text = "";
            // 
            // tabPageLista
            // 
            tabPageLista.Controls.Add(lstRegistros);
            tabPageLista.Location = new Point(4, 29);
            tabPageLista.Name = "tabPageLista";
            tabPageLista.Padding = new Padding(3);
            tabPageLista.Size = new Size(833, 466);
            tabPageLista.TabIndex = 2;
            tabPageLista.Text = "Lista (ListBox)";
            tabPageLista.UseVisualStyleBackColor = true;
            // 
            // lstRegistros
            // 
            lstRegistros.Dock = DockStyle.Fill;
            lstRegistros.FormattingEnabled = true;
            lstRegistros.Location = new Point(3, 3);
            lstRegistros.Name = "lstRegistros";
            lstRegistros.Size = new Size(827, 460);
            lstRegistros.TabIndex = 0;
            lstRegistros.SelectedIndexChanged += lstRegistros_SelectedIndexChanged;
            // 
            // tabPageDetalles
            // 
            tabPageDetalles.Controls.Add(lvRegistros);
            tabPageDetalles.Location = new Point(4, 29);
            tabPageDetalles.Name = "tabPageDetalles";
            tabPageDetalles.Padding = new Padding(3);
            tabPageDetalles.Size = new Size(833, 466);
            tabPageDetalles.TabIndex = 3;
            tabPageDetalles.Text = "Detalles (ListView)";
            tabPageDetalles.UseVisualStyleBackColor = true;
            // 
            // lvRegistros
            // 
            lvRegistros.Dock = DockStyle.Fill;
            lvRegistros.FullRowSelect = true;
            lvRegistros.GridLines = true;
            lvRegistros.Location = new Point(3, 3);
            lvRegistros.Name = "lvRegistros";
            lvRegistros.Size = new Size(827, 460);
            lvRegistros.TabIndex = 0;
            lvRegistros.UseCompatibleStateImageBehavior = false;
            lvRegistros.View = View.Details;
            lvRegistros.SelectedIndexChanged += lvRegistros_SelectedIndexChanged;
            // 
            // tabPageAgregar
            // 
            tabPageAgregar.Controls.Add(btnAgregarNuevo);
            tabPageAgregar.Controls.Add(txtEmailNuevo);
            tabPageAgregar.Controls.Add(txtApellidoNuevo);
            tabPageAgregar.Controls.Add(txtNombreNuevo);
            tabPageAgregar.Controls.Add(lblEmailNuevo);
            tabPageAgregar.Controls.Add(lblApellidoNuevo);
            tabPageAgregar.Controls.Add(lblNombreNuevo);
            tabPageAgregar.Location = new Point(4, 29);
            tabPageAgregar.Name = "tabPageAgregar";
            tabPageAgregar.Padding = new Padding(3);
            tabPageAgregar.Size = new Size(833, 466);
            tabPageAgregar.TabIndex = 4;
            tabPageAgregar.Text = "Agregar Registro";
            tabPageAgregar.UseVisualStyleBackColor = true;
            // 
            // btnAgregarNuevo
            // 
            btnAgregarNuevo.BackColor = Color.FromArgb(0, 64, 64);
            btnAgregarNuevo.ForeColor = SystemColors.ControlLightLight;
            btnAgregarNuevo.Location = new Point(100, 140);
            btnAgregarNuevo.Name = "btnAgregarNuevo";
            btnAgregarNuevo.Size = new Size(200, 35);
            btnAgregarNuevo.TabIndex = 6;
            btnAgregarNuevo.Text = "Agregar";
            btnAgregarNuevo.UseVisualStyleBackColor = false;
            btnAgregarNuevo.Click += btnAgregarNuevo_Click;
            // 
            // txtEmailNuevo
            // 
            txtEmailNuevo.Location = new Point(100, 97);
            txtEmailNuevo.Name = "txtEmailNuevo";
            txtEmailNuevo.Size = new Size(200, 27);
            txtEmailNuevo.TabIndex = 5;
            // 
            // txtApellidoNuevo
            // 
            txtApellidoNuevo.Location = new Point(100, 57);
            txtApellidoNuevo.Name = "txtApellidoNuevo";
            txtApellidoNuevo.Size = new Size(200, 27);
            txtApellidoNuevo.TabIndex = 3;
            // 
            // txtNombreNuevo
            // 
            txtNombreNuevo.Location = new Point(100, 17);
            txtNombreNuevo.Name = "txtNombreNuevo";
            txtNombreNuevo.Size = new Size(200, 27);
            txtNombreNuevo.TabIndex = 1;
            // 
            // lblEmailNuevo
            // 
            lblEmailNuevo.AutoSize = true;
            lblEmailNuevo.Location = new Point(20, 100);
            lblEmailNuevo.Name = "lblEmailNuevo";
            lblEmailNuevo.Size = new Size(51, 20);
            lblEmailNuevo.TabIndex = 4;
            lblEmailNuevo.Text = "Email:";
            // 
            // lblApellidoNuevo
            // 
            lblApellidoNuevo.AutoSize = true;
            lblApellidoNuevo.Location = new Point(20, 60);
            lblApellidoNuevo.Name = "lblApellidoNuevo";
            lblApellidoNuevo.Size = new Size(71, 20);
            lblApellidoNuevo.TabIndex = 2;
            lblApellidoNuevo.Text = "Apellido:";
            // 
            // lblNombreNuevo
            // 
            lblNombreNuevo.AutoSize = true;
            lblNombreNuevo.Location = new Point(20, 20);
            lblNombreNuevo.Name = "lblNombreNuevo";
            lblNombreNuevo.Size = new Size(71, 20);
            lblNombreNuevo.TabIndex = 0;
            lblNombreNuevo.Text = "Nombre:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label12.ForeColor = Color.FromArgb(0, 64, 64);
            label12.Location = new Point(23, 18);
            label12.Name = "label12";
            label12.Size = new Size(202, 23);
            label12.TabIndex = 0;
            label12.Text = "Vista previa del archivo ";
            // 
            // pPunto4
            // 
            pPunto4.BackColor = SystemColors.GradientInactiveCaption;
            pPunto4.Controls.Add(comBoxArchivos);
            pPunto4.Controls.Add(lblAbrirArchivo);
            pPunto4.Controls.Add(lblVistaPrevia);
            pPunto4.Location = new Point(14, 772);
            pPunto4.Name = "pPunto4";
            pPunto4.Size = new Size(347, 157);
            pPunto4.TabIndex = 4;
            // 
            // comBoxArchivos
            // 
            comBoxArchivos.FormattingEnabled = true;
            comBoxArchivos.Location = new Point(28, 101);
            comBoxArchivos.Name = "comBoxArchivos";
            comBoxArchivos.Size = new Size(295, 28);
            comBoxArchivos.TabIndex = 2;
            comBoxArchivos.SelectedIndexChanged += comBoxArchivos_SelectedIndexChanged;
            // 
            // lblAbrirArchivo
            // 
            lblAbrirArchivo.AutoSize = true;
            lblAbrirArchivo.Location = new Point(33, 61);
            lblAbrirArchivo.Name = "lblAbrirArchivo";
            lblAbrirArchivo.Size = new Size(77, 20);
            lblAbrirArchivo.TabIndex = 1;
            lblAbrirArchivo.Text = "Abrir con: ";
            // 
            // lblVistaPrevia
            // 
            lblVistaPrevia.AutoSize = true;
            lblVistaPrevia.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblVistaPrevia.ForeColor = Color.FromArgb(0, 64, 64);
            lblVistaPrevia.Location = new Point(28, 27);
            lblVistaPrevia.Name = "lblVistaPrevia";
            lblVistaPrevia.Size = new Size(222, 23);
            lblVistaPrevia.TabIndex = 0;
            lblVistaPrevia.Text = "4. Vista previa del archivo ";
            // 
            // btnImprimir
            // 
            btnImprimir.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnImprimir.BackColor = Color.FromArgb(0, 64, 64);
            btnImprimir.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnImprimir.ForeColor = SystemColors.ControlLight;
            btnImprimir.Location = new Point(1169, 896);
            btnImprimir.Name = "btnImprimir";
            btnImprimir.Size = new Size(112, 45);
            btnImprimir.TabIndex = 0;
            btnImprimir.Text = "Imprimir";
            btnImprimir.UseVisualStyleBackColor = false;
            btnImprimir.Click += btnImprimir_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1323, 953);
            Controls.Add(btnImprimir);
            Controls.Add(pPunto4);
            Controls.Add(pPunto5);
            Controls.Add(pPunto2);
            Controls.Add(pPunto3);
            Controls.Add(pPunto1);
            Controls.Add(pEncabezado);
            Name = "Form1";
            Text = "Cliente  API";
            pEncabezado.ResumeLayout(false);
            pEncabezado.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxImagen1).EndInit();
            pPunto1.ResumeLayout(false);
            pPunto1.PerformLayout();
            pPunto2.ResumeLayout(false);
            pPunto2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pBoxCSV).EndInit();
            ((System.ComponentModel.ISupportInitialize)pBoxJSON).EndInit();
            pPunto3.ResumeLayout(false);
            pPunto3.PerformLayout();
            pPunto5.ResumeLayout(false);
            pPunto5.PerformLayout();
            pDescarga.ResumeLayout(false);
            pDescarga.PerformLayout();
            pArchivo.ResumeLayout(false);
            pArchivo.PerformLayout();
            tabPestañas.ResumeLayout(false);
            tabVizualizacionPanel.ResumeLayout(false);
            tabPageLista.ResumeLayout(false);
            tabPageDetalles.ResumeLayout(false);
            tabPageAgregar.ResumeLayout(false);
            tabPageAgregar.PerformLayout();
            pPunto4.ResumeLayout(false);
            pPunto4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel pEncabezado;
        private Label lblSubtitulo;
        private Label lblTituloPrincipal;
        private Panel pPunto1;
        private Panel pPunto2;
        private Panel pPunto3;
        private Panel pPunto5;
        private TextBox txBoxEnlace;
        private Label lblEstado;
        private Button btnConectar;
        private Label lblURL;
        private Label lblConectarAPI;
        private RadioButton rBtnCSV;
        private RadioButton rBtnJSON;
        private Label lblTipoArchivo;
        private Button btnDescargar;
        private TextBox txBoxIDArchivo;
        private Label lblIdArchivo;
        private Label lblObDescargar;
        private Panel pPunto4;
        private Label lblAbrirArchivo;
        private Label lblVistaPrevia;
        private Panel pArchivo;
        private TabControl tabPestañas;
        private TabPage tabVizualizacionPanel;
        private TabPage tbPageVisualizacion;
        private RichTextBox rtbConsolaLogs;
        private Label label12;
        private ComboBox comBoxArchivos;
        private Label lblKB;
        private Label lblTam;
        private Label lblArchivo;
        private Label lblArchivoDescargado;
        private RichTextBox richTextBox1;
        private ProgressBar pbBarraPDescarga;
        private PictureBox pBoxImagen1;
        private Label lblDescripcionCSV;
        private Label lblCSV;
        private Label lblDescripcionJSON;
        private Label lblJSON;
        private PictureBox pBoxJSON;
        private PictureBox pBoxCSV;
        private Panel pDescarga;
        private Label lblDescargando;
        private Label lblPorcentaje;
        private Label lblTiempo;
        private Button btnImprimir;
        private TabPage tabPageLista;
        private ListBox lstRegistros;
        private TabPage tabPageDetalles;
        private ListView lvRegistros;
        private TabPage tabPageAgregar;
        private TextBox txtNombreNuevo;
        private TextBox txtApellidoNuevo;
        private TextBox txtEmailNuevo;
        private Label lblNombreNuevo;
        private Label lblApellidoNuevo;
        private Label lblEmailNuevo;
        private Button btnAgregarNuevo;
    }
}
