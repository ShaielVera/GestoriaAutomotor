using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class cambiamos_clientes_por_usuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Usuarios_ClienteId",
                table: "Vehiculos");

            migrationBuilder.DropTable(
                name: "ClientesTramites");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Vehiculos",
                newName: "UsuarioId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehiculos_ClienteId",
                table: "Vehiculos",
                newName: "IX_Vehiculos_UsuarioId");

            migrationBuilder.CreateTable(
                name: "UsuariosTramites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UsuarioId = table.Column<int>(type: "integer", nullable: false),
                    TramiteId = table.Column<int>(type: "integer", nullable: false),
                    RolUsuario = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuariosTramites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuariosTramites_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalTable: "Tramites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UsuariosTramites_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosTramites_TramiteId",
                table: "UsuariosTramites",
                column: "TramiteId");

            migrationBuilder.CreateIndex(
                name: "IX_UsuariosTramites_UsuarioId",
                table: "UsuariosTramites",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Usuarios_UsuarioId",
                table: "Vehiculos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vehiculos_Usuarios_UsuarioId",
                table: "Vehiculos");

            migrationBuilder.DropTable(
                name: "UsuariosTramites");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Vehiculos",
                newName: "ClienteId");

            migrationBuilder.RenameIndex(
                name: "IX_Vehiculos_UsuarioId",
                table: "Vehiculos",
                newName: "IX_Vehiculos_ClienteId");

            migrationBuilder.CreateTable(
                name: "ClientesTramites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClienteId = table.Column<int>(type: "integer", nullable: false),
                    TramiteId = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    RolCliente = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientesTramites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientesTramites_Tramites_TramiteId",
                        column: x => x.TramiteId,
                        principalTable: "Tramites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClientesTramites_Usuarios_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientesTramites_ClienteId",
                table: "ClientesTramites",
                column: "ClienteId");

            migrationBuilder.CreateIndex(
                name: "IX_ClientesTramites_TramiteId",
                table: "ClientesTramites",
                column: "TramiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vehiculos_Usuarios_ClienteId",
                table: "Vehiculos",
                column: "ClienteId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
