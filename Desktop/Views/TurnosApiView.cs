using Desktop.Services;
using Services.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Desktop.Views
{
    public partial class TurnosApiView : Form
    {
        TurnosApiService turnosService = new TurnosApiService();
        TramitesApiService tramitesService = new TramitesApiService();
        EmpleadosApiService empleadosService = new EmpleadosApiService();

        Turno turnoModificado;
        public TurnosApiView()
        {
            InitializeComponent();
            _ = LoadTurnos();
            _ = LoadComboTramites();
            _ = LoadComboEmpleados();

        }

        private async Task LoadComboTramites()
        {
            var tramites = await tramitesService.GetAllAsync();
            if (tramites != null)
            {
                //armamos una lista con el Id y el texto que se va a ver en el combo
                comboTramites.DataSource = tramites.Select(t => new { t.Id, Descripcion = t.ToString() }).ToList();
                comboTramites.DisplayMember = "Descripcion";
                comboTramites.ValueMember = "Id";
                comboTramites.SelectedIndex = -1;
            }
        }

        private async Task LoadComboEmpleados()
        {
            var empleados = await empleadosService.GetAllAsync();
            if (empleados != null)
            {
                //armamos una lista con el Id y el texto que se va a ver en el combo
                comboEmpleados.DataSource = empleados.Select(e => new { e.Id, Descripcion = e.ToString() }).ToList();
                comboEmpleados.DisplayMember = "Descripcion";
                comboEmpleados.ValueMember = "Id";
                comboEmpleados.SelectedIndex = -1;
            }
        }

        private async Task LoadTurnos()
        {
            var turnos = await turnosService.GetAllAsync();
            if (turnos != null)
            {
                dataGridTurnos.DataSource = turnos;
                OcultarColumnas();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var turnos = await turnosService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (turnos != null)
            {
                dataGridTurnos.DataSource = turnos;
                OcultarColumnas();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Turno turno = new Turno
            {
                Fecha = DateOnly.FromDateTime(dtpFecha.Value),
                Hora = TimeOnly.FromDateTime(dtpHora.Value),
                Estado = comboEstado.Text,
                TramiteId = comboTramites.SelectedValue != null ? (int)comboTramites.SelectedValue : 0,
                EmpleadoId = comboEmpleados.SelectedValue != null ? (int)comboEmpleados.SelectedValue : 0
            };
            bool turnoGuardado;
            if (turnoModificado == null)
            {
                turnoGuardado = await turnosService.AddTurnoAsync(turno);
            }
            else
            {
                turno.Id = turnoModificado.Id;
                turnoGuardado = await turnosService.UpdateTurnoAsync(turno);
            }
            if (!turnoGuardado)
            {
                MessageBox.Show("Error al guardar el turno");
                return;
            }
            MessageBox.Show("Turno guardado correctamente");
            await LoadTurnos();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            turnoModificado = null;

        }

        private void ClearTextBox()
        {
            dtpFecha.Value = DateTime.Today;
            dtpHora.Value = DateTime.Now;
            comboEstado.SelectedIndex = -1;
            comboTramites.SelectedIndex = -1;
            comboEmpleados.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            turnoModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el turno seleccionado en la grilla
            if (dataGridTurnos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un turno para modificar");
                return;
            }
            turnoModificado = (Turno)dataGridTurnos.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de turno seleccionado
            dtpFecha.Value = turnoModificado.Fecha.ToDateTime(TimeOnly.MinValue);
            dtpHora.Value = DateTime.Today.Add(turnoModificado.Hora.ToTimeSpan());
            comboEstado.SelectedItem = turnoModificado.Estado;
            if (turnoModificado.TramiteId != 0)
                comboTramites.SelectedValue = turnoModificado.TramiteId;
            if (turnoModificado.EmpleadoId != 0)
                comboEmpleados.SelectedValue = turnoModificado.EmpleadoId;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void OcultarColumnas()
        {
            // Ocultar las columnas que no queremos mostrar
            if (dataGridTurnos.Columns["TramiteId"] != null)
                dataGridTurnos.Columns["TramiteId"].Visible = false;

            if (dataGridTurnos.Columns["EmpleadoId"] != null)
                dataGridTurnos.Columns["EmpleadoId"].Visible = false;

            if (dataGridTurnos.Columns["IsDeleted"] != null)
                dataGridTurnos.Columns["IsDeleted"].Visible = false;
        }

        private void txtBusqueda_TextChanged(object sender, EventArgs e)
        {
            // si se borra el texto de búsqueda, se vuelven a mostrar todos
            if (string.IsNullOrWhiteSpace(txtBusqueda.Text))
            {
                btnBuscar.PerformClick();
            }
        }

        private void txtBusqueda_KeyPress(object sender, KeyPressEventArgs e)
        {
            //chequeamos si la tecla presionada es Enter y pulsamos el botón de buscar
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnBuscar.PerformClick();
                e.Handled = true; // Evita que el sonido de "ding" se reproduzca
            }
        }

        private async void btnEliminar_Click(object sender, EventArgs e)
        {
            //capturamos el turno seleccionado en la grilla
            if (dataGridTurnos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un turno para eliminar");
                return;
            }

            var turnoAEliminar = (Turno)dataGridTurnos.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el turno
            var result = MessageBox.Show($"¿Está seguro de eliminar el turno {turnoAEliminar.Fecha} {turnoAEliminar.Hora}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el turno
                var turnoEliminado = await turnosService.DeleteTurnoAsync(turnoAEliminar.Id);
                if (!turnoEliminado)
                {
                    MessageBox.Show("Error al eliminar el turno");
                    return;
                }
                MessageBox.Show($"Turno {turnoAEliminar.Fecha} {turnoAEliminar.Hora} eliminado correctamente");
                await LoadTurnos();
            }
        }

        private async void checkEliminado_CheckedChanged(object sender, EventArgs e)
        {
            txtBusqueda.Enabled = !checkEliminado.Checked;
            btnBuscar.Enabled = !checkEliminado.Checked;
            btnNuevo.Enabled = !checkEliminado.Checked;
            btnModificar.Enabled = !checkEliminado.Checked;
            btnEliminar.Enabled = !checkEliminado.Checked;
            btnRestaurar.Enabled = checkEliminado.Checked;
            if (checkEliminado.Checked)
            {
                await LoadDeleteds();
            }
            else
            {
                await LoadTurnos();
            }
        }

        private async Task LoadDeleteds()
        {
            var turnos = await turnosService.GetDeletedsAsync();
            if (turnos != null)
            {
                dataGridTurnos.DataSource = turnos;
                OcultarColumnas();
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el turno seleccionado en la grilla
            if (dataGridTurnos.CurrentRow != null)
            {
                var turnoARestaurar = (Turno)dataGridTurnos.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el turno
                var result = MessageBox.Show($"¿Está seguro de restaurar el turno {turnoARestaurar.Fecha} {turnoARestaurar.Hora}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el turno
                    var turnoRestaurado = await turnosService.RestoreTurnoAsync(turnoARestaurar.Id);
                    if (turnoRestaurado)
                    {
                        MessageBox.Show($"Turno {turnoARestaurar.Fecha} {turnoARestaurar.Hora} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el turno");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un turno para restaurar");
            }
        }
    }
}
