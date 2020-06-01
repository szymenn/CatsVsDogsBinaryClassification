using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CatsVsDogs.Infrastructure.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PredictionHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    PredictedValue = table.Column<string>(nullable: true),
                    PredictionTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredictionHistory", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PredictionHistory");
        }
    }
}
