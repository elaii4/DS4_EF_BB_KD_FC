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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly PeopleLinqDB db = new PeopleLinqDB();

        private void RefreshPeople()
        {
            try
            {
                dgvPeople.DataSource = db.Get();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshPeople();
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            RefreshPeople();
        }

        private int? GetSelectedId()
        {
            if (dgvPeople.CurrentRow == null)
                return null;

            Person person = dgvPeople.CurrentRow.DataBoundItem as Person;
            return person == null ? (int?)null : person.Id;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            using (FrmPersona form = new FrmPersona())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    RefreshPeople();
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (!id.HasValue)
            {
                MessageBox.Show("Seleccione una persona.");
                return;
            }
            using (FrmPersona form = new FrmPersona(id.Value))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    RefreshPeople();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            int? id = GetSelectedId();
            if (!id.HasValue)
            {
                MessageBox.Show("Seleccione una persona.");
                return;
            }

            DialogResult answer = MessageBox.Show(
                "¿Eliminar la persona con Id " + id.Value + "?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (answer != DialogResult.Yes)
                return;

            try
            {
                if (!db.Delete(id.Value))
                    MessageBox.Show("El registro ya no existe.");
                RefreshPeople();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            try
            {
                string text = txtBuscar.Text.Trim();
                dgvPeople.DataSource = string.IsNullOrWhiteSpace(text)
                    ? db.Get()
                    : db.Search(text);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al buscar: " + ex.Message);
            }
        }
        private void dgvPeople_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
        }

        private void tbNombreLINQ_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
