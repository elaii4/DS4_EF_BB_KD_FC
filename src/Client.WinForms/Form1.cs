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
    // Form1
    // Formulario principal que muestra la grilla interactiva y botones de acción.
    // Coordina la visualización, búsqueda, inserción, edición y eliminación de personas.
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private readonly PeopleApiClient db = new PeopleApiClient();

        private async Task RefreshPeopleAsync() // Recarga la lista desde la API asíncronamente
        {
            try
            {
                dgvPeople.DataSource = await db.GetAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al consultar: " + ex.Message);
            }
        }

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

        private async void Form1_Load(object sender, EventArgs e)
        {
            await RefreshPeopleAsync();
        }

        private async void btnActualizar_Click(object sender, EventArgs e)
        {
            txtBuscar.Clear();
            await RefreshPeopleAsync();
        }

        private int? GetSelectedId() // Obtiene el identificador de la fila seleccionada
        {
            if (dgvPeople.CurrentRow == null)
                return null;

            Person person = dgvPeople.CurrentRow.DataBoundItem as Person;
            return person == null ? (int?)null : person.Id;
        }

        private async void btnNuevo_Click(object sender, EventArgs e) // Abre ventana modal para crear nueva persona
        {
            using (FrmPersona form = new FrmPersona())
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                    await RefreshPeopleAsync();
            }
        }

        private async void btnEditar_Click(object sender, EventArgs e) // Abre ventana modal para editar persona seleccionada
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
                    await RefreshPeopleAsync();
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e) // Solicita confirmación y elimina la persona seleccionada
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
                if (!await db.DeleteAsync(id.Value))
                    MessageBox.Show("El registro ya no existe.");
                await RefreshPeopleAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e) // Filtra registros por texto ingresado en búsqueda
        {
            try
            {
                string text = txtBuscar.Text.Trim();
                dgvPeople.DataSource = string.IsNullOrWhiteSpace(text)
                    ? await db.GetAsync()
                    : await db.SearchAsync(text);
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
