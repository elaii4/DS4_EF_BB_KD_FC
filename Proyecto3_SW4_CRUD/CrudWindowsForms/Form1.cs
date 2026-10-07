using System;
using System.Windows.Forms;

namespace CrudWindowsForms
{
    public partial class Form1 : Form
    {
        private readonly PeopleDB db = new PeopleDB();

        public Form1()
        {
            InitializeComponent();
        }

        // Carga la lista desde la base de datos y la muestra en la tabla.
        private void RefreshPeople()
        {
            try
            {
                dgvPeople.AutoGenerateColumns = true;
                dgvPeople.DataSource = db.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudieron cargar los registros." + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                db.TestConnection();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo conectar con SQL Server. Revise el nombre del servidor en PeopleDB.cs." +
                    Environment.NewLine + ex.Message,
                    "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            RefreshPeople();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            RefreshPeople();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (FrmNuevo form = new FrmNuevo())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshPeople();
                }
            }
        }

        // Devuelve el Id de la persona seleccionada, o null si no hay selección.
        // Usa DataBoundItem (el objeto People), NO el número de fila.
        private int? GetSelectedId()
        {
            if (dgvPeople.SelectedRows.Count == 0)
            {
                return null;
            }

            People person = dgvPeople.SelectedRows[0].DataBoundItem as People;
            if (person == null)
            {
                return null;
            }

            return person.Id;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null)
            {
                MessageBox.Show("Seleccione una persona de la lista para editar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (FrmNuevo form = new FrmNuevo(id))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    RefreshPeople();
                }
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (id == null)
            {
                MessageBox.Show("Seleccione una persona de la lista para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult answer = MessageBox.Show("¿Está seguro de que desea eliminar el registro seleccionado?",
                "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer != DialogResult.Yes)
            {
                return;
            }

            try
            {
                if (!db.Delete(id.Value))
                {
                    MessageBox.Show("El registro ya no existe.",
                        "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                RefreshPeople();
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo eliminar el registro." + Environment.NewLine + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
