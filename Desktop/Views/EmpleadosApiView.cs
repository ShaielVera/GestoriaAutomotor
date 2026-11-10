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
    public partial class EmpleadosApiView : Form
    {
        EmpleadosApiService empleadosService = new EmpleadosApiService();


        Empleado empleadoModificado;
        public EmpleadosApiView()
        {
            InitializeComponent();
            _ = LoadEmpleados();

        }

        private async Task LoadEmpleados()
        {
            var empleados = await empleadosService.GetAllAsync();
            if (empleados != null)
            {
                dataGridEmpleados.DataSource = empleados;
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var empleados = await empleadosService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (empleados != null)
            {
                dataGridEmpleados.DataSource = empleados;
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Empleado empleado = new Empleado
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Cargo = txtCargo.Text,
                Telefono = txtTelefono.Text
            };
            bool empleadoGuardado;
            if (empleadoModificado == null)
            {
                empleadoGuardado = await empleadosService.AddEmpleadoAsync(empleado);
            }
            else
            {
                empleado.Id = empleadoModificado.Id;
                empleadoGuardado = await empleadosService.UpdateEmpleadoAsync(empleado);
            }
            if (!empleadoGuardado)
            {
                MessageBox.Show("Error al guardar el empleado");
                return;
            }
            MessageBox.Show("Empleado guardado correctamente");
            await LoadEmpleados();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            empleadoModificado = null;

        }

        private void ClearTextBox()
        {
            txtNombre.Clear();
            txtApellido.Clear();
            txtCargo.Clear();
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
            empleadoModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el empleado seleccionado en la grilla
            if (dataGridEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado para modificar");
                return;
            }
            empleadoModificado = (Empleado)dataGridEmpleados.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de empleado seleccionado
            txtNombre.Text = empleadoModificado.Nombre;
            txtApellido.Text = empleadoModificado.Apellido;
            txtCargo.Text = empleadoModificado.Cargo;
            txtTelefono.Text = empleadoModificado.Telefono;
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
            //capturamos el empleado seleccionado en la grilla
            if (dataGridEmpleados.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un empleado para eliminar");
                return;
            }

            var empleadoAEliminar = (Empleado)dataGridEmpleados.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el empleado
            var result = MessageBox.Show($"¿Está seguro de eliminar al empleado {empleadoAEliminar.Nombre} {empleadoAEliminar.Apellido}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el empleado
                var empleadoEliminado = await empleadosService.DeleteEmpleadoAsync(empleadoAEliminar.Id);
                if (!empleadoEliminado)
                {
                    MessageBox.Show("Error al eliminar el empleado");
                    return;
                }
                MessageBox.Show($"Empleado {empleadoAEliminar.Nombre} {empleadoAEliminar.Apellido} eliminado correctamente");
                await LoadEmpleados();
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
                await LoadEmpleados();
            }
        }

        private async Task LoadDeleteds()
        {
            var empleados = await empleadosService.GetDeletedsAsync();
            if (empleados != null)
            {
                dataGridEmpleados.DataSource = empleados;
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el empleado seleccionado en la grilla
            if (dataGridEmpleados.CurrentRow != null)
            {
                var empleadoARestaurar = (Empleado)dataGridEmpleados.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el empleado
                var result = MessageBox.Show($"¿Está seguro de restaurar al empleado {empleadoARestaurar.Nombre} {empleadoARestaurar.Apellido}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el empleado
                    var empleadoRestaurado = await empleadosService.RestoreEmpleadoAsync(empleadoARestaurar.Id);
                    if (empleadoRestaurado)
                    {
                        MessageBox.Show($"Empleado {empleadoARestaurar.Nombre} {empleadoARestaurar.Apellido} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el empleado");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un empleado para restaurar");
            }
        }
    }
}
