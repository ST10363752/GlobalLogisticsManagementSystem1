using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GLMS.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    ClientId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    ContractId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceLevel = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignedAgreementPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SignedAgreementFileName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.ContractId);
                    table.ForeignKey(
                        name: "FK_Contracts_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ServiceRequests",
                columns: table => new
                {
                    ServiceRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AmountUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountZAR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExchangeRateUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceRequests", x => x.ServiceRequestId);
                    table.ForeignKey(
                        name: "FK_ServiceRequests_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "ContractId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "ClientId", "ContactDetails", "Name", "Region" },
                values: new object[,]
                {
                    { 1, "contact@techmove.com", "TechMove Logistics", "North America" },
                    { 2, "info@globalshipping.com", "Global Shipping Inc", "Europe" },
                    { 3, "sales@asiafreight.com", "Asia Freight Solutions", "Asia Pacific" }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "ContractId", "ClientId", "EndDate", "ServiceLevel", "SignedAgreementFileName", "SignedAgreementPath", "StartDate", "Status" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 12, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3120), "Premium", null, null, new DateTime(2025, 12, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3089), "Active" },
                    { 2, 1, new DateTime(2026, 5, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3125), "Standard", null, null, new DateTime(2025, 6, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3124), "Expired" },
                    { 3, 2, new DateTime(2026, 9, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3128), "Express", null, null, new DateTime(2026, 3, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3127), "Active" },
                    { 4, 3, new DateTime(2026, 11, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3131), "Standard", null, null, new DateTime(2026, 5, 13, 15, 42, 31, 880, DateTimeKind.Local).AddTicks(3130), "Draft" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ClientId",
                table: "Contracts",
                column: "ClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceRequests_ContractId",
                table: "ServiceRequests",
                column: "ContractId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceRequests");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
