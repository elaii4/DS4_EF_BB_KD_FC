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
    public partial class FrmPersona : Form
    {
        private readonly PeopleLinqDB db = new PeopleLinqDB();
        private readonly int? id;

        public FrmPersona(int? id = null)
        {
            InitializeComponent();
            this.id = id;
            Text = id.HasValue ? "Editar persona" : "Nueva persona";
        }

        private void FrmPersona_Load(object sender, EventArgs e)
        {
            if (!id.HasValue)
                return;

            try
            {
                Person person = db.GetById(id.Value);
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

        private bool ValidateInput(out string name, out int age)
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string name;
            int age;
            if (!ValidateInput(out name, out age))
                return;

            try
            {
                if (id.HasValue)
                {
                    if (!db.Update(id.Value, name, age))
                    {
                        MessageBox.Show("El registro ya no existe. Actualice la lista.");
                        return;
                    }
                }
                else
                {
                    int newId = db.Add(name, age);
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