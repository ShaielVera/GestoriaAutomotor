namespace Desktop.Views
{
    partial class PagosApiView
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabPageLista = new TabPage();
            btnBuscar = new Button();
            btnRestaurar = new Button();
            btnEliminar = new Button();
            btnModificar = new Button();
            btnNuevo = new Button();
            dataGridPagos = new DataGridView();
            checkEliminado = new CheckBox();
            txtBusqueda = new TextBox();
            tabPageAgregarEditar = new TabPage();
            btnCancelar = new Button();
            btnGuardar = new Button();
            numMonto = new NumericUpDown();
            comboMetodoPago = new ComboBox();
            dtpFechaPago = new DateTimePicker();
            comboEstadoPago = new ComboBox();
            comboTurnos = new ComboBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            tabControl1.SuspendLayout();
            tabPageLista.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridPagos).BeginInit();
            tabPageAgregarEditar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numMonto).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPageLista);
            tabControl1.Controls.Add(tabPageAgregarEditar);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(800, 450);
            tabControl1.TabIndex = 0;
            // 
            // tabPageLista
            // 
            tabPageLista.Controls.Add(btnBuscar);
            tabPageLista.Controls.Add(btnRestaurar);
            tabPageLista.Controls.Add(btnEliminar);
            tabPageLista.Controls.Add(btnModificar);
            tabPageLista.Controls.Add(btnNuevo);
            tabPageLista.Controls.Add(dataGridPagos);
            tabPageLista.Controls.Add(checkEliminado);
            tabPageLista.Controls.Add(txtBusqueda);
            tabPageLista.Location = new Point(4, 24);
            tabPageLista.Name = "tabPageLista";
            tabPageLista.Padding = new Padding(3);
            tabPageLista.Size = new Size(792, 422);
            tabPageLista.TabIndex = 0;
            tabPageLista.Text = "Lista";
            tabPageLista.UseVisualStyleBackColor = true;
            // 
            // btnBuscar
            // 
            btnBuscar.Location = new Point(668, 84);
            btnBuscar.Name = "btnBuscar";
            btnBuscar.Size = new Size(94, 23);
            btnBuscar.TabIndex = 7;
            btnBuscar.Text = "Buscar";
            btnBuscar.UseVisualStyleBackColor = true;
            btnBuscar.Click += btnBuscar_Click;
            // 
            // btnRestaurar
            // 
            btnRestaurar.Enabled = false;
            btnRestaurar.Location = new Point(668, 280);
            btnRestaurar.Name = "btnRestaurar";
            btnRestaurar.Size = new Size(94, 23);
            btnRestaurar.TabIndex = 6;
            btnRestaurar.Text = "Restaurar";
            btnRestaurar.UseVisualStyleBackColor = true;
            btnRestaurar.Click += btnRestaurar_Click;
            // 
            // btnEliminar
            // 
            btnEliminar.Location = new Point(668, 232);
            btnEliminar.Name = "btnEliminar";
            btnEliminar.Size = new Size(94, 23);
            btnEliminar.TabIndex = 5;
            btnEliminar.Text = "Eliminar";
            btnEliminar.UseVisualStyleBackColor = true;
            btnEliminar.Click += btnEliminar_Click;
            // 
            // btnModificar
            // 
            btnModificar.Location = new Point(668, 183);
            btnModificar.Name = "btnModificar";
            btnModificar.Size = new Size(94, 23);
            btnModificar.TabIndex = 4;
            btnModificar.Text = "Modificar";
            btnModificar.UseVisualStyleBackColor = true;
            btnModificar.Click += btnModificar_Click;
            // 
            // btnNuevo
            // 
            btnNuevo.Location = new Point(668, 137);
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(94, 23);
            btnNuevo.TabIndex = 3;
            btnNuevo.Text = "Nuevo";
            btnNuevo.UseVisualStyleBackColor = true;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // dataGridPagos
            // 
            dataGridPagos.AllowUserToAddRows = false;
            dataGridPagos.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridPagos.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridPagos.Location = new Point(70, 137);
            dataGridPagos.Name = "dataGridPagos";
            dataGridPagos.ReadOnly = true;
            dataGridPagos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridPagos.Size = new Size(550, 265);
            dataGridPagos.TabIndex = 2;
            // 
            // checkEliminado
            // 
            checkEliminado.AutoSize = true;
            checkEliminado.Location = new Point(668, 328);
            checkEliminado.Name = "checkEliminado";
            checkEliminado.Size = new Size(103, 19);
            checkEliminado.TabIndex = 1;
            checkEliminado.Text = "Ver eliminados";
            checkEliminado.UseVisualStyleBackColor = true;
            checkEliminado.CheckedChanged += checkEliminado_CheckedChanged;
            // 
            // txtBusqueda
            // 
            txtBusqueda.Location = new Point(68, 84);
            txtBusqueda.Name = "txtBusqueda";
            txtBusqueda.Size = new Size(552, 23);
            txtBusqueda.TabIndex = 0;
            txtBusqueda.KeyPress += txtBusqueda_KeyPress;
            txtBusqueda.TextChanged += txtBusqueda_TextChanged;
            // 
            // tabPageAgregarEditar
            // 
            tabPageAgregarEditar.Controls.Add(btnCancelar);
            tabPageAgregarEditar.Controls.Add(btnGuardar);
            tabPageAgregarEditar.Controls.Add(numMonto);
            tabPageAgregarEditar.Controls.Add(comboMetodoPago);
            tabPageAgregarEditar.Controls.Add(dtpFechaPago);
            tabPageAgregarEditar.Controls.Add(comboEstadoPago);
            tabPageAgregarEditar.Controls.Add(comboTurnos);
            tabPageAgregarEditar.Controls.Add(label5);
            tabPageAgregarEditar.Controls.Add(label4);
            tabPageAgregarEditar.Controls.Add(label3);
            tabPageAgregarEditar.Controls.Add(label2);
            tabPageAgregarEditar.Controls.Add(label1);
            tabPageAgregarEditar.Location = new Point(4, 24);
            tabPageAgregarEditar.Name = "tabPageAgregarEditar";
            tabPageAgregarEditar.Padding = new Padding(3);
            tabPageAgregarEditar.Size = new Size(792, 422);
            tabPageAgregarEditar.TabIndex = 1;
            tabPageAgregarEditar.Text = "Agregar / Editar";
            tabPageAgregarEditar.UseVisualStyleBackColor = true;
            //
            // btnCancelar
            //
            btnCancelar.Location = new Point(671, 154);
            btnCancelar.Name = "btnCancelar";
            btnCancelar.Size = new Size(94, 33);
            btnCancelar.TabIndex = 11;
            btnCancelar.Text = "Cancelar";
            btnCancelar.UseVisualStyleBackColor = true;
            btnCancelar.Click += btnCancelar_Click;
            //
            // btnGuardar
            //
            btnGuardar.Location = new Point(671, 86);
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(94, 33);
            btnGuardar.TabIndex = 10;
            btnGuardar.Text = "Guardar";
            btnGuardar.UseVisualStyleBackColor = true;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // comboTurnos
            // 
            comboTurnos.DropDownStyle = ComboBoxStyle.DropDownList;
            comboTurnos.FormattingEnabled = true;
            comboTurnos.Location = new Point(184, 280);
            comboTurnos.Name = "comboTurnos";
            comboTurnos.Size = new Size(445, 23);
            comboTurnos.TabIndex = 9;
            // 
            // comboEstadoPago
            // 
            comboEstadoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEstadoPago.FormattingEnabled = true;
            comboEstadoPago.Items.AddRange(new object[] { "Pendiente", "Pagado", "Anulado" });
            comboEstadoPago.Location = new Point(184, 220);
            comboEstadoPago.Name = "comboEstadoPago";
            comboEstadoPago.Size = new Size(445, 23);
            comboEstadoPago.TabIndex = 8;
            // 
            // dtpFechaPago
            // 
            dtpFechaPago.Format = DateTimePickerFormat.Short;
            dtpFechaPago.Location = new Point(184, 160);
            dtpFechaPago.Name = "dtpFechaPago";
            dtpFechaPago.Size = new Size(445, 23);
            dtpFechaPago.TabIndex = 7;
            // 
            // comboMetodoPago
            // 
            comboMetodoPago.DropDownStyle = ComboBoxStyle.DropDownList;
            comboMetodoPago.FormattingEnabled = true;
            comboMetodoPago.Items.AddRange(new object[] { "Efectivo", "Transferencia", "Tarjeta de débito", "Tarjeta de crédito" });
            comboMetodoPago.Location = new Point(184, 100);
            comboMetodoPago.Name = "comboMetodoPago";
            comboMetodoPago.Size = new Size(445, 23);
            comboMetodoPago.TabIndex = 6;
            // 
            // numMonto
            // 
            numMonto.DecimalPlaces = 2;
            numMonto.ThousandsSeparator = true;
            numMonto.Location = new Point(184, 40);
            numMonto.Maximum = new decimal(new int[] { 100000000, 0, 0, 0 });
            numMonto.Name = "numMonto";
            numMonto.Size = new Size(445, 23);
            numMonto.TabIndex = 5;
            //
            // label5
            //
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label5.Location = new Point(35, 280);
            label5.Name = "label5";
            label5.Size = new Size(80, 21);
            label5.TabIndex = 4;
            label5.Text = "Turno:";
            //
            // label4
            //
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label4.Location = new Point(35, 220);
            label4.Name = "label4";
            label4.Size = new Size(80, 21);
            label4.TabIndex = 3;
            label4.Text = "Estado:";
            //
            // label3
            //
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label3.Location = new Point(35, 160);
            label3.Name = "label3";
            label3.Size = new Size(80, 21);
            label3.TabIndex = 2;
            label3.Text = "Fecha:";
            //
            // label2
            //
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label2.Location = new Point(35, 100);
            label2.Name = "label2";
            label2.Size = new Size(80, 21);
            label2.TabIndex = 1;
            label2.Text = "Método:";
            //
            // label1
            //
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 0);
            label1.Location = new Point(35, 40);
            label1.Name = "label1";
            label1.Size = new Size(80, 21);
            label1.TabIndex = 0;
            label1.Text = "Monto:";
            // 
            // PagosApiView
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(tabControl1);
            Name = "PagosApiView";
            Text = "Pagos";
            tabControl1.ResumeLayout(false);
            tabPageLista.ResumeLayout(false);
            tabPageLista.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridPagos).EndInit();
            ((System.ComponentModel.ISupportInitialize)numMonto).EndInit();
            tabPageAgregarEditar.ResumeLayout(false);
            tabPageAgregarEditar.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl1;
        private TabPage tabPageLista;
        private TabPage tabPageAgregarEditar;
        private TextBox txtBusqueda;
        private Button btnModificar;
        private Button btnNuevo;
        private DataGridView dataGridPagos;
        private CheckBox checkEliminado;
        private Button btnBuscar;
        private Button btnRestaurar;
        private Button btnEliminar;
        private NumericUpDown numMonto;
        private ComboBox comboMetodoPago;
        private DateTimePicker dtpFechaPago;
        private ComboBox comboEstadoPago;
        private ComboBox comboTurnos;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private Button btnCancelar;
        private Button btnGuardar;
    }
}