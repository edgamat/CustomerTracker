using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CustomerTracker.Persistence.Migrations
{
    public partial class AccountingId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AccountingId",
                table: "Customer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountingId",
                table: "Customer");
        }
    }
}
