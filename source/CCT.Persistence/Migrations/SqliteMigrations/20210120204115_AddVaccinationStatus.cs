using Microsoft.EntityFrameworkCore.Migrations;

namespace CCT.Persistence.Migrations.SqliteMigrations
{
    public partial class AddVaccinationStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVaccinated",
                table: "Persons",
                type: "INTEGER",
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
