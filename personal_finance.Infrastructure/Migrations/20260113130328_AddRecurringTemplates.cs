using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personal_finance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRecurringTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecurringTemplateId",
                table: "Transactions",
                type: "TEXT",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecurringTransactionTemplates",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DayOfMonth = table.Column<int>(type: "INTEGER", nullable: false),
                    CompetenceOffsetMonths = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    AccountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<Guid>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurringTransactionTemplates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_RecurringTemplateId",
                table: "Transactions",
                column: "RecurringTemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactionTemplates_AccountId_CategoryId",
                table: "RecurringTransactionTemplates",
                columns: new[] { "AccountId", "CategoryId" });

            migrationBuilder.CreateIndex(
                name: "IX_RecurringTransactionTemplates_IsActive_StartDate",
                table: "RecurringTransactionTemplates",
                columns: new[] { "IsActive", "StartDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurringTransactionTemplates");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_RecurringTemplateId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "RecurringTemplateId",
                table: "Transactions");
        }
    }
}
