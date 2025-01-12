using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LaundryManagement.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabaseWithTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MachineId",
                table: "Transactions",
                newName: "IdCycle");

            migrationBuilder.RenameColumn(
                name: "CycleId",
                table: "Transactions",
                newName: "IdClient");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IdClient",
                table: "Transactions",
                column: "IdClient");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_IdCycle",
                table: "Transactions",
                column: "IdCycle");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Clients_IdClient",
                table: "Transactions",
                column: "IdClient",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Cycles_IdCycle",
                table: "Transactions",
                column: "IdCycle",
                principalTable: "Cycles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Clients_IdClient",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Cycles_IdCycle",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_IdClient",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_IdCycle",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "IdCycle",
                table: "Transactions",
                newName: "MachineId");

            migrationBuilder.RenameColumn(
                name: "IdClient",
                table: "Transactions",
                newName: "CycleId");
        }
    }
}
