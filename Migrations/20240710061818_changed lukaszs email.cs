using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektTabAPI.Migrations
{
    /// <inheritdoc />
    public partial class changedlukaszsemail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"),
                column: "Email",
                value: "lukjpl@wp.pl");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 7, 10, 8, 18, 17, 350, DateTimeKind.Local).AddTicks(3884));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"),
                column: "Email",
                value: "lukasz@wp.pl");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 7, 1, 19, 40, 52, 940, DateTimeKind.Local).AddTicks(7760));
        }
    }
}
