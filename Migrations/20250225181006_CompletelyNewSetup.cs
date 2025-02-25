using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Demos.Migrations
{
    /// <inheritdoc />
    public partial class CompletelyNewSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: true),
                    SecurityStamp = table.Column<string>(type: "text", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InstrumentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstrumentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoleId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ClaimType = table.Column<string>(type: "text", nullable: true),
                    ClaimValue = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    ProviderKey = table.Column<string>(type: "text", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    RoleId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    LoginProvider = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    UserName = table.Column<string>(type: "text", nullable: false),
                    IsArtist = table.Column<bool>(type: "boolean", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProfileImage = table.Column<string>(type: "text", nullable: false),
                    IdentityUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_AspNetUsers_IdentityUserId",
                        column: x => x.IdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Instruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Instruments_InstrumentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "InstrumentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    PercentageDone = table.Column<int>(type: "integer", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    CoverArtUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Albums_UserProfiles_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    AudioUrl = table.Column<string>(type: "text", nullable: false),
                    PercentageDone = table.Column<int>(type: "integer", nullable: true),
                    UploadDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Deadline = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    IsComplete = table.Column<bool>(type: "boolean", nullable: false),
                    CoverArtUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tracks_UserProfiles_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumCollaborators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    UserProfileId = table.Column<int>(type: "integer", nullable: false),
                    CanAddTracks = table.Column<bool>(type: "boolean", nullable: false),
                    CanAddNotes = table.Column<bool>(type: "boolean", nullable: false),
                    AddedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumCollaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumCollaborators_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumCollaborators_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AlbumTracks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AlbumId = table.Column<int>(type: "integer", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    TrackOrder = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlbumTracks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AlbumTracks_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlbumTracks_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    TrackId = table.Column<int>(type: "integer", nullable: true),
                    AlbumId = table.Column<int>(type: "integer", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notes_Albums_AlbumId",
                        column: x => x.AlbumId,
                        principalTable: "Albums",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Notes_UserProfiles_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrackInstruments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TrackId = table.Column<int>(type: "integer", nullable: false),
                    InstrumentId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrackInstruments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrackInstruments_Instruments_InstrumentId",
                        column: x => x.InstrumentId,
                        principalTable: "Instruments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrackInstruments_Tracks_TrackId",
                        column: x => x.TrackId,
                        principalTable: "Tracks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8c26c17c-3649-4d48-a01d-65cbd7208758", null, "Artist", "ARTIST" },
                    { "9d592295-0d8e-443b-a80d-9f95d6c0afca", null, "Listener", "LISTENER" },
                    { "c3aaeb97-d2ba-4a53-a521-4eea61e59b35", null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e", 0, "433d1cd6-8aa3-4e04-acfa-15c53f404a17", "marcus@example.com", false, false, null, null, null, "AQAAAAIAAYagAAAAENtG0cM5aihzaWnv/lMsbQcX/yISagcAYicvQIdUxk+H8juQvmr7yyeawTCt/H27fQ==", null, false, "681ae5af-9206-477d-9e58-a3ccc10a0b0e", false, "marcus_j" },
                    { "a7d21fac-3b26-454d-a5c5-5a3ded62b960", 0, "6522a1cd-edb2-49e6-b7e6-395d0205646f", "daniel@example.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEAWCQzqIGfdHeWZh/qdVgT0D5n+2nlY1l7LlN8Udx2dt/ysXPcFKOqukLWdSJsPYmg==", null, false, "19f8cf49-3df0-41c6-b2b1-ddea43ba100e", false, "danielr" },
                    { "c81d7adc-5f52-4b4a-a88c-e8d087d5402f", 0, "0debcf42-b4c9-4a7b-afd7-2bbe2fff939a", "sara.c@example.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEJLY6bZ4kIjlOiHWYDI8+byoYoZu1+30whmyx1EeoFZEVcdQw5TFbTm3oHLBBj6o1g==", null, false, "0bce8933-12e0-463b-bb8a-d46a1b3b49e6", false, "sara_music" },
                    { "d8d76512-74f1-43bb-b1fd-87d3a8aa36df", 0, "aca1cff4-fb96-4638-974b-91c510197158", "melinda@example.com", false, false, null, null, null, "AQAAAAIAAYagAAAAEAeon1r81F6WgmxaKfHzyTDcFYUvna8xQaqmAvw0zW4EbO+3T48Mdtzj0lzvKBxdXA==", null, false, "bb7c0993-a58f-4cce-a827-c6ea8903e200", false, "melinda_j" },
                    { "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", 0, "72cd07f2-8558-4137-bc58-49e02bc0af88", "admina@strator.comx", false, false, null, null, null, "AQAAAAIAAYagAAAAEBqEB/rHiMgiTB4irmWmQlIyfSL7zpcfl3dd5CDn0fAlULXE+KqZiDCN+Kv66knwRg==", null, false, "c794eae9-5e82-4c00-a9e9-b5908574afd4", false, "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "InstrumentCategories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Instruments that produce sound from vibrating strings", "String Instruments" },
                    { 2, "Instruments that produce sound when struck or shaken", "Percussion" },
                    { 3, "Wind instruments that use wooden reeds", "Woodwind" },
                    { 4, "Wind instruments made of brass with a cup or funnel shaped mouthpiece", "Brass" },
                    { 5, "Instruments that produce sound using electronic circuits", "Electronic" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8c26c17c-3649-4d48-a01d-65cbd7208758", "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e" },
                    { "8c26c17c-3649-4d48-a01d-65cbd7208758", "a7d21fac-3b26-454d-a5c5-5a3ded62b960" },
                    { "8c26c17c-3649-4d48-a01d-65cbd7208758", "c81d7adc-5f52-4b4a-a88c-e8d087d5402f" },
                    { "8c26c17c-3649-4d48-a01d-65cbd7208758", "d8d76512-74f1-43bb-b1fd-87d3a8aa36df" },
                    { "9d592295-0d8e-443b-a80d-9f95d6c0afca", "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f" },
                    { "c3aaeb97-d2ba-4a53-a521-4eea61e59b35", "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f" }
                });

            migrationBuilder.InsertData(
                table: "Instruments",
                columns: new[] { "Id", "CategoryId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Six-stringed instrument played by strumming or plucking", "Acoustic Guitar" },
                    { 2, 1, "Six-stringed amplified instrument with solid body", "Electric Guitar" },
                    { 3, 1, "Four-stringed instrument that produces low-pitched tones", "Bass Guitar" },
                    { 4, 2, "Set of drums and cymbals arranged for convenient playing", "Drum Kit" },
                    { 5, 1, "Keyboard instrument with hammers that strike strings", "Piano" },
                    { 6, 3, "Single-reed woodwind instrument with curved metal body", "Saxophone" },
                    { 7, 5, "Electronic instrument that generates audio signals", "Synthesizer" },
                    { 8, 1, "Four-stringed instrument played with a bow", "Violin" },
                    { 9, 4, "Brass instrument with three valves and a bright tone", "Trumpet" },
                    { 10, 5, "Digital percussion controller that triggers sampled sounds", "Electronic Drum Pad" }
                });

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "Email", "FirstName", "IdentityUserId", "IsArtist", "JoinDate", "LastName", "ProfileImage", "UserName" },
                values: new object[,]
                {
                    { 1, "james@example.com", "James", "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", false, new DateTime(2023, 1, 15, 9, 30, 0, 0, DateTimeKind.Unspecified), "Wilson", "https://robohash.org/james.png?size=150x150&set=set4", "jameswilson" },
                    { 2, "melinda@example.com", "Melinda", "d8d76512-74f1-43bb-b1fd-87d3a8aa36df", true, new DateTime(2023, 3, 12, 14, 15, 0, 0, DateTimeKind.Unspecified), "Jackson", "https://robohash.org/melinda.png?size=150x150&set=set4", "melinda_j" },
                    { 3, "daniel@example.com", "Daniel", "a7d21fac-3b26-454d-a5c5-5a3ded62b960", true, new DateTime(2023, 2, 24, 11, 45, 0, 0, DateTimeKind.Unspecified), "Rodriguez", "https://robohash.org/daniel.png?size=150x150&set=set4", "danielr" },
                    { 4, "sara.c@example.com", "Sara", "c81d7adc-5f52-4b4a-a88c-e8d087d5402f", true, new DateTime(2023, 5, 5, 16, 20, 0, 0, DateTimeKind.Unspecified), "Chen", "https://robohash.org/sara.png?size=150x150&set=set4", "sara_music" },
                    { 5, "marcus@example.com", "Marcus", "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e", true, new DateTime(2023, 4, 18, 13, 10, 0, 0, DateTimeKind.Unspecified), "Johnson", "https://robohash.org/marcus.png?size=150x150&set=set4", "marcus_j" }
                });

            migrationBuilder.InsertData(
                table: "Albums",
                columns: new[] { "Id", "CoverArtUrl", "CreatedDate", "CreatorId", "Deadline", "Description", "IsComplete", "PercentageDone", "Title" },
                values: new object[,]
                {
                    { 1, "https://picsum.photos/seed/album1/500/500", new DateTime(2023, 5, 15, 9, 0, 0, 0, DateTimeKind.Unspecified), 2, null, "A collection of ambient tracks exploring dreamlike soundscapes", true, 100, "Ethereal Journeys" },
                    { 2, "https://picsum.photos/seed/album2/500/500", new DateTime(2023, 6, 20, 14, 30, 0, 0, DateTimeKind.Unspecified), 3, new DateTime(2023, 9, 1, 23, 59, 59, 0, DateTimeKind.Unspecified), "A narrative through the sounds of city life", false, 75, "Urban Chronicles" },
                    { 3, "https://picsum.photos/seed/album3/500/500", new DateTime(2023, 4, 10, 10, 15, 0, 0, DateTimeKind.Unspecified), 4, null, "Relaxing tracks inspired by ocean landscapes", true, 100, "Coastal Memories" },
                    { 4, "https://picsum.photos/seed/album4/500/500", new DateTime(2023, 7, 5, 16, 45, 0, 0, DateTimeKind.Unspecified), 5, new DateTime(2023, 10, 15, 23, 59, 59, 0, DateTimeKind.Unspecified), "Electronic music exploring futuristic themes", false, 60, "Digital Horizons" }
                });

            migrationBuilder.InsertData(
                table: "Tracks",
                columns: new[] { "Id", "AudioUrl", "CoverArtUrl", "CreatorId", "Deadline", "IsComplete", "PercentageDone", "Title", "UploadDate" },
                values: new object[,]
                {
                    { 1, "https://example.com/audio/sunset_dreams.mp3", "https://picsum.photos/seed/track1/300/300", 2, null, true, 100, "Sunset Dreams", new DateTime(2023, 6, 12, 15, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "https://example.com/audio/midnight_rain.mp3", "https://picsum.photos/seed/track2/300/300", 3, null, true, 100, "Midnight Rain", new DateTime(2023, 6, 15, 18, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "https://example.com/audio/urban_echoes.mp3", "https://picsum.photos/seed/track3/300/300", 2, new DateTime(2023, 8, 15, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 85, "Urban Echoes", new DateTime(2023, 7, 3, 10, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "https://example.com/audio/ocean_breeze.mp3", "https://picsum.photos/seed/track4/300/300", 4, null, true, 100, "Ocean Breeze", new DateTime(2023, 5, 28, 14, 20, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "https://example.com/audio/neon_city.mp3", "https://picsum.photos/seed/track5/300/300", 5, new DateTime(2023, 8, 30, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 70, "Neon City", new DateTime(2023, 7, 10, 11, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 6, "https://example.com/audio/mountain_high.mp3", "https://picsum.photos/seed/track6/300/300", 3, null, true, 100, "Mountain High", new DateTime(2023, 4, 15, 9, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 7, "https://example.com/audio/desert_wind.mp3", "https://picsum.photos/seed/track7/300/300", 4, new DateTime(2023, 9, 1, 23, 59, 59, 0, DateTimeKind.Unspecified), false, 50, "Desert Wind", new DateTime(2023, 7, 20, 16, 10, 0, 0, DateTimeKind.Unspecified) },
                    { 8, "https://example.com/audio/electric_dreams.mp3", "https://picsum.photos/seed/track8/300/300", 5, null, true, 100, "Electric Dreams", new DateTime(2023, 5, 5, 13, 25, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "AlbumCollaborators",
                columns: new[] { "Id", "AddedDate", "AlbumId", "CanAddNotes", "CanAddTracks", "UserProfileId" },
                values: new object[,]
                {
                    { 1, new DateTime(2023, 5, 16, 14, 30, 0, 0, DateTimeKind.Unspecified), 1, true, true, 3 },
                    { 2, new DateTime(2023, 5, 18, 10, 15, 0, 0, DateTimeKind.Unspecified), 1, true, false, 4 },
                    { 3, new DateTime(2023, 6, 22, 9, 45, 0, 0, DateTimeKind.Unspecified), 2, true, true, 2 },
                    { 4, new DateTime(2023, 6, 25, 16, 20, 0, 0, DateTimeKind.Unspecified), 2, true, true, 5 },
                    { 5, new DateTime(2023, 4, 15, 11, 30, 0, 0, DateTimeKind.Unspecified), 3, true, false, 2 },
                    { 6, new DateTime(2023, 7, 8, 13, 40, 0, 0, DateTimeKind.Unspecified), 4, true, true, 3 }
                });

            migrationBuilder.InsertData(
                table: "AlbumTracks",
                columns: new[] { "Id", "AlbumId", "TrackId", "TrackOrder" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 2, 1, 3, 2 },
                    { 3, 2, 2, 1 },
                    { 4, 2, 6, 2 },
                    { 5, 3, 4, 1 },
                    { 6, 3, 7, 2 },
                    { 7, 4, 5, 1 },
                    { 8, 4, 8, 2 }
                });

            migrationBuilder.InsertData(
                table: "Notes",
                columns: new[] { "Id", "AlbumId", "Content", "CreatedDate", "CreatorId", "TrackId" },
                values: new object[,]
                {
                    { 1, null, "Need to adjust the reverb on the second verse", new DateTime(2023, 7, 5, 11, 20, 0, 0, DateTimeKind.Unspecified), 2, 3 },
                    { 2, null, "Final mix approved by the whole team", new DateTime(2023, 6, 18, 15, 45, 0, 0, DateTimeKind.Unspecified), 3, 2 },
                    { 3, null, "Working on a new bridge section", new DateTime(2023, 7, 22, 9, 30, 0, 0, DateTimeKind.Unspecified), 4, 7 },
                    { 4, null, "Need to record additional synth layers", new DateTime(2023, 7, 15, 14, 10, 0, 0, DateTimeKind.Unspecified), 5, 5 },
                    { 5, 1, "Album ready for distribution", new DateTime(2023, 6, 1, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, null },
                    { 6, 2, "Still need to finalize the last two tracks", new DateTime(2023, 7, 10, 16, 35, 0, 0, DateTimeKind.Unspecified), 3, null },
                    { 7, 3, "Released on all streaming platforms", new DateTime(2023, 5, 20, 11, 45, 0, 0, DateTimeKind.Unspecified), 4, null },
                    { 8, 4, "Working on album artwork options", new DateTime(2023, 7, 25, 13, 15, 0, 0, DateTimeKind.Unspecified), 5, null }
                });

            migrationBuilder.InsertData(
                table: "TrackInstruments",
                columns: new[] { "Id", "InstrumentId", "TrackId" },
                values: new object[,]
                {
                    { 1, 1, 1 },
                    { 2, 5, 1 },
                    { 3, 2, 2 },
                    { 4, 4, 2 },
                    { 5, 7, 3 },
                    { 6, 1, 4 },
                    { 7, 8, 4 },
                    { 8, 7, 5 },
                    { 9, 10, 5 },
                    { 10, 6, 6 },
                    { 11, 9, 6 },
                    { 12, 1, 7 },
                    { 13, 7, 8 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlbumCollaborators_AlbumId",
                table: "AlbumCollaborators",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumCollaborators_UserProfileId",
                table: "AlbumCollaborators",
                column: "UserProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTracks_AlbumId",
                table: "AlbumTracks",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_AlbumTracks_TrackId",
                table: "AlbumTracks",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Albums_CreatorId",
                table: "Albums",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Instruments_CategoryId",
                table: "Instruments",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AlbumId",
                table: "Notes",
                column: "AlbumId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_CreatorId",
                table: "Notes",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_TrackId",
                table: "Notes",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackInstruments_InstrumentId",
                table: "TrackInstruments",
                column: "InstrumentId");

            migrationBuilder.CreateIndex(
                name: "IX_TrackInstruments_TrackId",
                table: "TrackInstruments",
                column: "TrackId");

            migrationBuilder.CreateIndex(
                name: "IX_Tracks_CreatorId",
                table: "Tracks",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_IdentityUserId",
                table: "UserProfiles",
                column: "IdentityUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlbumCollaborators");

            migrationBuilder.DropTable(
                name: "AlbumTracks");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "TrackInstruments");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Instruments");

            migrationBuilder.DropTable(
                name: "Tracks");

            migrationBuilder.DropTable(
                name: "InstrumentCategories");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
