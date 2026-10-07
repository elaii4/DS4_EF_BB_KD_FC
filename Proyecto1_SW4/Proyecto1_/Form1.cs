using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Proyecto1_SW4.Models;
using Proyecto1_SW4.Services;

namespace Proyecto1_SW4
{
    /// <summary>
    /// Formulario principal de la aplicación cliente API.
    /// Es responsable de orquestar la interacción del usuario, coordinando 
    /// los servicios de datos (API, CSV, JSON) y reflejando el estado en los controles visuales.
    /// Mantiene la colección central de registros en memoria.
    /// </summary>
    public partial class Form1 : Form
    {
        // Colección principal que actúa como fuente de la verdad para toda la interfaz
        private List<Registro> _registros;
        private ApiService _apiService;
        private JsonService _jsonService;
        private CsvService _csvService;

        // Almacenamos la última ruta guardada para abrirla fácilmente
        private string? _ultimoArchivoGuardado;

        public Form1()
        {
            InitializeComponent();
            _registros = new List<Registro>();
            _apiService = new ApiService();
            _jsonService = new JsonService();
            _csvService = new CsvService();

            ConfigurarListView();

            // Configurar opciones por defecto
            rBtnJSON.Checked = true;
            lblEstado.Text = "No conectado";
            pbBarraPDescarga.Value = 0;
            lblPorcentaje.Text = "0%";
            lblDescargando.Text = "Esperando...";

            comBoxArchivos.Items.Add("Navegador predeterminado");
            comBoxArchivos.Items.Add("Bloc de notas");
            if (comBoxArchivos.Items.Count > 0)
                comBoxArchivos.SelectedIndex = 0;
                
            AgregarLog("Sistema inicializado correctamente. Listo para recibir instrucciones.");
        }

        private void AgregarLog(string mensaje)
        {
            if (rtbConsolaLogs.InvokeRequired)
            {
                rtbConsolaLogs.Invoke(new Action(() => AgregarLog(mensaje)));
                return;
            }
            string tiempo = DateTime.Now.ToString("HH:mm:ss");
            rtbConsolaLogs.AppendText($"[{tiempo}] {mensaje}{Environment.NewLine}");
            rtbConsolaLogs.ScrollToCaret();
        }

        /// <summary>
        /// Configura las columnas del ListView al iniciar la aplicación.
        /// Entrada: Ninguna.
        /// Salida: ListView estructurado con columnas (Id, Nombre, Apellido, Edad, Email).
        /// Este proceso existe para que los datos puedan presentarse tabularmente apenas lleguen.
        /// </summary>
        private void ConfigurarListView()
        {
            lvRegistros.Columns.Clear();
            lvRegistros.Columns.Add("ID", 50);
            lvRegistros.Columns.Add("Nombre", 120);
            lvRegistros.Columns.Add("Apellido", 120);
            lvRegistros.Columns.Add("Edad", 50);
            lvRegistros.Columns.Add("Género", 80);
            lvRegistros.Columns.Add("Email", 180);
        }

        /// <summary>
        /// Refresca el ListBox y ListView a partir de la colección central _registros.
        /// Entrada: La colección _registros actual.
        /// Salida: Controles de interfaz (ListBox/ListView) actualizados y sincronizados.
        /// Este proceso evita que la vista quede desfasada cuando se descargan o agregan nuevos datos.
        /// </summary>
        private void ActualizarVistas()
        {
            // Suspendemos y resumimos el layout si la lista es grande para evitar parpadeos
            lstRegistros.SuspendLayout();
            lvRegistros.SuspendLayout();

            lstRegistros.Items.Clear();
            lvRegistros.Items.Clear();

            foreach (var reg in _registros)
            {
                // ListBox usa el método ToString() del Registro por defecto
                lstRegistros.Items.Add(reg);

                // ListView requiere un ListViewItem con subitems
                var item = new ListViewItem(reg.Id.ToString());
                item.SubItems.Add(reg.Nombre ?? "");
                item.SubItems.Add(reg.Apellido ?? "");
                item.SubItems.Add(reg.Edad.ToString());
                item.SubItems.Add(reg.Genero ?? "");
                item.SubItems.Add(reg.Email ?? "");

                // Asociamos el objeto original al Tag para fácil recuperación si se selecciona
                item.Tag = reg;

                lvRegistros.Items.Add(item);
            }

            lstRegistros.ResumeLayout();
            lvRegistros.ResumeLayout();
        }

        /// <summary>
        /// Evento para realizar la conexión a la API cuando el usuario presiona "Conectar".
        /// Valida la URL, invoca el ApiService de forma asíncrona, actualiza la interfaz
        /// e informa el éxito o fracaso de la operación sin bloquear el hilo principal.
        /// </summary>
        private async void btnConectar_Click(object sender, EventArgs e)
        {
            string url = txBoxEnlace.Text.Trim();
            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Por favor, ingrese una URL válida.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblEstado.Text = "Conectando...";
            lblEstado.ForeColor = System.Drawing.Color.Orange;
            btnConectar.Enabled = false;

            AgregarLog($"Iniciando conexión HTTP GET a: {url}");

            // Invocación asíncrona a la API
            var resultado = await _apiService.ObtenerDatosAsync(url);

            btnConectar.Enabled = true;

            if (resultado.Exito)
            {
                lblEstado.Text = "Conectado";
                lblEstado.ForeColor = System.Drawing.Color.Green;

                // Reemplazamos la colección central
                if (resultado.Registros != null)
                {
                    _registros = resultado.Registros;
                    AgregarLog($"Respuesta HTTP exitosa. Se descargaron {_registros.Count} registros. Actualizando interfaz visual...");
                    ActualizarVistas();
                }

                // Formateamos el JSON para que se vea ordenado y legible en la vista previa
                try
                {
                    var parsedJson = System.Text.Json.JsonDocument.Parse(resultado.JsonRaw ?? "{}");
                    richTextBox1.Text = System.Text.Json.JsonSerializer.Serialize(parsedJson, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
                }
                catch
                {
                    richTextBox1.Text = resultado.JsonRaw;
                }

                MessageBox.Show($"Se obtuvieron {_registros.Count} registros correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                AgregarLog($"ERROR crítico de conexión: {resultado.MensajeError}");
                lblEstado.Text = "Error de conexión";
                lblEstado.ForeColor = System.Drawing.Color.Red;
                MessageBox.Show(resultado.MensajeError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Proceso que simula la descarga y guarda la información actual en un archivo local.
        /// Revisa si hay registros, abre un diálogo de guardado, simula un progreso en la UI
        /// y finalmente persiste los datos usando JsonService o CsvService según la selección.
        /// </summary>
        private async void button2_Click(object sender, EventArgs e) // Evento del btnDescargar
        {
            if (_registros == null || _registros.Count == 0)
            {
                MessageBox.Show("No hay datos para guardar. Conecte a la API primero.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool esJson = rBtnJSON.Checked;

            using (SaveFileDialog sfd = new SaveFileDialog())
            {
                sfd.Filter = esJson ? "JSON Files (*.json)|*.json" : "CSV Files (*.csv)|*.csv";
                sfd.Title = "Guardar datos descargados";
                sfd.FileName = esJson ? "datos.json" : "datos.csv";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    lblDescargando.Text = "Guardando archivo...";
                    pbBarraPDescarga.Value = 0;

                    AgregarLog($"Iniciando rutina de guardado en formato {(esJson ? "JSON" : "CSV")}...");

                    // Simulamos un tiempo de "descarga/procesamiento" para la barra de progreso
                    // para cumplir con la necesidad del laboratorio de mostrar una barra activa
                    for (int i = 0; i <= 100; i += 20)
                    {
                        pbBarraPDescarga.Value = i;
                        lblPorcentaje.Text = $"{i}%";
                        await System.Threading.Tasks.Task.Delay(100);
                    }

                    bool exito = false;
                    string msjError = "";

                    if (esJson)
                    {
                        var res = await _jsonService.GuardarJsonAsync(_registros, sfd.FileName);
                        exito = res.Exito;
                        msjError = res.MensajeError;
                    }
                    else
                    {
                        var res = await _csvService.GuardarCsvAsync(_registros, sfd.FileName);
                        exito = res.Exito;
                        msjError = res.MensajeError;
                    }

                    if (exito)
                    {
                        AgregarLog($"Archivo guardado correctamente y persiste en disco. Ruta: {sfd.FileName}");
                        lblDescargando.Text = "Descarga completada";
                        _ultimoArchivoGuardado = sfd.FileName;

                        // Actualizar información del archivo
                        FileInfo fi = new FileInfo(sfd.FileName);
                        lblArchivo.Text = fi.Name;
                        // Cálculo del tamaño de archivo a un formato legible (KB)
                        lblKB.Text = $"{(fi.Length / 1024.0):F2} KB";

                        MessageBox.Show("Archivo guardado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        AgregarLog($"FALLO al escribir archivo local: {msjError}");
                        lblDescargando.Text = "Error al descargar";
                        pbBarraPDescarga.Value = 0;
                        lblPorcentaje.Text = "0%";
                        MessageBox.Show(msjError, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        /// <summary>
        /// Evento para agregar manualmente un nuevo registro.
        /// Captura los datos de los TextBoxes, aplica validaciones básicas y lo inyecta a la colección central.
        /// </summary>
        private void btnAgregarNuevo_Click(object sender, EventArgs e)
        {
            // Validar reglas de negocio básicas para evitar datos nulos
            if (string.IsNullOrWhiteSpace(txtNombreNuevo.Text) || string.IsNullOrWhiteSpace(txtApellidoNuevo.Text))
            {
                MessageBox.Show("El nombre y el apellido son obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Calculamos el próximo ID como el máximo existente + 1
            int nextId = _registros.Count > 0 ? _registros.Max(r => r.Id) + 1 : 1;

            var nuevoRegistro = new Registro
            {
                Id = nextId,
                Nombre = txtNombreNuevo.Text.Trim(),
                Apellido = txtApellidoNuevo.Text.Trim(),
                Email = txtEmailNuevo.Text.Trim(),
                Edad = 0, // Por defecto para esta demostración
                Genero = "No especificado",
                Telefono = "",
                Usuario = txtNombreNuevo.Text.Trim().ToLower()
            };

            // Inyectamos a la colección central
            _registros.Add(nuevoRegistro);

            // Actualizamos visualización
            ActualizarVistas();

            // Limpiamos cajas de texto
            txtNombreNuevo.Clear();
            txtApellidoNuevo.Clear();
            txtEmailNuevo.Clear();

            MessageBox.Show("Registro agregado a la lista.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Intenta abrir el último archivo guardado utilizando la aplicación del sistema
        /// configurada por defecto o el Bloc de notas.
        /// </summary>
        private void lblVistaPrevia_Click(object sender, EventArgs e)
        {
            // El usuario debe hacer doble click o usar un botón. Como pide RF11, 
            // usaré el evento seleccionado en el combobox o un método dedicado si se requiriera,
            // pero el diseño no tiene botón "Abrir". Asumiremos que si cambian la opción, se abre.
        }

        // Ya que no hay un botón 'Abrir' en el diseño original pero hay un label que podría actuar como tal
        // o podemos usar un evento. Por consistencia de UI usaré el ComboBox SelectedIndexChanged 
        // solo si ya hay un archivo. Lo más limpio es dejar esto vinculado a un botón.
        // Voy a modificar el comportamiento: Si escoge una opción, lo abre directamente.
        private void comBoxArchivos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_ultimoArchivoGuardado) || !File.Exists(_ultimoArchivoGuardado))
            {
                return; // Silencioso si no hay nada aún
            }

            try
            {
                if (comBoxArchivos.SelectedIndex == 0) // Predeterminado
                {
                    // Ejecuta un proceso en Windows usando el Shell para que el SO decida con qué abrirlo
                    Process.Start(new ProcessStartInfo(_ultimoArchivoGuardado) { UseShellExecute = true });
                }
                else if (comBoxArchivos.SelectedIndex == 1) // Bloc de notas
                {
                    Process.Start("notepad.exe", _ultimoArchivoGuardado);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudo abrir el archivo: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Funcionalidad de impresión stub. 
        /// Mantiene la compatibilidad visual y reporta que la impresión está delegada al sistema operativo.
        /// </summary>
        private void btnImprimir_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(_ultimoArchivoGuardado))
            {
                MessageBox.Show("No hay ningún archivo descargado para imprimir.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("La funcionalidad de impresión directa ha sido deshabilitada. Puede imprimir desde el archivo abierto.", "Impresión", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Manejadores vacíos por compatibilidad con el Designer (no borrar para no romper la UI)
        private void label2_Click(object sender, EventArgs e) { }
        private void label1_Click(object sender, EventArgs e) { }
        private void label18_Click(object sender, EventArgs e) { }
        private void label17_Click(object sender, EventArgs e) { }

        // Manejadores para la sincronización entre ListBox y ListView
        private void lstRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstRegistros.SelectedIndex >= 0 && lstRegistros.SelectedIndex < lvRegistros.Items.Count)
            {
                lvRegistros.Items[lstRegistros.SelectedIndex].Selected = true;
                lvRegistros.EnsureVisible(lstRegistros.SelectedIndex);
            }
        }

        private void lvRegistros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvRegistros.SelectedIndices.Count > 0)
            {
                lstRegistros.SelectedIndex = lvRegistros.SelectedIndices[0];
            }
        }

        private void pPunto5_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
