using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProjektTabAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberOfTriesToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfTries = table.Column<int>(type: "int", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BankingAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Blocked = table.Column<bool>(type: "bit", nullable: false),
                    Id_client = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BankingAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BankingAccounts_Clients_Id_client",
                        column: x => x.Id_client,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Successful = table.Column<bool>(type: "bit", nullable: false),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Id_Client = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logins_Clients_Id_Client",
                        column: x => x.Id_Client,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Balance_before = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Sender_BAId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Recipient_BAId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_BankingAccounts_Recipient_BAId",
                        column: x => x.Recipient_BAId,
                        principalTable: "BankingAccounts",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Transactions_BankingAccounts_Sender_BAId",
                        column: x => x.Sender_BAId,
                        principalTable: "BankingAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Age", "Blocked", "Email", "Login", "Name", "NumberOfTries", "Password", "Phone", "Surname" },
                values: new object[,]
                {
                    { new Guid("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"), 23, false, "lukasz@wp.pl", "lukasz", "Lukasz", 0, "lukasz", "111222333", "Jarzab" },
                    { new Guid("f1e1eb58-18a0-4617-a281-fb36e4a67cc2"), 23, false, "jan@wp.pl", "jan", "Jan", 0, "jan", "222333444", "Samiec" }
                });

            migrationBuilder.InsertData(
                table: "BankingAccounts",
                columns: new[] { "Id", "Amount", "Blocked", "Id_client", "Number" },
                values: new object[,]
                {
                    { new Guid("a099766e-075e-4696-91ed-3fa39c745051"), 100.00m, false, new Guid("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"), "11111222223333344444555556" },
                    { new Guid("e3df980b-f905-4595-b3ac-3ac2b1b7c4ca"), 350.19m, false, new Guid("f1e1eb58-18a0-4617-a281-fb36e4a67cc2"), "22222333334444455555666667" }
                });

            migrationBuilder.InsertData(
                table: "Logins",
                columns: new[] { "Id", "DateTime", "Id_Client", "Successful" },
                values: new object[] { new Guid("377c0de6-fefe-4169-b42f-3ffbc70c9965"), new DateTime(2024, 6, 26, 19, 19, 44, 595, DateTimeKind.Local).AddTicks(5015), new Guid("a257e3d3-eea9-47ef-a8dc-1c8dbc7a6536"), true });

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "Id", "Amount", "Balance_before", "Recipient_BAId", "Sender_BAId", "Title" },
                values: new object[] { new Guid("94a62f27-05c6-4308-b5cf-b23ef5a33ee8"), 100.00m, 200.00m, new Guid("e3df980b-f905-4595-b3ac-3ac2b1b7c4ca"), new Guid("a099766e-075e-4696-91ed-3fa39c745051"), "Impreza urodzinowa" });

            migrationBuilder.CreateIndex(
                name: "IX_BankingAccounts_Id_client",
                table: "BankingAccounts",
                column: "Id_client");

            migrationBuilder.CreateIndex(
                name: "IX_Logins_Id_Client",
                table: "Logins",
                column: "Id_Client");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Recipient_BAId",
                table: "Transactions",
                column: "Recipient_BAId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_Sender_BAId",
                table: "Transactions",
                column: "Sender_BAId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "BankingAccounts");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
