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
    public partial class UsuariosTramitesApiView : Form
    {
        UsuariosTramitesApiService usuariosTramitesService = new UsuariosTramitesApiService();
        UsuariosApiService usuariosService = new UsuariosApiService();
        TramitesApiService tramitesService = new TramitesApiService();

        UsuarioTramite usuarioTramiteModificado;
        public UsuariosTramitesApiView()
        {
            InitializeComponent();
            _ = LoadUsuariosTramites();
            _ = LoadComboUsuarios();
            _ = LoadComboTramites();

        }

        private async Task LoadComboUsuarios()
        {
            var usuarios = await usuariosService.GetAllAsync();
            if (usuarios != null)
            {
                //armamos una lista con el Id y el texto que se va a ver en el combo
                comboUsuarios.DataSource = usuarios.Select(u => new { u.Id, Descripcion = u.ToString() }).ToList();
                comboUsuarios.DisplayMember = "Descripcion";
                comboUsuarios.ValueMember = "Id";
                comboUsuarios.SelectedIndex = -1;
            }
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

        private async Task LoadUsuariosTramites()
        {
            var usuariosTramites = await usuariosTramitesService.GetAllAsync();
            if (usuariosTramites != null)
            {
                dataGridUsuariosTramites.DataSource = usuariosTramites;
                OcultarColumnas();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var usuariosTramites = await usuariosTramitesService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (usuariosTramites != null)
            {
                dataGridUsuariosTramites.DataSource = usuariosTramites;
                OcultarColumnas();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            UsuarioTramite usuarioTramite = new UsuarioTramite
            {
                UsuarioId = comboUsuarios.SelectedValue != null ? (int)comboUsuarios.SelectedValue : 0,
                TramiteId = comboTramites.SelectedValue != null ? (int)comboTramites.SelectedValue : 0,
                RolUsuario = comboRolUsuario.Text
            };
            bool usuarioTramiteGuardado;
            if (usuarioTramiteModificado == null)
            {
                usuarioTramiteGuardado = await usuariosTramitesService.AddUsuarioTramiteAsync(usuarioTramite);
            }
            else
            {
                usuarioTramite.Id = usuarioTramiteModificado.Id;
                usuarioTramiteGuardado = await usuariosTramitesService.UpdateUsuarioTramiteAsync(usuarioTramite);
            }
            if (!usuarioTramiteGuardado)
            {
                MessageBox.Show("Error al guardar el usuario del trámite");
                return;
            }
            MessageBox.Show("Usuario del trámite guardado correctamente");
            await LoadUsuariosTramites();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            usuarioTramiteModificado = null;

        }

        private void ClearTextBox()
        {
            comboUsuarios.SelectedIndex = -1;
            comboTramites.SelectedIndex = -1;
            comboRolUsuario.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            usuarioTramiteModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el usuario del trámite seleccionado en la grilla
            if (dataGridUsuariosTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario del trámite para modificar");
                return;
            }
            usuarioTramiteModificado = (UsuarioTramite)dataGridUsuariosTramites.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de usuario del trámite seleccionado
            if (usuarioTramiteModificado.UsuarioId != 0)
                comboUsuarios.SelectedValue = usuarioTramiteModificado.UsuarioId;
            if (usuarioTramiteModificado.TramiteId != 0)
                comboTramites.SelectedValue = usuarioTramiteModificado.TramiteId;
            comboRolUsuario.SelectedItem = usuarioTramiteModificado.RolUsuario;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void OcultarColumnas()
        {
            // Ocultar las columnas que no queremos mostrar
            if (dataGridUsuariosTramites.Columns["UsuarioId"] != null)
                dataGridUsuariosTramites.Columns["UsuarioId"].Visible = false;

            if (dataGridUsuariosTramites.Columns["TramiteId"] != null)
                dataGridUsuariosTramites.Columns["TramiteId"].Visible = false;

            if (dataGridUsuariosTramites.Columns["IsDeleted"] != null)
                dataGridUsuariosTramites.Columns["IsDeleted"].Visible = false;
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
            //capturamos el usuario del trámite seleccionado en la grilla
            if (dataGridUsuariosTramites.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario del trámite para eliminar");
                return;
            }

            var usuarioTramiteAEliminar = (UsuarioTramite)dataGridUsuariosTramites.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el usuario del trámite
            var result = MessageBox.Show($"¿Está seguro de eliminar al usuario del trámite {usuarioTramiteAEliminar.Usuario} ({usuarioTramiteAEliminar.RolUsuario})?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el usuario del trámite
                var usuarioTramiteEliminado = await usuariosTramitesService.DeleteUsuarioTramiteAsync(usuarioTramiteAEliminar.Id);
                if (!usuarioTramiteEliminado)
                {
                    MessageBox.Show("Error al eliminar el usuario del trámite");
                    return;
                }
                MessageBox.Show($"Usuario del trámite {usuarioTramiteAEliminar.Usuario} ({usuarioTramiteAEliminar.RolUsuario}) eliminado correctamente");
                await LoadUsuariosTramites();
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
                await LoadUsuariosTramites();
            }
        }

        private async Task LoadDeleteds()
        {
            var usuariosTramites = await usuariosTramitesService.GetDeletedsAsync();
            if (usuariosTramites != null)
            {
                dataGridUsuariosTramites.DataSource = usuariosTramites;
                OcultarColumnas();
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el usuario del trámite seleccionado en la grilla
            if (dataGridUsuariosTramites.CurrentRow != null)
            {
                var usuarioTramiteARestaurar = (UsuarioTramite)dataGridUsuariosTramites.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el usuario del trámite
                var result = MessageBox.Show($"¿Está seguro de restaurar al usuario del trámite {usuarioTramiteARestaurar.Usuario} ({usuarioTramiteARestaurar.RolUsuario})?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el usuario del trámite
                    var usuarioTramiteRestaurado = await usuariosTramitesService.RestoreUsuarioTramiteAsync(usuarioTramiteARestaurar.Id);
                    if (usuarioTramiteRestaurado)
                    {
                        MessageBox.Show($"Usuario del trámite {usuarioTramiteARestaurar.Usuario} ({usuarioTramiteARestaurar.RolUsuario}) restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el usuario del trámite");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un usuario del trámite para restaurar");
            }
        }
    }
}
