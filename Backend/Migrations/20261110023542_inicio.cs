using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class inicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gestoria");

            migrationBuilder.CreateTable(
                name: "Clientes",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Dni = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Empleados",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nombre = table.Column<string>(type: "text", nullable: false),
                    Apellido = table.Column<string>(type: "text", nullable: false),
                    Cargo = table.Column<string>(type: "text", nullable: false),
                    Telefono = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Empleados", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehiculos",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Patente = table.Column<string>(type: "text", nullable: false),
                    Marca = table.Column<string>(type: "text", nullable: false),
                    Tipo = table.Column<string>(type: "text", nullable: false),
                    Modelo = table.Column<string>(type: "text", nullable: false),
                    Anio = table.Column<int>(type: "integer", nullable: false),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehiculos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehiculos_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalSchema: "gestoria",
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tramites",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TipoTramite = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    VehiculoId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tramites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tramites_Vehiculos_VehiculoId",
                        column: x => x.VehiculoId,
                        principalSchema: "gestoria",
                        principalTable: "Vehiculos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientesTramites",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    TramiteId = table.Column<int>(type: "integer", nullable: false),
                    RolCliente = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesTramites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientesTramites_Clientes_ClienteId",
                        column: x => x.ClienteId,
                        principalSchema: "gestoria",
                        principalTable: "Clientes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientesTramites_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalSchema: "gestoria",
                        principalTable: "Tramites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Turnos",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Fecha = table.Column<DateOnly>(type: "date", nullable: false),
                    Hora = table.Column<TimeOnly>(type: "time without time zone", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    TramiteId = table.Column<int>(type: "integer", nullable: false),
                    EmpleadoId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Turnos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Turnos_Empleados_EmpleadoId",
                        column: x => x.EmpleadoId,
                        principalSchema: "gestoria",
                        principalTable: "Empleados",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Turnos_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalSchema: "gestoria",
                        principalTable: "Tramites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Pagos",
                schema: "gestoria",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Monto = table.Column<decimal>(type: "numeric", nullable: false),
                    MetodoPago = table.Column<string>(type: "text", nullable: false),
                    FechaPago = table.Column<DateOnly>(type: "date", nullable: false),
                    EstadoPago = table.Column<string>(type: "text", nullable: false),
                    TurnoId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pagos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pagos_Turnos_TurnoId",
                        column: x => x.TurnoId,
                        principalSchema: "gestoria",
                        principalTable: "Turnos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "gestoria",
                table: "Clientes",
                columns: new[] { "Id", "Apellido", "Dni", "IsDeleted", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Pérez", "30111222", false, "Juan", "3415551111" },
                    { 2, "Gómez", "32444555", false, "Laura", "3415552222" },
                    { 3, "Díaz", "28777888", false, "Carlos", "3415553333" }
                });

            migrationBuilder.InsertData(
                schema: "gestoria",
                table: "Empleados",
                columns: new[] { "Id", "Apellido", "Cargo", "IsDeleted", "Nombre", "Telefono" },
                values: new object[,]
                {
                    { 1, "Martínez", "Gestora", false, "Ana", "3415554444" },
                    { 2, "Romero", "Administrativo", false, "Diego", "3415555555" }
                });

            migrationBuilder.InsertData(
                schema: "gestoria",
                table: "Vehiculos",
                columns: new[] { "Id", "Anio", "ClienteId", "IsDeleted", "Marca", "Modelo", "Patente", "Tipo" },
                values: new object[,]
                {
                    { 1, 2019, 1, false, "Ford", "Fiesta", "AB123CD", "Auto" },
                    { 2, 2021, 2, false, "Honda", "Wave", "A012BCD", "Motocicleta" },
                    { 3, 2022, 3, false, "Toyota", "Corolla", "AC456EF", "Auto" }
                });

            migrationBuilder.InsertData(
                schema: "gestoria",
                table: "Tramites",
                columns: new[] { "Id", "Estado", "IsDeleted", "TipoTramite", "VehiculoId" },
                values: new object[,]
                {
                    { 1, "Pendiente", false, "Transferencia", 1 },
                    { 2, "En proceso", false, "Cambio de radicación", 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientesTramites_ClienteId",
                schema: "gestoria",
                table: "ClientesTramites",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesTramites_TramiteId",
                schema: "gestoria",
                table: "ClientesTramites",
                column: "TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Pagos_TurnoId",
                schema: "gestoria",
                table: "Pagos",
                column: "TurnoId");

            migrationBuilder.CreateIndex(
                name: "IX_Tramites_VehiculoId",
                schema: "gestoria",
                table: "Tramites",
                column: "VehiculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_EmpleadoId",
                schema: "gestoria",
                table: "Turnos",
                column: "EmpleadoId");

            migrationBuilder.CreateIndex(
                name: "IX_Turnos_TramiteId",
                schema: "gestoria",
                table: "Turnos",
                column: "TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehiculos_ClienteId",
                schema: "gestoria",
                table: "Vehiculos",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientesTramites",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Pagos",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Turnos",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Empleados",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Tramites",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Vehiculos",
                schema: "gestoria");

            migrationBuilder.DropTable(
                name: "Clientes",
                schema: "gestoria");
        }
    }
}
