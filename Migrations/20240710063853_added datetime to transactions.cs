using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektTabAPI.Migrations
{
    /// <inheritdoc />
    public partial class addeddatetimetotransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 7, 10, 8, 38, 52, 808, DateTimeKind.Local).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "Transactions",
                keyColumn: "Id",
                keyValue: new Guid("94a62f27-05c6-4308-b5cf-b23ef5a33ee8"),
                column: "CreatedAt",
                value: new DateTime(2024, 7, 10, 8, 38, 52, 808, DateTimeKind.Local).AddTicks(9920));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Transactions");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 7, 10, 8, 18, 17, 350, DateTimeKind.Local).AddTicks(3884));
        }
    }
}
