using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IM.IdentityService.DataAccess.Migrations
{
    public partial class applicationUsings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "Applications");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Applications_ApplicationId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ApplicationApplicationUser",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ApplicationId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_Applications_ApplicationUserId",
                schema: "public",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                schema: "public",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                schema: "public",
                table: "Applications");

            migrationBuilder.CreateTable(
                name: "ApplicationUsing",
                schema: "public",
                columns: table => new
                {
                    ApplicationUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUsing", x => new { x.ApplicationUserId, x.ApplicationId });
                    table.ForeignKey(
                        name: "FK_ApplicationUsing_Applications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "public",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUsing_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsing_ApplicationId",
                schema: "public",
                table: "ApplicationUsing",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUsing_ApplicationUserId",
                schema: "public",
                table: "ApplicationUsing",
                column: "ApplicationUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUsing",
                schema: "public");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                schema: "public",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicationUserId",
                schema: "public",
                table: "Applications",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ApplicationApplicationUser",
                schema: "public",
                columns: table => new
                {
                    ApplicationsId = table.Column<int>(type: "integer", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationApplicationUser", x => new { x.ApplicationsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ApplicationApplicationUser_Applications_ApplicationsId",
                        column: x => x.ApplicationsId,
                        principalSchema: "public",
                        principalTable: "Applications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationApplicationUser_AspNetUsers_UsersId",
                        column: x => x.UsersId,
                        principalSchema: "public",
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ApplicationId",
                schema: "public",
                table: "AspNetUsers",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Applications_ApplicationUserId",
                schema: "public",
                table: "Applications",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationApplicationUser_UsersId",
                schema: "public",
                table: "ApplicationApplicationUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_ApplicationUserId",
                schema: "public",
                table: "Applications",
                column: "ApplicationUserId",
                principalSchema: "public",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Applications_ApplicationId",
                schema: "public",
                table: "AspNetUsers",
                column: "ApplicationId",
                principalSchema: "public",
                principalTable: "Applications",
                principalColumn: "Id");
        }
    }
}
