using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DrivingChargesApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialDBCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    CityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Coefficient = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.CityId);
                });

            migrationBuilder.CreateTable(
                name: "Congestions",
                columns: table => new
                {
                    CongestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Validity = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Coefficient = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Congestions", x => x.CongestionId);
                    table.ForeignKey(
                        name: "FK_Congestions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LowEmissions",
                columns: table => new
                {
                    LowEmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coefficient = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LowEmissions", x => x.LowEmissionId);
                    table.ForeignKey(
                        name: "FK_LowEmissions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UltraLowEmissions",
                columns: table => new
                {
                    UltraLowEmissionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Coefficient = table.Column<double>(type: "float", nullable: false),
                    CityId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UltraLowEmissions", x => x.UltraLowEmissionId);
                    table.ForeignKey(
                        name: "FK_UltraLowEmissions_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "CityId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Periods",
                columns: table => new
                {
                    PeriodId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Start = table.Column<TimeSpan>(type: "time", nullable: false),
                    End = table.Column<TimeSpan>(type: "time", nullable: false),
                    Coefficient = table.Column<double>(type: "float", nullable: false),
                    CongestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Periods", x => x.PeriodId);
                    table.ForeignKey(
                        name: "FK_Periods_Congestions_CongestionId",
                        column: x => x.CongestionId,
                        principalTable: "Congestions",
                        principalColumn: "CongestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    PeriodId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleId);
                    table.ForeignKey(
                        name: "FK_Vehicles_Periods_PeriodId",
                        column: x => x.PeriodId,
                        principalTable: "Periods",
                        principalColumn: "PeriodId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "CityId", "Coefficient", "Name" },
                values: new object[] { 1, 1.0, "London" });

            migrationBuilder.InsertData(
                table: "Congestions",
                columns: new[] { "CongestionId", "CityId", "Coefficient", "Validity" },
                values: new object[] { 1, 1, 1.0, "Weekly" });

            migrationBuilder.InsertData(
                table: "Periods",
                columns: new[] { "PeriodId", "Coefficient", "CongestionId", "End", "Start" },
                values: new object[,]
                {
                    { 1, 1.0, 1, new TimeSpan(0, 12, 0, 0, 0), new TimeSpan(0, 7, 0, 0, 0) },
                    { 2, 1.0, 1, new TimeSpan(0, 19, 0, 0, 0), new TimeSpan(0, 12, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "VehicleId", "PeriodId", "Rate", "Type" },
                values: new object[,]
                {
                    { 1, 1, 2.0, "Car" },
                    { 2, 1, 3.0, "Van" },
                    { 3, 1, 1.0, "Motorbike" },
                    { 4, 2, 2.5, "Car" },
                    { 5, 2, 3.5, "Van" },
                    { 6, 2, 1.0, "Motorbike" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Congestions_CityId",
                table: "Congestions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_LowEmissions_CityId",
                table: "LowEmissions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Periods_CongestionId",
                table: "Periods",
                column: "CongestionId");

            migrationBuilder.CreateIndex(
                name: "IX_UltraLowEmissions_CityId",
                table: "UltraLowEmissions",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_PeriodId",
                table: "Vehicles",
                column: "PeriodId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LowEmissions");

            migrationBuilder.DropTable(
                name: "UltraLowEmissions");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Periods");

            migrationBuilder.DropTable(
                name: "Congestions");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
