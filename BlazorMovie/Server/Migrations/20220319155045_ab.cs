using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovie.Server.Migrations
{
    public partial class ab : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies",
                column: "StudioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies");

            migrationBuilder.AlterColumn<Guid>(
                name: "StudioId",
                table: "Movies",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movies_AspNetUsers_StudioId",
                table: "Movies",
                column: "StudioId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
