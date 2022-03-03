using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Enity.Migrations
{
    public partial class _030320222024 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AccountManagementModels_StudioId1",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_StudioId1",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "StudioId1",
                table: "Movies");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movies_StudioId",
                table: "Movies",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AccountManagementModels_StudioId",
                table: "Movies",
                column: "StudioId",
                principalTable: "AccountManagementModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AccountManagementModels_StudioId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_StudioId",
                table: "Movies");

            migrationBuilder.AlterColumn<string>(
                name: "StudioId",
                table: "Movies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "StudioId1",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Movies_StudioId1",
                table: "Movies",
                column: "StudioId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AccountManagementModels_StudioId1",
                table: "Movies",
                column: "StudioId1",
                principalTable: "AccountManagementModels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
