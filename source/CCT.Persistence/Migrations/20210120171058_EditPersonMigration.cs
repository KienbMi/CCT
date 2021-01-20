using Microsoft.EntityFrameworkCore.Migrations;

namespace CCT.Persistence.Migrations
{
    public partial class EditPersonMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVaccinated",
                table: "Persons",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVaccinated",
                table: "Persons");
        }
    }
}
