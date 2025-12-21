using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bokhandel_Labb.Migrations
{
    /// <inheritdoc />
    public partial class LoggHistorik : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Butiker",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Butiksnamn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Postnummer = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: false),
                    Stad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Telefon = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Butiker__3214EC27B7B476AB", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Författare",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Förnamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Efternamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Födelsedatum = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Författa__3214EC2725F35BCA", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Förlag",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Land = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Grundat = table.Column<int>(type: "int", nullable: true),
                    Webbplats = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Förlag__3214EC27C41A285E", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kategorier",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Namn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Beskrivning = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kategori__3214EC27097C1965", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Kunder",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Förnamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Efternamn = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Telefon = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    Adress = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Postnummer = table.Column<string>(type: "char(5)", unicode: false, fixedLength: true, maxLength: 5, nullable: true),
                    Stad = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Registreringsdatum = table.Column<DateOnly>(type: "date", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Kunder__3214EC274C6CF72A", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Logg",
                columns: table => new
                {
                    LoggId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    User = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Butiksnamn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ButikId = table.Column<int>(type: "int", nullable: false),
                    Datum = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Händelse = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logg", x => x.LoggId);
                });

            migrationBuilder.CreateTable(
                name: "Böcker",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Titel = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Språk = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Utgivningsdatum = table.Column<DateOnly>(type: "date", nullable: false),
                    FörlagID = table.Column<int>(type: "int", nullable: true),
                    Sidantal = table.Column<int>(type: "int", nullable: true),
                    KategoriID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Böcker_ISBN", x => x.ISBN);
                    table.ForeignKey(
                        name: "FK_Böcker_Förlag",
                        column: x => x.FörlagID,
                        principalTable: "Förlag",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Böcker_Kategorier",
                        column: x => x.KategoriID,
                        principalTable: "Kategorier",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "Ordrar",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KundID = table.Column<int>(type: "int", nullable: false),
                    ButikID = table.Column<int>(type: "int", nullable: false),
                    Orderdatum = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    TotalBelopp = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Pågående")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Ordrar__3214EC27C5DA7BF8", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Ordrar_Butiker",
                        column: x => x.ButikID,
                        principalTable: "Butiker",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Ordrar_Kunder",
                        column: x => x.KundID,
                        principalTable: "Kunder",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "BokFörfattare",
                columns: table => new
                {
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    FörfattareID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BokFörfattare", x => new { x.ISBN, x.FörfattareID });
                    table.ForeignKey(
                        name: "FK_BokFörfattare_Böcker",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN");
                    table.ForeignKey(
                        name: "FK_BokFörfattare_Författare",
                        column: x => x.FörfattareID,
                        principalTable: "Författare",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LagerSaldo",
                columns: table => new
                {
                    ButikID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LagerSaldo", x => new { x.ButikID, x.ISBN });
                    table.ForeignKey(
                        name: "FK_LagerSaldo_Butiker",
                        column: x => x.ButikID,
                        principalTable: "Butiker",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LagerSaldo_Böcker",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN");
                });

            migrationBuilder.CreateTable(
                name: "OrderRader",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderID = table.Column<int>(type: "int", nullable: false),
                    ISBN = table.Column<string>(type: "char(13)", unicode: false, fixedLength: true, maxLength: 13, nullable: false),
                    Antal = table.Column<int>(type: "int", nullable: false),
                    Pris = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderRad__3214EC27F072A007", x => x.ID);
                    table.ForeignKey(
                        name: "FK_OrderRader_Böcker",
                        column: x => x.ISBN,
                        principalTable: "Böcker",
                        principalColumn: "ISBN");
                    table.ForeignKey(
                        name: "FK_OrderRader_Ordrar",
                        column: x => x.OrderID,
                        principalTable: "Ordrar",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Böcker_FörlagID",
                table: "Böcker",
                column: "FörlagID");

            migrationBuilder.CreateIndex(
                name: "IX_Böcker_KategoriID",
                table: "Böcker",
                column: "KategoriID");

            migrationBuilder.CreateIndex(
                name: "IX_BokFörfattare_FörfattareID",
                table: "BokFörfattare",
                column: "FörfattareID");

            migrationBuilder.CreateIndex(
                name: "UQ__Kategori__737584FD56687B9C",
                table: "Kategorier",
                column: "Namn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__Kunder__A9D10534E0A6C92C",
                table: "Kunder",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LagerSaldo_ISBN",
                table: "LagerSaldo",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRader_ISBN",
                table: "OrderRader",
                column: "ISBN");

            migrationBuilder.CreateIndex(
                name: "IX_OrderRader_OrderID",
                table: "OrderRader",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_ButikID",
                table: "Ordrar",
                column: "ButikID");

            migrationBuilder.CreateIndex(
                name: "IX_Ordrar_KundID",
                table: "Ordrar",
                column: "KundID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BokFörfattare");

            migrationBuilder.DropTable(
                name: "LagerSaldo");

            migrationBuilder.DropTable(
                name: "Logg");

            migrationBuilder.DropTable(
                name: "OrderRader");

            migrationBuilder.DropTable(
                name: "Författare");

            migrationBuilder.DropTable(
                name: "Böcker");

            migrationBuilder.DropTable(
                name: "Ordrar");

            migrationBuilder.DropTable(
                name: "Förlag");

            migrationBuilder.DropTable(
                name: "Kategorier");

            migrationBuilder.DropTable(
                name: "Butiker");

            migrationBuilder.DropTable(
                name: "Kunder");
        }
    }
}
