using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorMovie.Server.Migrations
{
    public partial class D : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("c294babc-bed5-4402-adc0-d80bf48466ec"),
                column: "ConcurrencyStamp",
                value: "92202232-5294-4640-8885-2eb26ef1d201");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("cf8c7373-c04f-40a1-b1b7-64612eba45d8"),
                column: "ConcurrencyStamp",
                value: "052bc632-2398-49f3-a439-2a0dd36b98e0");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("d6fceefd-466a-4b02-b748-221c84112a42"),
                column: "ConcurrencyStamp",
                value: "3856ee1d-7ffb-4b5c-abab-bdf693af8189");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("219bb40e-0cab-4f08-a408-f33ecb138ed0"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "PasswordHash" },
                values: new object[] { "a6e25e16-1b41-410a-80b5-fc54ed158f8e", new DateTime(2022, 3, 22, 22, 55, 58, 325, DateTimeKind.Local).AddTicks(5523), "AQAAAAEAACcQAAAAEJ/o9V4Yjy3/a6d/ExBlGdC8qWGxpdBxpK9E7xVCzpPGiWs4X7XJ9eutG11NqnwENw==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("8aacfc8a-3418-46f4-9cf8-395fc5b90499"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "Name", "PasswordHash" },
                values: new object[] { "2e5653f5-3561-4ad0-9491-c2cff4bd8fa5", new DateTime(2022, 3, 22, 22, 55, 58, 327, DateTimeKind.Local).AddTicks(1175), "studio@thuyen.com", "AQAAAAEAACcQAAAAENUcVhz551EDgNpGGybJDednl/dx8dzoUcPgFlfIx6FG0TlmCgFMJxbCCsFcn3lz5g==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1e21bad7-34d0-491e-97f7-9d6277a0a561", new DateTime(2022, 3, 22, 22, 55, 58, 327, DateTimeKind.Local).AddTicks(1199), "customer@thuyen.com", "AQAAAAEAACcQAAAAEIrMnEAimyTT9e9euaSfrs6ay9DMbOSkHyW1Unb16S/Tx9yDk3SH1gt+qCeQsSLuRw==", "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "Name", "PasswordHash" },
                values: new object[] { "1821e7c3-330b-4f20-894b-1f0eac400f97", new DateTime(2022, 3, 22, 22, 11, 6, 570, DateTimeKind.Local).AddTicks(4987), "admin@thuyen.com", "AQAAAAEAACcQAAAAENttwqE/hZ8Rgg1x+zg5bnvxmNlnsi+eksul8SYyXF1b5Mp7t7aODlpnPueriOU0tg==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("c37a3f36-08b8-44ba-adda-85f3827811ba"),
                columns: new[] { "ConcurrencyStamp", "DateOfBirth", "Name", "PasswordHash", "SecurityStamp" },
                values: new object[] { "177c63c5-3c68-4ac9-93a5-a08281941b99", new DateTime(2022, 3, 22, 22, 11, 6, 570, DateTimeKind.Local).AddTicks(5021), "admin@thuyen.com", "AQAAAAEAACcQAAAAEBDxgcRVWiWRf6s8zX+oG3mroZJcMk8RewD1F9fHgJWDBWm07QlX+DdY1lhbnMSkHQ==", null });
        }
    }
}
