using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public string Patente { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int Anio { get; set; }

        public int ClienteId { get; set; }          // clave foránea
        public Cliente? Cliente { get; set; }       // navegación

        public bool IsDeleted { get; set; } = false;

        public override string ToString() => Patente;
    }
}