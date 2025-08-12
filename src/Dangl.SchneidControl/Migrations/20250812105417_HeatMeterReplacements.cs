using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dangl.SchneidControl.Migrations
{
    /// <inheritdoc />
    public partial class HeatMeterReplacements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HeatMeterReplacements",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReplacedAtUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OldMeterValue = table.Column<int>(type: "INTEGER", nullable: false),
                    InitialValue = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HeatMeterReplacements", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HeatMeterReplacements");
        }
    }
}
