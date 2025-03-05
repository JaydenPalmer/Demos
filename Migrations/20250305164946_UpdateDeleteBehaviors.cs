using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Demos.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehaviors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Albums_AlbumId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tracks_TrackId",
                table: "Notes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6db17db7-cbf4-49f3-aa1f-9f4c0a866840", "AQAAAAIAAYagAAAAEFXt/S6+8UzsjKNYZjrx9dH7pincbLDZoDa+FKobNRFckEORrTWiTTuDv0vPSN4Lig==", "0e7f667f-206c-452b-9398-a18f85491f87" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a7d21fac-3b26-454d-a5c5-5a3ded62b960",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "06c1cfdc-5c61-473d-8185-8c0bf29d6052", "AQAAAAIAAYagAAAAEFV9DQ4XrKboMtfxcZq2Y6onKWXL3MEzIoCQHSIifGSd6y7zPND2WNyKU3tHc1MQkw==", "5ad9afa3-8ca2-41c5-beac-16e4087d2d1d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c81d7adc-5f52-4b4a-a88c-e8d087d5402f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "042bbc6b-0fd1-449b-8c77-97195caf70e0", "AQAAAAIAAYagAAAAEL7fsQMiChIoMqT4k2rNJaOtfvn9AmGnjNn9/mZNZ6NJJhQ+IJoEdCPsbl6V4UxItw==", "295f5d90-c974-44a1-ad6f-846c18e1edaf" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "51b039b3-e401-46a5-8345-378f9a4aa8c5", "AQAAAAIAAYagAAAAEKyp93mJ2KxgCMhSXAzJXKckmf/il0RzZcmEC9p83GvANNZJAfRqgUgUYWKY+nl8rg==", "25fab7ba-75e0-482a-a53f-6c6408eca199" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0f10242-8beb-48aa-8c03-9e5531d513af", "AQAAAAIAAYagAAAAEO7v/LG6Uj2H8+Rt+A4GBk1OeLfHoIdQX5QCb+/P0s0q2/w0SaErlnFBJfZq8D2nLg==", "30f18af5-3228-46c1-9a9e-cf4c77f3cdc1" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Albums_AlbumId",
                table: "Notes",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tracks_TrackId",
                table: "Notes",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Albums_AlbumId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_Tracks_TrackId",
                table: "Notes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "37b43af5-9a23-43e6-9a20-9b30c4d2c3a8", "AQAAAAIAAYagAAAAEIKo+iCMdqZTuzLboJvJUFWHMvOZ0ZArzxEHhro1ycQLwPkrEnalS2xJM6cYWrtGYA==", "58922c0b-e982-4d6f-a8d3-255fac7d35b5" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a7d21fac-3b26-454d-a5c5-5a3ded62b960",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "3a44cf78-6be2-4f3f-8d0b-76d0a66b67f5", "AQAAAAIAAYagAAAAEJEfqgcuHbVbmmaC+2511RQAXdvJX7r69RspE2RFypl/GtcMZnF5e8tkMRkWzPYvJA==", "d640c7a8-b382-4401-b762-a00c45568ade" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c81d7adc-5f52-4b4a-a88c-e8d087d5402f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "adc6fe5a-d9a9-4ac3-9a81-3b3c920ed446", "AQAAAAIAAYagAAAAECYcJcEdhoWOxsdbCndh2UBdkWikQc5xl7r0Y8zecDE4r+/ZRhAWBXYttGCJB4Bj5Q==", "c848959f-9a6a-4ab8-ab84-9ab824037cc8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4188364c-4815-4f05-8f34-1ae664da5905", "AQAAAAIAAYagAAAAEAILUToFdnSUvod+pxHQf6IU/zjhoOR80CB1G06SWOpSZ/2nJp141P/tkriJqnCIYw==", "06fd1052-0d58-4652-bd17-daa69bffd599" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fcda28e7-4ea2-44eb-8d76-d7100b437ccc", "AQAAAAIAAYagAAAAEOIIW4KHkydhqG5DzbOamEmG8gmaWrycHo2xntxgWn5ZYKOKRQkFKmAld9gjvt91iA==", "82d7edd8-7b68-47d7-961a-d92d493795ef" });

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Albums_AlbumId",
                table: "Notes",
                column: "AlbumId",
                principalTable: "Albums",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_Tracks_TrackId",
                table: "Notes",
                column: "TrackId",
                principalTable: "Tracks",
                principalColumn: "Id");
        }
    }
}
