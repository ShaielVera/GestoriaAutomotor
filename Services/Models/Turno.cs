using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Turno
    {
        public int Id { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public string Estado { get; set; } = string.Empty;

        public int TramiteId { get; set; }
        public Tramite? Tramite { get; set; }

        public int EmpleadoId { get; set; }
        public Empleado? Empleado { get; set; }

        public bool IsDeleted { get; set; } = false;

        public override string ToString() => $"{Fecha:dd/MM/yyyy} {Hora:HH:mm} - {Tramite}";
    }
}