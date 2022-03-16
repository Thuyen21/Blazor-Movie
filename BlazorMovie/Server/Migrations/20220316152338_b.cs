using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovie.Server.Migrations
{
    public partial class b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StudioId",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Movies_StudioId",
                table: "Movies",
                column: "StudioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies",
                column: "StudioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies");

            migrationBuilder.DropIndex(
                name: "IX_Movies_StudioId",
                table: "Movies");

            migrationBuilder.DropColumn(
                name: "StudioId",
                table: "Movies");
        }
    }
}
