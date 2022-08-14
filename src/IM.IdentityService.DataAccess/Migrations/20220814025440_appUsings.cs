using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM.IdentityService.DataAccess.Migrations
{
    public partial class appUsings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsing_Applications_ApplicationId",
                schema: "public",
                table: "ApplicationUsing");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsing_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsing");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsing",
                schema: "public",
                table: "ApplicationUsing");

            migrationBuilder.RenameTable(
                name: "ApplicationUsing",
                schema: "public",
                newName: "ApplicationUsings",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsing_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsings",
                newName: "IX_ApplicationUsings_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsing_ApplicationId",
                schema: "public",
                table: "ApplicationUsings",
                newName: "IX_ApplicationUsings_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsings",
                schema: "public",
                table: "ApplicationUsings",
                columns: new[] { "ApplicationUserId", "ApplicationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsings_Applications_ApplicationId",
                schema: "public",
                table: "ApplicationUsings",
                column: "ApplicationId",
                principalSchema: "public",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsings_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsings",
                column: "ApplicationUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsings_Applications_ApplicationId",
                schema: "public",
                table: "ApplicationUsings");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUsings_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUsings",
                schema: "public",
                table: "ApplicationUsings");

            migrationBuilder.RenameTable(
                name: "ApplicationUsings",
                schema: "public",
                newName: "ApplicationUsing",
                newSchema: "public");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsings_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsing",
                newName: "IX_ApplicationUsing_ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUsings_ApplicationId",
                schema: "public",
                table: "ApplicationUsing",
                newName: "IX_ApplicationUsing_ApplicationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUsing",
                schema: "public",
                table: "ApplicationUsing",
                columns: new[] { "ApplicationUserId", "ApplicationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsing_Applications_ApplicationId",
                schema: "public",
                table: "ApplicationUsing",
                column: "ApplicationId",
                principalSchema: "public",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUsing_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsing",
                column: "ApplicationUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
