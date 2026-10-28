using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrudWindowsFormsLinq
{
    // FrmPersona
    // Ventana modal para captura y edición de datos de una persona.
    // Valida entradas del usuario y envía peticiones de guardado a PeopleApiClient.
    public partial class FrmPersona : Form
    {
        private readonly PeopleApiClient db = new PeopleApiClient();
        private readonly int? id;

        public FrmPersona(int? id = null)
        {
            InitializeComponent();
            this.id = id;
            Text = id.HasValue ? "Editar persona" : "Nueva persona";
        }

        private async void FrmPersona_Load(object sender, EventArgs e) // Carga datos existentes al editar una persona
        {
            if (!id.HasValue)
                return;

            try
            {
                Person person = await db.GetByIdAsync(id.Value);
                if (person == null)
                {
                    MessageBox.Show("El registro ya no existe.");
                    DialogResult = DialogResult.Cancel;
                    Close();
                    return;
                }
                txtName.Text = person.Name;
                txtEdad.Text = person.Age.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar: " + ex.Message);
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }

        private bool ValidateInput(out string name, out int age) // Valida formato de texto y rango numérico
        {
            name = txtName.Text.Trim();
            age = 0;
            if (string.IsNullOrWhiteSpace(name) || name.Length > 100)
            {
                MessageBox.Show("Escriba un nombre de 1 a 100 caracteres.");
                txtName.Focus();
                return false;
            }
            if (!int.TryParse(txtEdad.Text.Trim(), out age) || age < 0 || age > 120)
            {
                MessageBox.Show("Escriba una edad entera entre 0 y 120.");
                txtEdad.Focus();
                return false;
            }
            return true;
        }

        private async void btnGuardar_Click(object sender, EventArgs e) // Ejecuta creación o actualización según corresponda
        {
            string name;
            int age;
            if (!ValidateInput(out name, out age))
                return;

            try
            {
                if (id.HasValue)
                {
                    bool ok = await db.UpdateAsync(id.Value, name, age);
                    if (!ok)
                    {
                        MessageBox.Show("El registro ya no existe. Actualice la lista.");
                        return;
                    }
                }
                else
                {
                    int newId = await db.AddAsync(name, age);
                    MessageBox.Show("Registro creado con Id " + newId);
                }
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblNombre_Click(object sender, EventArgs e)
        {
        }

        private void lblEdad_Click(object sender, EventArgs e)
        {
        }

        private void txtEdad_TextChanged(object sender, EventArgs e)
        {
        }

        private void lblSubtitulo1_Click(object sender, EventArgs e)
        {
        }

        private void lblSubtitulo2_Click(object sender, EventArgs e)
        {
        }

        private void pbIcono_Click(object sender, EventArgs e)
        {
        }

        private void lblTitulo_Click(object sender, EventArgs e)
        {
        }

        private void pnlPrincipal_Paint(object sender, PaintEventArgs e)
        {
        }

        private void pnlEncabezado_Paint(object sender, PaintEventArgs e)
        {
        }
    }
}