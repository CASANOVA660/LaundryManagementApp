using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMachineCycleTiming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentCycleEndTime",
                table: "Machines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentCycleId",
                table: "Machines",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CurrentCycleStartTime",
                table: "Machines",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Machines_CurrentCycleId",
                table: "Machines",
                column: "CurrentCycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Cycles_CurrentCycleId",
                table: "Machines",
                column: "CurrentCycleId",
                principalTable: "Cycles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Cycles_CurrentCycleId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_CurrentCycleId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "CurrentCycleEndTime",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "CurrentCycleId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "CurrentCycleStartTime",
                table: "Machines");
        }
    }
}
