using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Pago
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public DateOnly FechaPago { get; set; }
        public string EstadoPago { get; set; } = string.Empty;

        public int TurnoId { get; set; }
        public Turno? Turno { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}