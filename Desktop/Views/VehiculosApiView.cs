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
    public partial class VehiculosApiView : Form
    {
        VehiculosApiService vehiculosService = new VehiculosApiService();
        UsuariosApiService usuariosService = new UsuariosApiService();

        Vehiculo vehiculoModificado;
        public VehiculosApiView()
        {
            InitializeComponent();
            _ = LoadVehiculos();
            _ = LoadComboUsuarios();

        }

        private async Task LoadComboUsuarios()
        {
            var usuarios = await usuariosService.GetAllAsync();
            if (usuarios != null)
            {
                comboUsuarios.DataSource = usuarios;
                comboUsuarios.ValueMember = "Id";
                comboUsuarios.SelectedIndex = -1;
            }
        }

        private async Task LoadVehiculos()
        {
            var vehiculos = await vehiculosService.GetAllAsync();
            if (vehiculos != null)
            {
                dataGridVehiculos.DataSource = vehiculos;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var vehiculos = await vehiculosService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (vehiculos != null)
            {
                dataGridVehiculos.DataSource = vehiculos;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Vehiculo vehiculo = new Vehiculo
            {
                Patente = txtPatente.Text,
                Marca = txtMarca.Text,
                Modelo = txtModelo.Text,
                Tipo = comboTipo.Text,
                Anio = (int)numAnio.Value,
                UsuarioId = comboUsuarios.SelectedValue != null ? (int)comboUsuarios.SelectedValue : 0
            };
            bool vehiculoGuardado;
            if (vehiculoModificado == null)
            {
                vehiculoGuardado = await vehiculosService.AddVehiculoAsync(vehiculo);
            }
            else
            {
                vehiculo.Id = vehiculoModificado.Id;
                vehiculoGuardado = await vehiculosService.UpdateVehiculoAsync(vehiculo);
            }
            if (!vehiculoGuardado)
            {
                MessageBox.Show("Error al guardar el vehículo");
                return;
            }
            MessageBox.Show("Vehículo guardado correctamente");
            await LoadVehiculos();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            vehiculoModificado = null;

        }

        private void ClearTextBox()
        {
            txtPatente.Clear();
            txtMarca.Clear();
            txtModelo.Clear();
            comboTipo.SelectedIndex = -1;
            numAnio.Value = 2020;
            comboUsuarios.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            vehiculoModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el vehículo seleccionado en la grilla
            if (dataGridVehiculos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un vehículo para modificar");
                return;
            }
            vehiculoModificado = (Vehiculo)dataGridVehiculos.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de vehículo seleccionado
            txtPatente.Text = vehiculoModificado.Patente;
            txtMarca.Text = vehiculoModificado.Marca;
            txtModelo.Text = vehiculoModificado.Modelo;
            comboTipo.SelectedItem = vehiculoModificado.Tipo;
            numAnio.Value = vehiculoModificado.Anio;
            if (vehiculoModificado.UsuarioId != 0)
                comboUsuarios.SelectedValue = vehiculoModificado.UsuarioId;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
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
            //capturamos el vehículo seleccionado en la grilla
            if (dataGridVehiculos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un vehículo para eliminar");
                return;
            }

            var vehiculoAEliminar = (Vehiculo)dataGridVehiculos.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el vehículo
            var result = MessageBox.Show($"¿Está seguro de eliminar el vehículo {vehiculoAEliminar.Patente}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el vehículo
                var vehiculoEliminado = await vehiculosService.DeleteVehiculoAsync(vehiculoAEliminar.Id);
                if (!vehiculoEliminado)
                {
                    MessageBox.Show("Error al eliminar el vehículo");
                    return;
                }
                MessageBox.Show($"Vehículo {vehiculoAEliminar.Patente} eliminado correctamente");
                await LoadVehiculos();
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
                await LoadVehiculos();
            }
        }

        private async Task LoadDeleteds()
        {
            var vehiculos = await vehiculosService.GetDeletedsAsync();
            if (vehiculos != null)
            {
                dataGridVehiculos.DataSource = vehiculos;
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el vehículo seleccionado en la grilla
            if (dataGridVehiculos.CurrentRow != null)
            {
                var vehiculoARestaurar = (Vehiculo)dataGridVehiculos.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el vehículo
                var result = MessageBox.Show($"¿Está seguro de restaurar el vehículo {vehiculoARestaurar.Patente}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el vehículo
                    var vehiculoRestaurado = await vehiculosService.RestoreVehiculoAsync(vehiculoARestaurar.Id);
                    if (vehiculoRestaurado)
                    {
                        MessageBox.Show($"Vehículo {vehiculoARestaurar.Patente} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el vehículo");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un vehículo para restaurar");
            }
        }
    }
}
