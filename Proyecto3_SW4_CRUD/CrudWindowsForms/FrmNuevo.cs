using System;
using System.Windows.Forms;

namespace CrudWindowsForms
{
    public partial class FrmNuevo : Form
    {
        private readonly PeopleDB db = new PeopleDB();

        // null = insertar, con valor = editar esa persona.
        private readonly int? id;

        public FrmNuevo(int? id = null)
        {
            InitializeComponent();
            this.id = id;
        }

        // Valida nombre y edad. Si todo es correcto devuelve true
        // y entrega los valores ya limpios en name y age.
        private bool ValidateInput(out string name, out int age)
        {
            name = txtName.Text.Trim();
            age = 0;

            if (name.Length < 1 || name.Length > 100)
            {
                MessageBox.Show("Escriba un nombre de 1 a 100 caracteres.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (!int.TryParse(txtEdad.Text.Trim(), out age) || age < 0 || age > 120)
            {
                MessageBox.Show("Escriba una edad entera entre 0 y 120.",
                    "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtEdad.Focus();
                return false;
            }

            return true;
        }

        private void FrmNuevo_Load(object sender, EventArgs e)
        {
            if (!id.HasValue)
            {
                this.Text = "Nueva persona";
                return;
            }

            this.Text = "Editar persona";
            lblTitulo.Text = "Editar persona";
            lblSubtitulo1.Text = "Modifica los datos de";
            lblSubtitulo2.Text = "la persona seleccionada.";

            try
            {
                People person = db.GetById(id.Value);
                if (person == null)
                {
                    MessageBox.Show("El registro ya no existe.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.Cancel;
                    BeginInvoke(new MethodInvoker(Close));
                    return;
                }

                txtName.Text = person.Name;
                txtEdad.Text = person.Age.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo cargar el registro." + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
                BeginInvoke(new MethodInvoker(Close));
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            string name;
            int age;

            if (!ValidateInput(out name, out age))
            {
                return;
            }

            try
            {
                People person = new People();
                person.Name = name;
                person.Age = age;

                if (id.HasValue)
                {
                    person.Id = id.Value;
                    if (!db.Update(person))
                    {
                        MessageBox.Show("El registro ya no existe.",
                            "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    db.Add(person);
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo guardar el registro." + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
