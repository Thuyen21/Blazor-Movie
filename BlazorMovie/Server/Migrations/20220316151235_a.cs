using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovie.Server.Migrations
{
    public partial class a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Movies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PremiereDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MoviesDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    MovieFile = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movies", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Movies");
        }
    }
}
