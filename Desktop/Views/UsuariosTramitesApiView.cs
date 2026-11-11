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
    public partial class ClientesTramitesApiView : Form
    {
        ClientesTramitesApiService clientesTramitesService = new ClientesTramitesApiService();
        ClientesApiService clientesService = new ClientesApiService();
        TramitesApiService tramitesService = new TramitesApiService();

        ClienteTramite clienteTramiteModificado;
        public ClientesTramitesApiView()
        {
            InitializeComponent();
            _ = LoadClientesTramites();
            _ = LoadComboClientes();
            _ = LoadComboTramites();

        }

        private async Task LoadComboClientes()
        {
            var clientes = await clientesService.GetAllAsync();
            if (clientes != null)
            {
                comboClientes.DataSource = clientes;
                comboClientes.ValueMember = "Id";
                comboClientes.SelectedIndex = -1;
            }
        }

        private async Task LoadComboTramites()
        {
            var tramites = await tramitesService.GetAllAsync();
            if (tramites != null)
            {
                comboTramites.DataSource = tramites;
                comboTramites.ValueMember = "Id";
                comboTramites.SelectedIndex = -1;
            }
        }

        private async Task LoadClientesTramites()
        {
            var clientesTramites = await clientesTramitesService.GetAllAsync();
            if (clientesTramites != null)
            {
                dataGridClientesTramites.DataSource = clientesTramites;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var clientesTramites = await clientesTramitesService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (clientesTramites != null)
            {
                dataGridClientesTramites.DataSource = clientesTramites;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            ClienteTramite clienteTramite = new ClienteTramite
            {
                ClienteId = comboClientes.SelectedValue != null ? (int)comboClientes.SelectedValue : 0,
                TramiteId = comboTramites.SelectedValue != null ? (int)comboTramites.SelectedValue : 0,
                RolCliente = comboRolCliente.Text
            };
            bool clienteTramiteGuardado;
            if (clienteTramiteModificado == null)
            {
                clienteTramiteGuardado = await clientesTramitesService.AddClienteTramiteAsync(clienteTramite);
            }
            else
            {
                clienteTramite.Id = clienteTramiteModificado.Id;
                clienteTramiteGuardado = await clientesTramitesService.UpdateClienteTramiteAsync(clienteTramite);
            }
            if (!clienteTramiteGuardado)
            {
                MessageBox.Show("Error al guardar el cliente del trámite");
                return;
            }
            MessageBox.Show("Cliente del trámite guardado correctamente");
            await LoadClientesTramites();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            clienteTramiteModificado = null;

        }

        private void ClearTextBox()
        {
            comboClientes.SelectedIndex = -1;
            comboTramites.SelectedIndex = -1;
            comboRolCliente.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            clienteTramiteModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el cliente del trámite seleccionado en la grilla
            if (dataGridClientesTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un cliente del trámite para modificar");
                return;
            }
            clienteTramiteModificado = (ClienteTramite)dataGridClientesTramites.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de cliente del trámite seleccionado
            if (clienteTramiteModificado.ClienteId != 0)
                comboClientes.SelectedValue = clienteTramiteModificado.ClienteId;
            if (clienteTramiteModificado.TramiteId != 0)
                comboTramites.SelectedValue = clienteTramiteModificado.TramiteId;
            comboRolCliente.SelectedItem = clienteTramiteModificado.RolCliente;
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
            //capturamos el cliente del trámite seleccionado en la grilla
            if (dataGridClientesTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un cliente del trámite para eliminar");
                return;
            }

            var clienteTramiteAEliminar = (ClienteTramite)dataGridClientesTramites.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el cliente del trámite
            var result = MessageBox.Show($"¿Está seguro de eliminar al cliente del trámite {clienteTramiteAEliminar.Cliente} ({clienteTramiteAEliminar.RolCliente})?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el cliente del trámite
                var clienteTramiteEliminado = await clientesTramitesService.DeleteClienteTramiteAsync(clienteTramiteAEliminar.Id);
                if (!clienteTramiteEliminado)
                {
                    MessageBox.Show("Error al eliminar el cliente del trámite");
                    return;
                }
                MessageBox.Show($"Cliente del trámite {clienteTramiteAEliminar.Cliente} ({clienteTramiteAEliminar.RolCliente}) eliminado correctamente");
                await LoadClientesTramites();
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
                await LoadClientesTramites();
            }
        }

        private async Task LoadDeleteds()
        {
            var clientesTramites = await clientesTramitesService.GetDeletedsAsync();
            if (clientesTramites != null)
            {
                dataGridClientesTramites.DataSource = clientesTramites;
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el cliente del trámite seleccionado en la grilla
            if (dataGridClientesTramites.CurrentRow != null)
            {
                var clienteTramiteARestaurar = (ClienteTramite)dataGridClientesTramites.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el cliente del trámite
                var result = MessageBox.Show($"¿Está seguro de restaurar al cliente del trámite {clienteTramiteARestaurar.Cliente} ({clienteTramiteARestaurar.RolCliente})?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el cliente del trámite
                    var clienteTramiteRestaurado = await clientesTramitesService.RestoreClienteTramiteAsync(clienteTramiteARestaurar.Id);
                    if (clienteTramiteRestaurado)
                    {
                        MessageBox.Show($"Cliente del trámite {clienteTramiteARestaurar.Cliente} ({clienteTramiteARestaurar.RolCliente}) restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el cliente del trámite");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un cliente del trámite para restaurar");
            }
        }
    }
}
