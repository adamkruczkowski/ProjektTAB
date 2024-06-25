using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektTabAPI.Migrations
{
    /// <inheritdoc />
    public partial class Addedtitlefortransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 6, 25, 21, 18, 17, 295, DateTimeKind.Local).AddTicks(8047));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("94a62f27-05c6-4308-b5cf-b23ef5a33ee8"),
                column: "Title",
                value: "Impreza urodzinowa");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 6, 5, 18, 10, 14, 167, DateTimeKind.Local).AddTicks(6946));
        }
    }
}
