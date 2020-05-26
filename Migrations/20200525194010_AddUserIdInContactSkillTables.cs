using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiDeveloperChallenge.Migrations
{
    public partial class AddUserIdInContactSkillTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Skills_Name_Level",
                table: "Skills");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Skills",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Contacts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Skills_UserId",
                table: "Skills",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name_Level_UserId",
                table: "Skills",
                columns: new[] { "Name", "Level", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_Firstname_Lastname_UserId",
                table: "Contacts",
                columns: new[] { "Firstname", "Lastname", "UserId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_User_UserId",
                table: "Contacts",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Skills_User_UserId",
                table: "Skills",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_User_UserId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Skills_User_UserId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_UserId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Skills_Name_Level_UserId",
                table: "Skills");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_UserId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_Firstname_Lastname_UserId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Skills");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Contacts");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Name_Level",
                table: "Skills",
                columns: new[] { "Name", "Level" },
                unique: true);
        }
    }
}
