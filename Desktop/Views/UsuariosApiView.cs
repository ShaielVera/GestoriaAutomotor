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
    public partial class UsuariosApiView : Form
    {
        UsuariosApiService usuariosService = new UsuariosApiService();

        Usuario usuarioModificado;
        public UsuariosApiView()
        {
            InitializeComponent();
            _ = LoadUsuarios();

        }

        private async Task LoadUsuarios()
        {
            var usuarios = await usuariosService.GetAllAsync();
            if (usuarios != null)
            {
                dataGridUsuarios.DataSource = usuarios;
                OcultarColumnas();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var usuarios = await usuariosService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (usuarios != null)
            {
                dataGridUsuarios.DataSource = usuarios;
                OcultarColumnas();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Usuario usuario = new Usuario
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Dni = txtDni.Text,
                Telefono = txtTelefono.Text
            };
            bool usuarioGuardado;
            if (usuarioModificado == null)
            {
                usuarioGuardado = await usuariosService.AddUsuarioAsync(usuario);
            }
            else
            {
                usuario.Id = usuarioModificado.Id;
                usuarioGuardado = await usuariosService.UpdateUsuarioAsync(usuario);
            }
            if (!usuarioGuardado)
            {
                MessageBox.Show("Error al guardar el usuario");
                return;
            }
            MessageBox.Show("Usuario guardado correctamente");
            await LoadUsuarios();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            usuarioModificado = null;

        }

        private void ClearTextBox()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtDni.Clear();
            txtTelefono.Clear();
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            usuarioModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el usuario seleccionado en la grilla
            if (dataGridUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para modificar");
                return;
            }
            usuarioModificado = (Usuario)dataGridUsuarios.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos del usuario seleccionado
            txtNombre.Text = usuarioModificado.Nombre;
            txtApellido.Text = usuarioModificado.Apellido;
            txtDni.Text = usuarioModificado.Dni;
            txtTelefono.Text = usuarioModificado.Telefono;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void OcultarColumnas()
        {
            // Ocultar las columnas que no queremos mostrar
            if (dataGridUsuarios.Columns["IsDeleted"] != null)
                dataGridUsuarios.Columns["IsDeleted"].Visible = false;
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
            //capturamos el usuario seleccionado en la grilla
            if (dataGridUsuarios.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un usuario para eliminar");
                return;
            }

            var usuarioAEliminar = (Usuario)dataGridUsuarios.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el usuario
            var result = MessageBox.Show($"¿Está seguro de eliminar al usuario {usuarioAEliminar.Nombre} {usuarioAEliminar.Apellido}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el usuario
                var usuarioEliminado = await usuariosService.DeleteUsuarioAsync(usuarioAEliminar.Id);
                if (!usuarioEliminado)
                {
                    MessageBox.Show("Error al eliminar el usuario");
                    return;
                }
                MessageBox.Show($"Usuario {usuarioAEliminar.Nombre} {usuarioAEliminar.Apellido} eliminado correctamente");
                await LoadUsuarios();
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
                await LoadUsuarios();
            }
        }

        private async Task LoadDeleteds()
        {
            var usuarios = await usuariosService.GetDeletedsAsync();
            if (usuarios != null)
            {
                dataGridUsuarios.DataSource = usuarios;
                OcultarColumnas();
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el usuario seleccionado en la grilla
            if (dataGridUsuarios.CurrentRow != null)
            {
                var usuarioARestaurar = (Usuario)dataGridUsuarios.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el usuario
                var result = MessageBox.Show($"¿Está seguro de restaurar al usuario {usuarioARestaurar.Nombre} {usuarioARestaurar.Apellido}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el usuario
                    var usuarioRestaurado = await usuariosService.RestoreUsuarioAsync(usuarioARestaurar.Id);
                    if (usuarioRestaurado)
                    {
                        MessageBox.Show($"Usuario {usuarioARestaurar.Nombre} {usuarioARestaurar.Apellido} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el usuario");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un usuario para restaurar");
            }
        }
    }
}
