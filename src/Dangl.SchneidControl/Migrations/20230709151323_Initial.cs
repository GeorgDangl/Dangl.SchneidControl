using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dangl.SchneidControl.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataEntries",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CreatedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LogEntryType = table.Column<int>(type: "INTEGER", nullable: false),
                    Value = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_CreatedAtUtc",
                table: "DataEntries",
                column: "CreatedAtUtc");

            migrationBuilder.CreateIndex(
                name: "IX_DataEntries_LogEntryType",
                table: "DataEntries",
                column: "LogEntryType");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataEntries");
        }
    }
}
