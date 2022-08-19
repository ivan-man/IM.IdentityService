using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM.IdentityService.DataAccess.Migrations
{
    public partial class applicationKeyUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_AppKey",
                schema: "public",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AppKey",
                schema: "public",
                table: "Applications",
                column: "AppKey",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Applications_AppKey",
                schema: "public",
                table: "Applications");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_AppKey",
                schema: "public",
                table: "Applications",
                column: "AppKey");
        }
    }
}
