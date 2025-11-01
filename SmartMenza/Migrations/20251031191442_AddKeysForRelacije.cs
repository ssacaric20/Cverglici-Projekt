using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartMenza.API.Migrations
{
    /// <inheritdoc />
    public partial class AddKeysForRelacije : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LozinkaHash",
                table: "Korisnici",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UlogaId",
                table: "Korisnici",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ciljevi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KalorijeCilj = table.Column<int>(type: "int", nullable: false),
                    ProteiniCilj = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MastiCilj = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UgljikohidratiCilj = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DatumPostavljanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ciljevi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ciljevi_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Jela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cijena = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Opis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Kalorije = table.Column<int>(type: "int", nullable: false),
                    Proteini = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Masti = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Ugljikohidrati = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SlikaPutanja = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NutritivneVrijednostiId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jela", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sastojci",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sastojci", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Uloge",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NazivUloge = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Uloge", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DnevniMeniji",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Datum = table.Column<DateOnly>(type: "date", nullable: false),
                    JeloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnevniMeniji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DnevniMeniji_Jela_JeloId",
                        column: x => x.JeloId,
                        principalTable: "Jela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DnevniUnosi",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DatumUnosa = table.Column<DateTime>(type: "datetime2", nullable: false),
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    JeloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DnevniUnosi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DnevniUnosi_Jela_JeloId",
                        column: x => x.JeloId,
                        principalTable: "Jela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DnevniUnosi_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Favorit",
                columns: table => new
                {
                    KorisnikId = table.Column<int>(type: "int", nullable: false),
                    JeloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Favorit", x => new { x.KorisnikId, x.JeloId });
                    table.ForeignKey(
                        name: "FK_Favorit_Jela_JeloId",
                        column: x => x.JeloId,
                        principalTable: "Jela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Favorit_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OcjenaJela",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Ocjena = table.Column<int>(type: "int", nullable: false),
                    JeloId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OcjenaJela", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OcjenaJela_Jela_JeloId",
                        column: x => x.JeloId,
                        principalTable: "Jela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JeloSastojci",
                columns: table => new
                {
                    JeloId = table.Column<int>(type: "int", nullable: false),
                    SastojakId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JeloSastojci", x => new { x.JeloId, x.SastojakId });
                    table.ForeignKey(
                        name: "FK_JeloSastojci_Jela_JeloId",
                        column: x => x.JeloId,
                        principalTable: "Jela",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JeloSastojci_Sastojci_SastojakId",
                        column: x => x.SastojakId,
                        principalTable: "Sastojci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Korisnici_UlogaId",
                table: "Korisnici",
                column: "UlogaId");

            migrationBuilder.CreateIndex(
                name: "IX_Ciljevi_KorisnikId",
                table: "Ciljevi",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_DnevniMeniji_JeloId",
                table: "DnevniMeniji",
                column: "JeloId");

            migrationBuilder.CreateIndex(
                name: "IX_DnevniUnosi_JeloId",
                table: "DnevniUnosi",
                column: "JeloId");

            migrationBuilder.CreateIndex(
                name: "IX_DnevniUnosi_KorisnikId",
                table: "DnevniUnosi",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorit_JeloId",
                table: "Favorit",
                column: "JeloId");

            migrationBuilder.CreateIndex(
                name: "IX_JeloSastojci_SastojakId",
                table: "JeloSastojci",
                column: "SastojakId");

            migrationBuilder.CreateIndex(
                name: "IX_OcjenaJela_JeloId",
                table: "OcjenaJela",
                column: "JeloId");

            migrationBuilder.AddForeignKey(
                name: "FK_Korisnici_Uloge_UlogaId",
                table: "Korisnici",
                column: "UlogaId",
                principalTable: "Uloge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Korisnici_Uloge_UlogaId",
                table: "Korisnici");

            migrationBuilder.DropTable(
                name: "Ciljevi");

            migrationBuilder.DropTable(
                name: "DnevniMeniji");

            migrationBuilder.DropTable(
                name: "DnevniUnosi");

            migrationBuilder.DropTable(
                name: "Favorit");

            migrationBuilder.DropTable(
                name: "JeloSastojci");

            migrationBuilder.DropTable(
                name: "OcjenaJela");

            migrationBuilder.DropTable(
                name: "Uloge");

            migrationBuilder.DropTable(
                name: "Sastojci");

            migrationBuilder.DropTable(
                name: "Jela");

            migrationBuilder.DropIndex(
                name: "IX_Korisnici_UlogaId",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "LozinkaHash",
                table: "Korisnici");

            migrationBuilder.DropColumn(
                name: "UlogaId",
                table: "Korisnici");
        }
    }
}
