using Microsoft.EntityFrameworkCore;
using Services.Models;

namespace Backend.Data
{
    public class GestoriaContext : DbContext
    {
        public GestoriaContext()
        {

        }
        public GestoriaContext(DbContextOptions<GestoriaContext> options) : base(options)
        {
        }
        //la tabla se llama Usuarios porque Clientes ya existe en la base (es la del inventario)
        public DbSet<Cliente> Usuarios { get; set; }
        public DbSet<Empleado> Empleados { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }
        public DbSet<Tramite> Tramites { get; set; }
        public DbSet<ClienteTramite> ClientesTramites { get; set; }
        public DbSet<Turno> Turnos { get; set; }
        public DbSet<Pago> Pagos { get; set; }

        //creamos el método OnConfiguring para configurar la cadena de conexión a la base de datos postgreSQL
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                var cadenaConexion = configuration.GetConnectionString("postgresRemote");

                optionsBuilder.UseNpgsql(cadenaConexion);
            }
        }

        //creamos el método OnModelCreating para configurar el modelo e insertar datos semilla
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //cargamos datos semilla para la tabla Clientes
            modelBuilder.Entity<Cliente>().HasData(
                new Cliente { Id = 1, Nombre = "Juan", Apellido = "Pérez", Dni = "30111222", Telefono = "3415551111" },
                new Cliente { Id = 2, Nombre = "Laura", Apellido = "Gómez", Dni = "32444555", Telefono = "3415552222" },
                new Cliente { Id = 3, Nombre = "Carlos", Apellido = "Díaz", Dni = "28777888", Telefono = "3415553333" }
            );

            //cargamos datos semilla para la tabla Empleados
            modelBuilder.Entity<Empleado>().HasData(
                new Empleado { Id = 1, Nombre = "Ana", Apellido = "Martínez", Cargo = "Gestora", Telefono = "3415554444" },
                new Empleado { Id = 2, Nombre = "Diego", Apellido = "Romero", Cargo = "Administrativo", Telefono = "3415555555" }
            );

            //cargamos datos semilla para la tabla Vehiculos
            modelBuilder.Entity<Vehiculo>().HasData(
                new Vehiculo { Id = 1, Patente = "AB123CD", Marca = "Ford", Tipo = "Auto", Modelo = "Fiesta", Anio = 2019, ClienteId = 1 },
                new Vehiculo { Id = 2, Patente = "A012BCD", Marca = "Honda", Tipo = "Motocicleta", Modelo = "Wave", Anio = 2021, ClienteId = 2 },
                new Vehiculo { Id = 3, Patente = "AC456EF", Marca = "Toyota", Tipo = "Auto", Modelo = "Corolla", Anio = 2022, ClienteId = 3 }
            );

            //cargamos datos semilla para la tabla Tramites
            modelBuilder.Entity<Tramite>().HasData(
                new Tramite { Id = 1, TipoTramite = "Transferencia", Estado = "Pendiente", VehiculoId = 1 },
                new Tramite { Id = 2, TipoTramite = "Cambio de radicación", Estado = "En proceso", VehiculoId = 2 }
            );

            //desactivamos la eliminación en cascada para la relación entre Vehiculo y Cliente usando Fluent API
            modelBuilder.Entity<Vehiculo>()
                .HasOne(v => v.Cliente)
                .WithMany()
                .HasForeignKey(v => v.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            //desactivamos la eliminación en cascada para la relación entre Tramite y Vehiculo usando Fluent API
            modelBuilder.Entity<Tramite>()
                .HasOne(t => t.Vehiculo)
                .WithMany()
                .HasForeignKey(t => t.VehiculoId)
                .OnDelete(DeleteBehavior.Restrict);
            //desactivamos la eliminación en cascada para las relaciones de la tabla intermedia ClienteTramite
            modelBuilder.Entity<ClienteTramite>()
                .HasOne(ct => ct.Cliente)
                .WithMany()
                .HasForeignKey(ct => ct.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<ClienteTramite>()
                .HasOne(ct => ct.Tramite)
                .WithMany()
                .HasForeignKey(ct => ct.TramiteId)
                .OnDelete(DeleteBehavior.Restrict);
            //desactivamos la eliminación en cascada para las relaciones de Turno con Tramite y con Empleado
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Tramite)
                .WithMany()
                .HasForeignKey(t => t.TramiteId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Turno>()
                .HasOne(t => t.Empleado)
                .WithMany()
                .HasForeignKey(t => t.EmpleadoId)
                .OnDelete(DeleteBehavior.Restrict);
            //desactivamos la eliminación en cascada para la relación entre Pago y Turno
            modelBuilder.Entity<Pago>()
                .HasOne(p => p.Turno)
                .WithMany()
                .HasForeignKey(p => p.TurnoId)
                .OnDelete(DeleteBehavior.Restrict);

            // configuramos los queries filters para que no se muestren los registros eliminados
            modelBuilder.Entity<Cliente>()
                .HasQueryFilter(c => !c.IsDeleted);
            modelBuilder.Entity<Empleado>()
                .HasQueryFilter(e => !e.IsDeleted);
            modelBuilder.Entity<Vehiculo>()
                .HasQueryFilter(v => !v.IsDeleted);
            modelBuilder.Entity<Tramite>()
                .HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<ClienteTramite>()
                .HasQueryFilter(ct => !ct.IsDeleted);
            modelBuilder.Entity<Turno>()
                .HasQueryFilter(t => !t.IsDeleted);
            modelBuilder.Entity<Pago>()
                .HasQueryFilter(p => !p.IsDeleted);
        }
    }
}