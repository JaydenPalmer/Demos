using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demos.Migrations
{
    /// <inheritdoc />
    public partial class AddStandaloneTracks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "AudioUrl", "CoverArtUrl", "CreatorId", "Deadline", "IsComplete", "PercentageDone", "Title", "UploadDate" },
                values: new object[,]
                {
                    { 9, "https://example.com/audio/solo_flight.mp3", "https://picsum.photos/seed/track9/300/300", 2, null, true, 100, "Solo Flight", new DateTime(2023, 7, 25, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 10, "https://example.com/audio/midnight_jazz.mp3", "https://picsum.photos/seed/track10/300/300", 3, null, true, 100, "Midnight Jazz", new DateTime(2023, 7, 28, 14, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 11, "https://example.com/audio/morning_meditation.mp3", "https://picsum.photos/seed/track11/300/300", 4, new DateTime(2023, 8, 15, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 90, "Morning Meditation", new DateTime(2023, 7, 22, 9, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 12, "https://example.com/audio/urban_beat.mp3", "https://picsum.photos/seed/track12/300/300", 5, null, true, 100, "Urban Beat", new DateTime(2023, 7, 15, 16, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 13, "https://example.com/audio/rainy_day.mp3", "https://picsum.photos/seed/track13/300/300", 2, new DateTime(2023, 8, 20, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 75, "Rainy Day", new DateTime(2023, 7, 18, 11, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 14, "https://example.com/audio/desert_sands.mp3", "https://picsum.photos/seed/track14/300/300", 3, null, true, 100, "Desert Sands", new DateTime(2023, 7, 10, 13, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 15, "https://example.com/audio/acoustic_session.mp3", "https://picsum.photos/seed/track15/300/300", 4, null, true, 100, "Acoustic Session", new DateTime(2023, 7, 5, 15, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 16, "https://example.com/audio/future_nostalgia.mp3", "https://picsum.photos/seed/track16/300/300", 5, new DateTime(2023, 9, 1, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 60, "Future Nostalgia", new DateTime(2023, 7, 20, 10, 30, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "TrackInstruments",
                columns: new[] { "Id", "InstrumentId", "TrackId" },
                values: new object[,]
                {
                    { 14, 5, 9 },
                    { 15, 6, 10 },
                    { 16, 3, 10 },
                    { 17, 1, 11 },
                    { 18, 4, 12 },
                    { 19, 7, 12 },
                    { 20, 5, 13 },
                    { 21, 1, 14 },
                    { 22, 1, 15 },
                    { 23, 8, 15 },
                    { 24, 7, 16 },
                    { 25, 10, 16 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "TrackInstruments",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Tracks",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "433d1cd6-8aa3-4e04-acfa-15c53f404a17", "AQAAAAIAAYagAAAAENtG0cM5aihzaWnv/lMsbQcX/yISagcAYicvQIdUxk+H8juQvmr7yyeawTCt/H27fQ==", "681ae5af-9206-477d-9e58-a3ccc10a0b0e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a7d21fac-3b26-454d-a5c5-5a3ded62b960",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6522a1cd-edb2-49e6-b7e6-395d0205646f", "AQAAAAIAAYagAAAAEAWCQzqIGfdHeWZh/qdVgT0D5n+2nlY1l7LlN8Udx2dt/ysXPcFKOqukLWdSJsPYmg==", "19f8cf49-3df0-41c6-b2b1-ddea43ba100e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "c81d7adc-5f52-4b4a-a88c-e8d087d5402f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0debcf42-b4c9-4a7b-afd7-2bbe2fff939a", "AQAAAAIAAYagAAAAEJLY6bZ4kIjlOiHWYDI8+byoYoZu1+30whmyx1EeoFZEVcdQw5TFbTm3oHLBBj6o1g==", "0bce8933-12e0-463b-bb8a-d46a1b3b49e6" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d8d76512-74f1-43bb-b1fd-87d3a8aa36df",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aca1cff4-fb96-4638-974b-91c510197158", "AQAAAAIAAYagAAAAEAeon1r81F6WgmxaKfHzyTDcFYUvna8xQaqmAvw0zW4EbO+3T48Mdtzj0lzvKBxdXA==", "bb7c0993-a58f-4cce-a827-c6ea8903e200" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "72cd07f2-8558-4137-bc58-49e02bc0af88", "AQAAAAIAAYagAAAAEBqEB/rHiMgiTB4irmWmQlIyfSL7zpcfl3dd5CDn0fAlULXE+KqZiDCN+Kv66knwRg==", "c794eae9-5e82-4c00-a9e9-b5908574afd4" });
        }
    }
}
