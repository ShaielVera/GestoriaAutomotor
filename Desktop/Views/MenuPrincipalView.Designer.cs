namespace Desktop
{
    partial class MenuPrincipalView
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
            menuStrip1 = new MenuStrip();
            iconMenuItem1 = new ToolStripMenuItem();
            SubMenuUsuariosApi = new ToolStripMenuItem();
            SubMenuEmpleadosApi = new ToolStripMenuItem();
            SubMenuVehiculosApi = new ToolStripMenuItem();
            SubMenuTramitesApi = new ToolStripMenuItem();
            SubMenuUsuariosTramitesApi = new ToolStripMenuItem();
            SubMenuTurnosApi = new ToolStripMenuItem();
            SubMenuPagosApi = new ToolStripMenuItem();
            iconMenuItem2 = new ToolStripMenuItem();
            SubMenuSalirDelSistema = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            //
            // menuStrip1
            //
            menuStrip1.Items.AddRange(new ToolStripItem[] { iconMenuItem1, iconMenuItem2 });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(800, 24);
            menuStrip1.TabIndex = 0;
            menuStrip1.Text = "menuStrip1";
            //
            // iconMenuItem1
            //
            iconMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { SubMenuUsuariosApi, SubMenuEmpleadosApi, SubMenuVehiculosApi, SubMenuTramitesApi, SubMenuUsuariosTramitesApi, SubMenuTurnosApi, SubMenuPagosApi });
            iconMenuItem1.Name = "iconMenuItem1";
            iconMenuItem1.Size = new Size(65, 20);
            iconMenuItem1.Text = "Principal";
            //
            // SubMenuUsuariosApi
            //
            SubMenuUsuariosApi.Name = "SubMenuUsuariosApi";
            SubMenuUsuariosApi.Size = new Size(200, 22);
            SubMenuUsuariosApi.Text = "Usuarios";
            SubMenuUsuariosApi.Click += SubMenuUsuariosApi_Click;
            //
            // SubMenuEmpleadosApi
            //
            SubMenuEmpleadosApi.Name = "SubMenuEmpleadosApi";
            SubMenuEmpleadosApi.Size = new Size(200, 22);
            SubMenuEmpleadosApi.Text = "Empleados";
            SubMenuEmpleadosApi.Click += SubMenuEmpleadosApi_Click;
            //
            // SubMenuVehiculosApi
            //
            SubMenuVehiculosApi.Name = "SubMenuVehiculosApi";
            SubMenuVehiculosApi.Size = new Size(200, 22);
            SubMenuVehiculosApi.Text = "Vehículos";
            SubMenuVehiculosApi.Click += SubMenuVehiculosApi_Click;
            //
            // SubMenuTramitesApi
            //
            SubMenuTramitesApi.Name = "SubMenuTramitesApi";
            SubMenuTramitesApi.Size = new Size(200, 22);
            SubMenuTramitesApi.Text = "Trámites";
            SubMenuTramitesApi.Click += SubMenuTramitesApi_Click;
            //
            // SubMenuUsuariosTramitesApi
            //
            SubMenuUsuariosTramitesApi.Name = "SubMenuUsuariosTramitesApi";
            SubMenuUsuariosTramitesApi.Size = new Size(200, 22);
            SubMenuUsuariosTramitesApi.Text = "Usuarios por trámite";
            SubMenuUsuariosTramitesApi.Click += SubMenuUsuariosTramitesApi_Click;
            //
            // SubMenuTurnosApi
            //
            SubMenuTurnosApi.Name = "SubMenuTurnosApi";
            SubMenuTurnosApi.Size = new Size(200, 22);
            SubMenuTurnosApi.Text = "Turnos";
            SubMenuTurnosApi.Click += SubMenuTurnosApi_Click;
            //
            // SubMenuPagosApi
            //
            SubMenuPagosApi.Name = "SubMenuPagosApi";
            SubMenuPagosApi.Size = new Size(200, 22);
            SubMenuPagosApi.Text = "Pagos";
            SubMenuPagosApi.Click += SubMenuPagosApi_Click;
            //
            // iconMenuItem2
            //
            iconMenuItem2.DropDownItems.AddRange(new ToolStripItem[] { SubMenuSalirDelSistema });
            iconMenuItem2.Name = "iconMenuItem2";
            iconMenuItem2.Size = new Size(41, 20);
            iconMenuItem2.Text = "Salir";
            //
            // SubMenuSalirDelSistema
            //
            SubMenuSalirDelSistema.Name = "SubMenuSalirDelSistema";
            SubMenuSalirDelSistema.Size = new Size(200, 22);
            SubMenuSalirDelSistema.Text = "Salir del sistema";
            SubMenuSalirDelSistema.Click += SubMenuSalirDelSistema_Click;
            //
            // MenuPrincipalView
            //
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MenuPrincipalView";
            Text = "Gestoría del Automotor";
            WindowState = FormWindowState.Maximized;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem iconMenuItem1;
        private ToolStripMenuItem SubMenuUsuariosApi;
        private ToolStripMenuItem SubMenuEmpleadosApi;
        private ToolStripMenuItem SubMenuVehiculosApi;
        private ToolStripMenuItem SubMenuTramitesApi;
        private ToolStripMenuItem SubMenuUsuariosTramitesApi;
        private ToolStripMenuItem SubMenuTurnosApi;
        private ToolStripMenuItem SubMenuPagosApi;
        private ToolStripMenuItem iconMenuItem2;
        private ToolStripMenuItem SubMenuSalirDelSistema;
    }
}
