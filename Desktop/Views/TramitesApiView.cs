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
    public partial class TramitesApiView : Form
    {
        TramitesApiService tramitesService = new TramitesApiService();
        VehiculosApiService vehiculosService = new VehiculosApiService();

        Tramite tramiteModificado;
        public TramitesApiView()
        {
            InitializeComponent();
            _ = LoadTramites();
            _ = LoadComboVehiculos();

        }

        private async Task LoadComboVehiculos()
        {
            var vehiculos = await vehiculosService.GetAllAsync();
            if (vehiculos != null)
            {
                comboVehiculos.DataSource = vehiculos;
                comboVehiculos.DisplayMember = "Patente";
                comboVehiculos.ValueMember = "Id";
                comboVehiculos.SelectedIndex = -1;
            }
        }

        private async Task LoadTramites()
        {
            var tramites = await tramitesService.GetAllAsync();
            if (tramites != null)
            {
                dataGridTramites.DataSource = tramites;
                OcultarColumnas();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var tramites = await tramitesService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (tramites != null)
            {
                dataGridTramites.DataSource = tramites;
                OcultarColumnas();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Tramite tramite = new Tramite
            {
                TipoTramite = txtTipoTramite.Text,
                Estado = comboEstado.Text,
                VehiculoId = comboVehiculos.SelectedValue != null ? (int)comboVehiculos.SelectedValue : 0
            };
            bool tramiteGuardado;
            if (tramiteModificado == null)
            {
                tramiteGuardado = await tramitesService.AddTramiteAsync(tramite);
            }
            else
            {
                tramite.Id = tramiteModificado.Id;
                tramiteGuardado = await tramitesService.UpdateTramiteAsync(tramite);
            }
            if (!tramiteGuardado)
            {
                MessageBox.Show("Error al guardar el trámite");
                return;
            }
            MessageBox.Show("Trámite guardado correctamente");
            await LoadTramites();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            tramiteModificado = null;

        }

        private void ClearTextBox()
        {
            txtTipoTramite.Text = "";
            comboEstado.SelectedIndex = -1;
            comboVehiculos.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            tramiteModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el trámite seleccionado en la grilla
            if (dataGridTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un trámite para modificar");
                return;
            }
            tramiteModificado = (Tramite)dataGridTramites.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de trámite seleccionado
            txtTipoTramite.Text = tramiteModificado.TipoTramite;
            comboEstado.SelectedItem = tramiteModificado.Estado;
            if (tramiteModificado.VehiculoId != 0)
                comboVehiculos.SelectedValue = tramiteModificado.VehiculoId;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void OcultarColumnas()
        {
            // Ocultar las columnas que no queremos mostrar
            if (dataGridTramites.Columns["VehiculoId"] != null)
                dataGridTramites.Columns["VehiculoId"].Visible = false;

            if (dataGridTramites.Columns["IsDeleted"] != null)
                dataGridTramites.Columns["IsDeleted"].Visible = false;
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
            //capturamos el trámite seleccionado en la grilla
            if (dataGridTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un trámite para eliminar");
                return;
            }

            var tramiteAEliminar = (Tramite)dataGridTramites.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el trámite
            var result = MessageBox.Show($"¿Está seguro de eliminar el trámite {tramiteAEliminar.TipoTramite}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el trámite
                var tramiteEliminado = await tramitesService.DeleteTramiteAsync(tramiteAEliminar.Id);
                if (!tramiteEliminado)
                {
                    MessageBox.Show("Error al eliminar el trámite");
                    return;
                }
                MessageBox.Show($"Trámite {tramiteAEliminar.TipoTramite} eliminado correctamente");
                await LoadTramites();
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
                await LoadTramites();
            }
        }

        private async Task LoadDeleteds()
        {
            var tramites = await tramitesService.GetDeletedsAsync();
            if (tramites != null)
            {
                dataGridTramites.DataSource = tramites;
                OcultarColumnas();
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el trámite seleccionado en la grilla
            if (dataGridTramites.CurrentRow != null)
            {
                var tramiteARestaurar = (Tramite)dataGridTramites.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el trámite
                var result = MessageBox.Show($"¿Está seguro de restaurar el trámite {tramiteARestaurar.TipoTramite}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el trámite
                    var tramiteRestaurado = await tramitesService.RestoreTramiteAsync(tramiteARestaurar.Id);
                    if (tramiteRestaurado)
                    {
                        MessageBox.Show($"Trámite {tramiteARestaurar.TipoTramite} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el trámite");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un trámite para restaurar");
            }
        }
    }
}
