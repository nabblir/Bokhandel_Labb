using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bokhandel_Labb.Migrations
{
    /// <inheritdoc />
    public partial class LoggHistorik2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Logg",
                table: "Logg");

            migrationBuilder.RenameTable(
                name: "Logg",
                newName: "LoggHistorik");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "LoggHistorik",
                type: "nchar(10)",
                fixedLength: true,
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "LoggHistorik",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LoggHistorik",
                table: "LoggHistorik",
                column: "LoggId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LoggHistorik",
                table: "LoggHistorik");

            migrationBuilder.RenameTable(
                name: "LoggHistorik",
                newName: "Logg");

            migrationBuilder.AlterColumn<string>(
                name: "User",
                table: "Logg",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nchar(10)",
                oldFixedLength: true,
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Datum",
                table: "Logg",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Logg",
                table: "Logg",
                column: "LoggId");
        }
    }
}
