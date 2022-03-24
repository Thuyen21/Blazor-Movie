using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovie.Server.Migrations
{
    public partial class c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MoviesDescription",
                table: "Movies",
                newName: "Description");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c294babc-bed5-4402-adc0-d80bf48466ec"),
                column: "ConcurrencyStamp",
                value: "51bb9ff8-c02b-4776-8847-fc3bff817822");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf8c7373-c04f-40a1-b1b7-64612eba45d8"),
                column: "ConcurrencyStamp",
                value: "13362001-efce-4842-bdd9-9708094a1f23");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d6fceefd-466a-4b02-b748-221c84112a42"),
                column: "ConcurrencyStamp",
                value: "7e693b31-a974-4208-afb4-57d8a58a3b4a");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("219bb40e-0cab-4f08-a408-f33ecb138ed0"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "f40be634-1d8d-481d-9da5-44dfd2e05cf1", new DateTime(2022, 3, 22, 22, 11, 6, 568, DateTimeKind.Local).AddTicks(3493), "AQAAAAEAACcQAAAAEBAXvqi1Ayt6go8/d54J/sgBGqRmM9IWw7DbV02rqTDtjfyIVX+m4oFT68Ra3EHNXA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8aacfc8a-3418-46f4-9cf8-395fc5b90499"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "1821e7c3-330b-4f20-894b-1f0eac400f97", new DateTime(2022, 3, 22, 22, 11, 6, 570, DateTimeKind.Local).AddTicks(4987), "AQAAAAEAACcQAAAAENttwqE/hZ8Rgg1x+zg5bnvxmNlnsi+eksul8SYyXF1b5Mp7t7aODlpnPueriOU0tg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "177c63c5-3c68-4ac9-93a5-a08281941b99", new DateTime(2022, 3, 22, 22, 11, 6, 570, DateTimeKind.Local).AddTicks(5021), "AQAAAAEAACcQAAAAEBDxgcRVWiWRf6s8zX+oG3mroZJcMk8RewD1F9fHgJWDBWm07QlX+DdY1lhbnMSkHQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Movies",
                newName: "MoviesDescription");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c294babc-bed5-4402-adc0-d80bf48466ec"),
                column: "ConcurrencyStamp",
                value: "921b99fb-c11f-4520-ac5e-f13a15af03c1");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf8c7373-c04f-40a1-b1b7-64612eba45d8"),
                column: "ConcurrencyStamp",
                value: "a5822236-e067-407d-bef9-3b15c7715c79");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d6fceefd-466a-4b02-b748-221c84112a42"),
                column: "ConcurrencyStamp",
                value: "3cb9592f-1298-412a-8631-ebb510b345ce");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("219bb40e-0cab-4f08-a408-f33ecb138ed0"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "015a6f8e-24c2-4419-8377-4772af13ce84", new DateTime(2022, 3, 22, 21, 51, 47, 60, DateTimeKind.Local).AddTicks(2624), "AQAAAAEAACcQAAAAEHh7bu5ffUJpTYBIioMFFfMCDG36tesFrxVUgMZeJsFVbttwnPQfvnO6/AWObOByMQ==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8aacfc8a-3418-46f4-9cf8-395fc5b90499"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "4c46b7e6-17aa-4d36-9ad4-237129386aa0", new DateTime(2022, 3, 22, 21, 51, 47, 61, DateTimeKind.Local).AddTicks(3523), "AQAAAAEAACcQAAAAEL8j8RBcRu6YBy7Ez1MpQj+wxR5sxbJ9LqEQx99A/YEyZeaPdoRmFjJD83Zm0tvArA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "cca3c4b1-ef9b-4422-8529-b7decaeb2d27", new DateTime(2022, 3, 22, 21, 51, 47, 61, DateTimeKind.Local).AddTicks(3541), "AQAAAAEAACcQAAAAEEXqeSzIzPFDpdC27Xa+rBMlBv3aTcR7TA4BPNACIWkZMeRWEyTNi8xd0h8EZQx7vQ==" });
        }
    }
}
