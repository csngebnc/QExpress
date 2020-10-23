using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QExpress.Data.Migrations
{
    public partial class InitMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "jogosultsagi_szint",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Ceg",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nev = table.Column<string>(nullable: false),
                    CegadminId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ceg", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ceg_AspNetUsers_CegadminId",
                        column: x => x.CegadminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Kategoria",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Megnevezes = table.Column<string>(nullable: false),
                    CegId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategoria", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kategoria_Ceg_CegId",
                        column: x => x.CegId,
                        principalTable: "Ceg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Telephely",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cim = table.Column<string>(nullable: false),
                    Ceg_id = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Telephely", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Telephely_Ceg_Ceg_id",
                        column: x => x.Ceg_id,
                        principalTable: "Ceg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "UgyfLevelek",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Panasz = table.Column<string>(nullable: false),
                    PanaszoloId = table.Column<string>(nullable: false),
                    CegId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UgyfLevelek", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UgyfLevelek_Ceg_CegId",
                        column: x => x.CegId,
                        principalTable: "Ceg",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_UgyfLevelek_AspNetUsers_PanaszoloId",
                        column: x => x.PanaszoloId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FelhasznaloTelephely",
                columns: table => new
                {
                    FelhasznaloId = table.Column<string>(nullable: false),
                    TelephelyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FelhasznaloTelephely", x => new { x.FelhasznaloId, x.TelephelyId });
                    table.ForeignKey(
                        name: "FK_FelhasznaloTelephely_AspNetUsers_FelhasznaloId",
                        column: x => x.FelhasznaloId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FelhasznaloTelephely_Telephely_TelephelyId",
                        column: x => x.TelephelyId,
                        principalTable: "Telephely",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Sorszam",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SorszamIdTelephelyen = table.Column<int>(nullable: false),
                    Idopont = table.Column<DateTime>(nullable: false),
                    Allapot = table.Column<string>(nullable: false),
                    UgyfelId = table.Column<string>(nullable: false),
                    TelephelyId = table.Column<int>(nullable: false),
                    KategoriaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sorszam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sorszam_Kategoria_KategoriaId",
                        column: x => x.KategoriaId,
                        principalTable: "Kategoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Sorszam_Telephely_TelephelyId",
                        column: x => x.TelephelyId,
                        principalTable: "Telephely",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Sorszam_AspNetUsers_UgyfelId",
                        column: x => x.UgyfelId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ceg_CegadminId",
                table: "Ceg",
                column: "CegadminId");

            migrationBuilder.CreateIndex(
                name: "IX_FelhasznaloTelephely_TelephelyId",
                table: "FelhasznaloTelephely",
                column: "TelephelyId");

            migrationBuilder.CreateIndex(
                name: "IX_Kategoria_CegId",
                table: "Kategoria",
                column: "CegId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorszam_KategoriaId",
                table: "Sorszam",
                column: "KategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorszam_TelephelyId",
                table: "Sorszam",
                column: "TelephelyId");

            migrationBuilder.CreateIndex(
                name: "IX_Sorszam_UgyfelId",
                table: "Sorszam",
                column: "UgyfelId");

            migrationBuilder.CreateIndex(
                name: "IX_Telephely_Ceg_id",
                table: "Telephely",
                column: "Ceg_id");

            migrationBuilder.CreateIndex(
                name: "IX_UgyfLevelek_CegId",
                table: "UgyfLevelek",
                column: "CegId");

            migrationBuilder.CreateIndex(
                name: "IX_UgyfLevelek_PanaszoloId",
                table: "UgyfLevelek",
                column: "PanaszoloId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FelhasznaloTelephely");

            migrationBuilder.DropTable(
                name: "Sorszam");

            migrationBuilder.DropTable(
                name: "UgyfLevelek");

            migrationBuilder.DropTable(
                name: "Kategoria");

            migrationBuilder.DropTable(
                name: "Telephely");

            migrationBuilder.DropTable(
                name: "Ceg");

            migrationBuilder.DropColumn(
                name: "jogosultsagi_szint",
                table: "AspNetUsers");
        }
    }
}
