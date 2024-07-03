using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjektTabAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddVerificationCodeEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VerificationCodes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificationCodes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 7, 1, 19, 40, 52, 940, DateTimeKind.Local).AddTicks(7760));

            migrationBuilder.CreateIndex(
                name: "IX_VerificationCodes_ClientId",
                table: "VerificationCodes",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VerificationCodes");

            migrationBuilder.UpdateData(
                table: "Logins",
                keyColumn: "Id",
                keyValue: new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"),
                column: "DateTime",
                value: new DateTime(2024, 6, 26, 19, 19, 44, 595, DateTimeKind.Local).AddTicks(5015));
        }
    }
}
