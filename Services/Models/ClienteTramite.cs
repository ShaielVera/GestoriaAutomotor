using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class ClienteTramite
    {
        public int Id { get; set; }

        public int ClienteId { get; set; }
        public Cliente? Cliente { get; set; }

        public int TramiteId { get; set; }
        public Tramite? Tramite { get; set; }

        public string RolCliente { get; set; } = string.Empty; // comprador, vendedor, solicitante

        public bool IsDeleted { get; set; } = false;
    }
}