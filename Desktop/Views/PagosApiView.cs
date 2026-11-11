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
    public partial class PagosApiView : Form
    {
        PagosApiService pagosService = new PagosApiService();
        TurnosApiService turnosService = new TurnosApiService();

        Pago pagoModificado;
        public PagosApiView()
        {
            InitializeComponent();
            _ = LoadPagos();
            _ = LoadComboTurnos();

        }

        private async Task LoadComboTurnos()
        {
            var turnos = await turnosService.GetAllAsync();
            if (turnos != null)
            {
                //armamos una lista con el Id y el texto que se va a ver en el combo
                comboTurnos.DataSource = turnos.Select(t => new { t.Id, Descripcion = t.ToString() }).ToList();
                comboTurnos.DisplayMember = "Descripcion";
                comboTurnos.ValueMember = "Id";
                comboTurnos.SelectedIndex = -1;
            }
        }

        private async Task LoadPagos()
        {
            var pagos = await pagosService.GetAllAsync();
            if (pagos != null)
            {
                dataGridPagos.DataSource = pagos;
                OcultarColumnas();
            }
        }

        private async void btnBuscar_Click(object sender, EventArgs e)
        {
            var pagos = await pagosService.GetAllWithFilterAsync(txtBusqueda.Text);
            if (pagos != null)
            {
                dataGridPagos.DataSource = pagos;
                OcultarColumnas();
            }
        }

        private async void btnGuardar_Click(object sender, EventArgs e)
        {
            Pago pago = new Pago
            {
                Monto = numMonto.Value,
                MetodoPago = comboMetodoPago.Text,
                FechaPago = DateOnly.FromDateTime(dtpFechaPago.Value),
                EstadoPago = comboEstadoPago.Text,
                TurnoId = comboTurnos.SelectedValue != null ? (int)comboTurnos.SelectedValue : 0
            };
            bool pagoGuardado;
            if (pagoModificado == null)
            {
                pagoGuardado = await pagosService.AddPagoAsync(pago);
            }
            else
            {
                pago.Id = pagoModificado.Id;
                pagoGuardado = await pagosService.UpdatePagoAsync(pago);
            }
            if (!pagoGuardado)
            {
                MessageBox.Show("Error al guardar el pago");
                return;
            }
            MessageBox.Show("Pago guardado correctamente");
            await LoadPagos();
            ClearTextBox();
            tabControl1.SelectedTab = tabPageLista;
            pagoModificado = null;

        }

        private void ClearTextBox()
        {
            numMonto.Value = 0;
            comboMetodoPago.SelectedIndex = -1;
            dtpFechaPago.Value = DateTime.Today;
            comboEstadoPago.SelectedIndex = -1;
            comboTurnos.SelectedIndex = -1;
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPageLista;
            ClearTextBox();
            pagoModificado = null;
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //capturamos el pago seleccionado en la grilla
            if (dataGridPagos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un pago para modificar");
                return;
            }
            pagoModificado = (Pago)dataGridPagos.CurrentRow.DataBoundItem;
            //llenamos los campos del formulario con los datos de pago seleccionado
            numMonto.Value = pagoModificado.Monto;
            comboMetodoPago.SelectedItem = pagoModificado.MetodoPago;
            dtpFechaPago.Value = pagoModificado.FechaPago.ToDateTime(TimeOnly.MinValue);
            comboEstadoPago.SelectedItem = pagoModificado.EstadoPago;
            if (pagoModificado.TurnoId != 0)
                comboTurnos.SelectedValue = pagoModificado.TurnoId;
            //cambiamos a la pestaña de agregar/editar
            tabControl1.SelectedTab = tabPageAgregarEditar;
        }

        private void OcultarColumnas()
        {
            // Ocultar las columnas que no queremos mostrar
            if (dataGridPagos.Columns["TurnoId"] != null)
                dataGridPagos.Columns["TurnoId"].Visible = false;

            if (dataGridPagos.Columns["IsDeleted"] != null)
                dataGridPagos.Columns["IsDeleted"].Visible = false;
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
            //capturamos el pago seleccionado en la grilla
            if (dataGridPagos.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un pago para eliminar");
                return;
            }

            var pagoAEliminar = (Pago)dataGridPagos.CurrentRow.DataBoundItem;
            //preguntamos si está seguro de eliminar el pago
            var result = MessageBox.Show($"¿Está seguro de eliminar el pago {pagoAEliminar.Monto} del {pagoAEliminar.FechaPago}?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                //eliminamos el pago
                var pagoEliminado = await pagosService.DeletePagoAsync(pagoAEliminar.Id);
                if (!pagoEliminado)
                {
                    MessageBox.Show("Error al eliminar el pago");
                    return;
                }
                MessageBox.Show($"Pago {pagoAEliminar.Monto} del {pagoAEliminar.FechaPago} eliminado correctamente");
                await LoadPagos();
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
                await LoadPagos();
            }
        }

        private async Task LoadDeleteds()
        {
            var pagos = await pagosService.GetDeletedsAsync();
            if (pagos != null)
            {
                dataGridPagos.DataSource = pagos;
                OcultarColumnas();
            }
        }

        private async void btnRestaurar_Click(object sender, EventArgs e)
        {
            //restauramos el pago seleccionado en la grilla
            if (dataGridPagos.CurrentRow != null)
            {
                var pagoARestaurar = (Pago)dataGridPagos.CurrentRow.DataBoundItem;
                //preguntamos si está seguro de restaurar el pago
                var result = MessageBox.Show($"¿Está seguro de restaurar el pago {pagoARestaurar.Monto} del {pagoARestaurar.FechaPago}?", "Confirmar restauración", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    //restauramos el pago
                    var pagoRestaurado = await pagosService.RestorePagoAsync(pagoARestaurar.Id);
                    if (pagoRestaurado)
                    {
                        MessageBox.Show($"Pago {pagoARestaurar.Monto} del {pagoARestaurar.FechaPago} restaurado correctamente");
                        await LoadDeleteds();
                    }
                    else
                    {
                        MessageBox.Show("Error al restaurar el pago");
                    }
                }
            }

            else
            {
                MessageBox.Show("Seleccione un pago para restaurar");
            }
        }
    }
}
