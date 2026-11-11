using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class UsuarioTramite
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int TramiteId { get; set; }
        public Tramite? Tramite { get; set; }

        public string RolUsuario { get; set; } = string.Empty; // comprador, vendedor, solicitante

        public bool IsDeleted { get; set; } = false;
    }
}