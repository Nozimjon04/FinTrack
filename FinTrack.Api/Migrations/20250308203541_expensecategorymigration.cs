using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinTrack.Api.Migrations
{
    /// <inheritdoc />
    public partial class expensecategorymigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "ExpenseCategories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_UserId",
                table: "ExpenseCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseCategories_Users_UserId",
                table: "ExpenseCategories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseCategories_Users_UserId",
                table: "ExpenseCategories");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseCategories_UserId",
                table: "ExpenseCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExpenseCategories");
        }
    }
}
