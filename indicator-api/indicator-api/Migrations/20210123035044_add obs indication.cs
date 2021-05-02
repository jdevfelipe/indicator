using Microsoft.EntityFrameworkCore.Migrations;

namespace indicator_api.Migrations
{
    public partial class addobsindication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "obs",
                table: "indications",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "obs",
                table: "indications");
        }
    }
}
