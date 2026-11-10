using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public override string ToString() => $"{Nombre} {Apellido}";
    }
}