using System;
using System.Windows.Forms;

namespace Proyecto2_
{
    // Pantalla de estado: Muestra metadatos informativos sobre la sesión activa y expone acciones de reseteo.
    public partial class FormSession : Form
    {
        public FormSession()
        {
            InitializeComponent();

            this.btnConfig.Click += BtnConfig_Click;
            this.btnNewSession.Click += BtnNewSession_Click;
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {
            FormConfig config = new FormConfig();
            config.ShowDialog();
        }

        // Confirma la acción de destrucción y reinicio al controlador principal enviando un DialogResult.OK.
        private void BtnNewSession_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnConfig_Click_1(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }
    }
}
