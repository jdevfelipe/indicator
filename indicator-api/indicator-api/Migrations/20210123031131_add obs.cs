using Microsoft.EntityFrameworkCore.Migrations;

namespace indicator_api.Migrations
{
    public partial class addobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Obs",
                table: "products",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "obs",
                table: "companies",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Obs",
                table: "products");

            migrationBuilder.DropColumn(
                name: "obs",
                table: "companies");
        }
    }
}
