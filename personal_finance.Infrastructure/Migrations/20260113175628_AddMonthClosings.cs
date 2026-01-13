using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace personal_finance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMonthClosings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MonthClosings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Year = table.Column<int>(type: "INTEGER", nullable: false),
                    Month = table.Column<int>(type: "INTEGER", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthClosings", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonthClosings_Year_Month",
                table: "MonthClosings",
                columns: new[] { "Year", "Month" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonthClosings");
        }
    }
}
