using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiDeveloperChallenge.Migrations
{
    public partial class RemoveContactFirstNameLastNameUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Contacts_Firstname_Lastname",
                table: "Contacts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Firstname_Lastname",
                table: "Contacts",
                columns: new[] { "Firstname", "Lastname" },
                unique: true);
        }
    }
}
