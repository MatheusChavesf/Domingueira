using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FootballStats.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jogadores",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jogadores", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosPartida",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    JogadorId = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<DateOnly>(type: "date", nullable: false),
                    Gols = table.Column<int>(type: "integer", nullable: false),
                    Assistencias = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPartida", x => x.Id);
                    table.CheckConstraint("CK_RegistrosPartida_Assistencias", "\"Assistencias\" >= 0 AND \"Assistencias\" <= 99");
                    table.CheckConstraint("CK_RegistrosPartida_Gols", "\"Gols\" >= 0 AND \"Gols\" <= 99");
                    table.ForeignKey(
                        name: "FK_RegistrosPartida_Jogadores_JogadorId",
                        column: x => x.JogadorId,
                        principalTable: "Jogadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPartida_JogadorId",
                table: "RegistrosPartida",
                column: "JogadorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RegistrosPartida");

            migrationBuilder.DropTable(
                name: "Jogadores");
        }
    }
}
