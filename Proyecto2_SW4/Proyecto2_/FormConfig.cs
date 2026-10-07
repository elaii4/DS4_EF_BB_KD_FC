using System;
using System.Windows.Forms;

namespace Proyecto2_
{
    // Módulo de configuración: Recolecta parámetros del usuario para definir el comportamiento y proveedor del Agente (Aún no implementa el retorno de datos al Form1).
    public partial class FormConfig : Form
    {
        public FormConfig()
        {
            InitializeComponent();
            
            this.cmbProvider.SelectedIndex = 2; // Gemini por defecto
            
            this.btnCreate.Click += BtnCreate_Click;
            this.btnCancel.Click += BtnCancel_Click;
        }

        // Simula la persistencia de configuración. Actualmente solo cierra el modal (Falta implementar paso de parámetros al controlador principal).
        private void BtnCreate_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Configuración guardada exitosamente en memoria.", "Orientador - Configuración", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
