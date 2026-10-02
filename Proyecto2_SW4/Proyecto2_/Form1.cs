using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.Agents.AI;
using System.Linq;

namespace Proyecto2_
{
    // Controlador de la UI (Capa de Presentación). Coordina eventos del usuario, renderizado del chat y gestión de la sesión de IA.
    public partial class Form1 : Form
    {
        // Motor de IA que procesará el texto ingresado.
        private AIAgent _agente;
        // Objeto que retiene el historial de la conversación (memoria) durante la sesión activa.
        private AgentSession _sesion;

        public Form1()
        {
            InitializeComponent();
            
            // Conectar eventos
            this.pbLogo.Cursor = Cursors.Hand;
            this.pbLogo.Click += PbLogo_Click;
            this.btnSend.Click += BtnSend_Click;
            this.btnSettings.Click += BtnSettings_Click;
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            FormConfig config = new FormConfig();
            config.ShowDialog();
        }

        private async void PbLogo_Click(object sender, EventArgs e)
        {
            // Abrir el formulario de información de sesión
            FormSession session = new FormSession();
            if (session.ShowDialog() == DialogResult.OK)
            {
                if (_agente != null)
                {
                    _sesion = await _agente.CreateSessionAsync();
                    MessageBox.Show("Nueva sesión iniciada.", "Orientador de Phishing", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Manejador del flujo de consulta: Valida input, controla estado visual, se comunica asíncronamente con el agente y gestiona retries ante bloqueos (rate-limits).
        private async void BtnSend_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                string userText = txtMessage.Text;
                txtMessage.Text = ""; // Limpiar input
                
                if (_agente == null)
                {
                    MessageBox.Show("El agente no se pudo inicializar. Verifica tu API Key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                btnSend.Enabled = false;
                
                // Transición a la vista de chat si es el primer mensaje
                if (!flpChat.Visible)
                {
                    TransitionToChatView();
                }

                // Añadir el mensaje del usuario al chat
                AddMessageBubble(userText, true);
                
                // Mostrar mensaje de carga
                Panel loadingBubble = AddMessageBubble("Kodu está escribiendo...", false);
                flpChat.ScrollControlIntoView(loadingBubble);

                int maxRetries = 3;
                int currentRetry = 0;
                bool success = false;

                while (currentRetry < maxRetries && !success)
                {
                    try
                    {
                        var runTask = System.Threading.Tasks.Task.Run(() => _agente.RunAsync(userText, _sesion));
                        var timeoutTask = System.Threading.Tasks.Task.Delay(60000); // 60 segundos timeout
                        
                        if (await System.Threading.Tasks.Task.WhenAny(runTask, timeoutTask) == timeoutTask)
                        {
                            throw new TimeoutException("La solicitud tardó demasiado y el servidor no respondió.");
                        }
                        
                        var response = await runTask;
                        flpChat.Controls.Remove(loadingBubble);
                        
                        // Mostrar respuesta en el chat
                        AddMessageBubble(response.Text, false);
                        
                        success = true;
                    }
                    catch (Exception ex)
                    {
                        currentRetry++;
                        if (ex.Message.Contains("high demand") && currentRetry < maxRetries)
                        {
                            loadingBubble.Controls.OfType<Label>().First().Text = $"Kodu está escribiendo... (Alta demanda, reintentando {currentRetry}/{maxRetries})";
                            await System.Threading.Tasks.Task.Delay(2000 * currentRetry);
                        }
                        else
                        {
                            flpChat.Controls.Remove(loadingBubble);
                            AddMessageBubble($"[Error de conexión: {ex.Message}]", false, true);
                            break;
                        }
                    }
                }
                btnSend.Enabled = true;
            }
        }

        // Transforma la interfaz de 'Landing Page' a vista de 'Chat' modificando el layout y anclaje de paneles (Transformación visual).
        private void TransitionToChatView()
        {
            // Ocultar elementos iniciales
            pbLogo.Visible = false;
            lblGreeting.Visible = false;
            lblSubtitle.Visible = false;
            lblCategories.Visible = false;

            // Mostrar el botón de nuevo chat
            btnNewChat.Visible = true;

            // Mover y anclar el panel de input hacia abajo
            pnlInput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlInput.Location = new Point(100, this.ClientSize.Height - 80);
            pnlInput.Width = this.ClientSize.Width - 200;

            // Mostrar y configurar el contenedor del chat
            flpChat.Visible = true;
            flpChat.Location = new Point(100, 50);
            flpChat.Size = new Size(this.ClientSize.Width - 200, this.ClientSize.Height - 150);
        }

        // Instancia un contenedor y Label dinámicamente para simular burbujas. Modifica UI en base al rol (Agente = Derecha/Azul, Usuario = Izquierda/Slate).
        private Panel AddMessageBubble(string text, bool isUser, bool isError = false)
        {
            // Panel contenedor para la burbuja
            Panel container = new Panel();
            container.Width = flpChat.ClientSize.Width - 25; // Dejar margen para la barra de desplazamiento
            container.Margin = new Padding(0, 10, 0, 10);
            container.AutoSize = true;

            // La burbuja de texto
            Label lblBubble = new Label();
            lblBubble.Text = text;
            lblBubble.Font = new Font("Segoe UI", 11F);
            lblBubble.MaximumSize = new Size((int)(container.Width * 0.8), 0); // Ocupar hasta el 80% del ancho
            lblBubble.AutoSize = true;
            lblBubble.Padding = new Padding(15);
            
            if (!isUser) // Agente (Kodu) - ahora a la derecha
            {
                if (isError)
                {
                    lblBubble.BackColor = Color.FromArgb(220, 38, 38); // Red 600
                }
                else
                {
                    lblBubble.BackColor = Color.FromArgb(37, 99, 235); // Blue 600 para Kodu
                }
                lblBubble.ForeColor = Color.White;
                // Anclar a la derecha
                lblBubble.Location = new Point(container.Width - lblBubble.PreferredWidth, 0);
                lblBubble.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            }
            else // Usuario - ahora a la izquierda
            {
                lblBubble.BackColor = Color.FromArgb(30, 41, 59); // Slate 800 para usuario
                lblBubble.ForeColor = Color.FromArgb(248, 250, 252); // Slate 50
                // Anclar a la izquierda
                lblBubble.Location = new Point(0, 0);
                lblBubble.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            }

            // Para que la burbuja se ajuste al redimensionarse si es necesario
            container.Controls.Add(lblBubble);
            
            // Reajustar la posición de la burbuja por si varió el tamaño
            if (!isUser)
            {
                lblBubble.Left = container.Width - lblBubble.Width;
            }

            flpChat.Controls.Add(container);
            
            // Auto scroll hacia abajo
            flpChat.VerticalScroll.Value = flpChat.VerticalScroll.Maximum;
            flpChat.PerformLayout();
            
            return container;
        }

        // Orquestador de inicio: Instancia el agente quemado (Hardcodeado) vía Factory y crea la primera sesión asíncrona.
        private async void Form1_Load(object sender, EventArgs e)
        {
            // Set double buffering to avoid flickering
            this.DoubleBuffered = true;
            try 
            {
                _agente = AgentFactory.Crear("Gemini", "Kodu", "Eres Kodu, un Orientador de señales de phishing. Ayuda al usuario a analizar correos electrónicos sospechosos identificando indicadores de riesgo como remitentes extraños, enlaces engañosos, urgencia injustificada y archivos adjuntos peligrosos. Haz preguntas sobre el contenido del correo sin pedirles que hagan clic en nada. Proporciona una lista de indicadores de riesgo detectados. No abras enlaces ni verifiques su contenido real. No inventes datos.");
                
                _sesion = await _agente.CreateSessionAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error de inicialización", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Evita el sonido de "ding" al presionar Enter en un textbox simple
                if (btnSend.Enabled) // Asegurarse de que no se envíe si ya se está cargando
                {
                    btnSend.PerformClick(); // Simular clic en el botón Enviar
                }
            }
        }
        private void lblGreeting_Click(object sender, EventArgs e)
        {

        }

        // Resetea el estado gráfico completo de la vista y destruye la memoria del chat anterior regenerando la sesión.
        private async void btnNewChat_Click(object sender, EventArgs e)
        {
            // Ocultar chat y ocultar botón de nuevo chat
            flpChat.Visible = false;
            btnNewChat.Visible = false;

            // Limpiar contenido del chat
            flpChat.Controls.Clear();

            // Restaurar estado inicial de la UI
            pbLogo.Visible = true;
            lblGreeting.Visible = true;
            lblSubtitle.Visible = true;
            lblCategories.Visible = true;
            
            lblSubtitle.Text = "Pega ese correo dudoso y descubriré si intentan engañarte.";
            txtMessage.Text = "";
            txtMessage.ForeColor = Color.FromArgb(203, 213, 225);

            // Devolver panel de input al centro
            pnlInput.Anchor = AnchorStyles.None;
            pnlInput.Location = new Point(100, 360);
            pnlInput.Width = 600;

            // Generar nueva sesión para limpiar el historial del agente
            if (_agente != null)
            {
                _sesion = await _agente.CreateSessionAsync();
            }
        }
    }
}
