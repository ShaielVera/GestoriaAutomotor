using Desktop.Views;

namespace Desktop
{
    public partial class MenuPrincipalView : Form
    {
        public MenuPrincipalView()
        {
            InitializeComponent();
        }

        private void SubMenuSalirDelSistema_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SubMenuUsuariosApi_Click(object sender, EventArgs e)
        {
            UsuariosApiView usuariosApiView = new();
            usuariosApiView.ShowDialog();
        }

        private void SubMenuEmpleadosApi_Click(object sender, EventArgs e)
        {
            EmpleadosApiView empleadosApiView = new();
            empleadosApiView.ShowDialog();
        }

        private void SubMenuVehiculosApi_Click(object sender, EventArgs e)
        {
            VehiculosApiView vehiculosApiView = new();
            vehiculosApiView.ShowDialog();
        }

        private void SubMenuTramitesApi_Click(object sender, EventArgs e)
        {
            TramitesApiView tramitesApiView = new();
            tramitesApiView.ShowDialog();
        }

        private void SubMenuUsuariosTramitesApi_Click(object sender, EventArgs e)
        {
            UsuariosTramitesApiView usuariosTramitesApiView = new();
            usuariosTramitesApiView.ShowDialog();
        }

        private void SubMenuTurnosApi_Click(object sender, EventArgs e)
        {
            TurnosApiView turnosApiView = new();
            turnosApiView.ShowDialog();
        }

        private void SubMenuPagosApi_Click(object sender, EventArgs e)
        {
            PagosApiView pagosApiView = new();
            pagosApiView.ShowDialog();
        }
    }
}
