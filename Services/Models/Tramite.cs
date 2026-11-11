using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Tramite
    {
        public int Id { get; set; }
        public string TipoTramite { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;

        public int VehiculoId { get; set; }
        public Vehiculo? Vehiculo { get; set; }

        public bool IsDeleted { get; set; } = false;

        public override string ToString() => $"{TipoTramite} - {Vehiculo}";
    }
}