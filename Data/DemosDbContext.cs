using Demos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Demos.Data;

public class DemosDbContext : IdentityDbContext<IdentityUser>
{
    private readonly IConfiguration _configuration;
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Album> Albums { get; set; }
    public DbSet<AlbumTrack> AlbumTracks { get; set; }
    public DbSet<Instrument> Instruments { get; set; }
    public DbSet<InstrumentCategory> InstrumentCategories { get; set; }
    public DbSet<TrackInstrument> TrackInstruments { get; set; }
    public DbSet<Note> Notes { get; set; }
    public DbSet<AlbumCollaborator> AlbumCollaborators { get; set; }

    public DemosDbContext(DbContextOptions<DemosDbContext> context, IConfiguration config)
        : base(context)
    {
        _configuration = config;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .Entity<IdentityRole>()
            .HasData(
                new IdentityRole
                {
                    Id = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },
                new IdentityRole
                {
                    Id = "8c26c17c-3649-4d48-a01d-65cbd7208758",
                    Name = "Artist",
                    NormalizedName = "ARTIST",
                },
                new IdentityRole
                {
                    Id = "9d592295-0d8e-443b-a80d-9f95d6c0afca",
                    Name = "Listener",
                    NormalizedName = "LISTENER",
                }
            );

        modelBuilder
            .Entity<IdentityUser>()
            .HasData(
                // Keep your existing admin user
                new IdentityUser
                {
                    Id = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f",
                    UserName = "Administrator",
                    Email = "admina@strator.comx",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        null,
                        _configuration["AdminPassword"]
                    ),
                },
                // Add new identity users for other profiles
                new IdentityUser
                {
                    Id = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df", // New ID for Melinda
                    UserName = "melinda_j",
                    Email = "melinda@example.com",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        null,
                        _configuration["AdminPassword"]
                    ),
                },
                new IdentityUser
                {
                    Id = "a7d21fac-3b26-454d-a5c5-5a3ded62b960", // New ID for Daniel
                    UserName = "danielr",
                    Email = "daniel@example.com",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        null,
                        _configuration["AdminPassword"]
                    ),
                },
                new IdentityUser
                {
                    Id = "c81d7adc-5f52-4b4a-a88c-e8d087d5402f", // New ID for Sara
                    UserName = "sara_music",
                    Email = "sara.c@example.com",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        null,
                        _configuration["AdminPassword"]
                    ),
                },
                new IdentityUser
                {
                    Id = "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e", // New ID for Marcus
                    UserName = "marcus_j",
                    Email = "marcus@example.com",
                    PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(
                        null,
                        _configuration["AdminPassword"]
                    ),
                }
            );

        modelBuilder
            .Entity<IdentityUserRole<string>>()
            .HasData(
                // Admin role
                new IdentityUserRole<string>
                {
                    RoleId = "c3aaeb97-d2ba-4a53-a521-4eea61e59b35",
                    UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", // James/Admin
                },
                // Listener role for James (non-artist)
                new IdentityUserRole<string>
                {
                    RoleId = "9d592295-0d8e-443b-a80d-9f95d6c0afca", // Listener role
                    UserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", // James/Admin
                },
                // Artist roles for the other users
                new IdentityUserRole<string>
                {
                    RoleId = "8c26c17c-3649-4d48-a01d-65cbd7208758", // Artist role
                    UserId = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df", // Melinda
                },
                new IdentityUserRole<string>
                {
                    RoleId = "8c26c17c-3649-4d48-a01d-65cbd7208758", // Artist role
                    UserId = "a7d21fac-3b26-454d-a5c5-5a3ded62b960", // Daniel
                },
                new IdentityUserRole<string>
                {
                    RoleId = "8c26c17c-3649-4d48-a01d-65cbd7208758", // Artist role
                    UserId = "c81d7adc-5f52-4b4a-a88c-e8d087d5402f", // Sara
                },
                new IdentityUserRole<string>
                {
                    RoleId = "8c26c17c-3649-4d48-a01d-65cbd7208758", // Artist role
                    UserId = "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e", // Marcus
                }
            );

        // UserProfile seed data
        modelBuilder
            .Entity<UserProfile>()
            .HasData(
                new UserProfile
                {
                    Id = 1,
                    IdentityUserId = "dbc40bc6-0829-4ac5-a3ed-180f5e916a5f", // James/Admin
                    FirstName = "James",
                    LastName = "Wilson",
                    Email = "james@example.com",
                    UserName = "jameswilson",
                    IsArtist = false,
                    JoinDate = new DateTime(2023, 1, 15, 9, 30, 0),
                    ProfileImage = "https://robohash.org/james.png?size=150x150&set=set4",
                },
                new UserProfile
                {
                    Id = 2,
                    IdentityUserId = "d8d76512-74f1-43bb-b1fd-87d3a8aa36df", // Melinda
                    FirstName = "Melinda",
                    LastName = "Jackson",
                    Email = "melinda@example.com",
                    UserName = "melinda_j",
                    IsArtist = true,
                    JoinDate = new DateTime(2023, 3, 12, 14, 15, 0),
                    ProfileImage = "https://robohash.org/melinda.png?size=150x150&set=set4",
                },
                // Update the other UserProfiles with matching IDs...
                new UserProfile
                {
                    Id = 3,
                    IdentityUserId = "a7d21fac-3b26-454d-a5c5-5a3ded62b960", // Daniel
                    FirstName = "Daniel",
                    LastName = "Rodriguez",
                    Email = "daniel@example.com",
                    UserName = "danielr",
                    IsArtist = true,
                    JoinDate = new DateTime(2023, 2, 24, 11, 45, 0),
                    ProfileImage = "https://robohash.org/daniel.png?size=150x150&set=set4",
                },
                new UserProfile
                {
                    Id = 4,
                    IdentityUserId = "c81d7adc-5f52-4b4a-a88c-e8d087d5402f", // Sara
                    FirstName = "Sara",
                    LastName = "Chen",
                    Email = "sara.c@example.com",
                    UserName = "sara_music",
                    IsArtist = true,
                    JoinDate = new DateTime(2023, 5, 5, 16, 20, 0),
                    ProfileImage = "https://robohash.org/sara.png?size=150x150&set=set4",
                },
                new UserProfile
                {
                    Id = 5,
                    IdentityUserId = "9b2f41e3-1ba9-4df4-ae95-a0cf2c561b7e", // Marcus
                    FirstName = "Marcus",
                    LastName = "Johnson",
                    Email = "marcus@example.com",
                    UserName = "marcus_j",
                    IsArtist = true,
                    JoinDate = new DateTime(2023, 4, 18, 13, 10, 0),
                    ProfileImage = "https://robohash.org/marcus.png?size=150x150&set=set4",
                }
            );

        // InstrumentCategory seed data
        modelBuilder
            .Entity<InstrumentCategory>()
            .HasData(
                new InstrumentCategory
                {
                    Id = 1,
                    Name = "String Instruments",
                    Description = "Instruments that produce sound from vibrating strings",
                },
                new InstrumentCategory
                {
                    Id = 2,
                    Name = "Percussion",
                    Description = "Instruments that produce sound when struck or shaken",
                },
                new InstrumentCategory
                {
                    Id = 3,
                    Name = "Woodwind",
                    Description = "Wind instruments that use wooden reeds",
                },
                new InstrumentCategory
                {
                    Id = 4,
                    Name = "Brass",
                    Description =
                        "Wind instruments made of brass with a cup or funnel shaped mouthpiece",
                },
                new InstrumentCategory
                {
                    Id = 5,
                    Name = "Electronic",
                    Description = "Instruments that produce sound using electronic circuits",
                }
            );

        // Instrument seed data
        modelBuilder
            .Entity<Instrument>()
            .HasData(
                new Instrument
                {
                    Id = 1,
                    Name = "Acoustic Guitar",
                    CategoryId = 1,
                    Description = "Six-stringed instrument played by strumming or plucking",
                },
                new Instrument
                {
                    Id = 2,
                    Name = "Electric Guitar",
                    CategoryId = 1,
                    Description = "Six-stringed amplified instrument with solid body",
                },
                new Instrument
                {
                    Id = 3,
                    Name = "Bass Guitar",
                    CategoryId = 1,
                    Description = "Four-stringed instrument that produces low-pitched tones",
                },
                new Instrument
                {
                    Id = 4,
                    Name = "Drum Kit",
                    CategoryId = 2,
                    Description = "Set of drums and cymbals arranged for convenient playing",
                },
                new Instrument
                {
                    Id = 5,
                    Name = "Piano",
                    CategoryId = 1,
                    Description = "Keyboard instrument with hammers that strike strings",
                },
                new Instrument
                {
                    Id = 6,
                    Name = "Saxophone",
                    CategoryId = 3,
                    Description = "Single-reed woodwind instrument with curved metal body",
                },
                new Instrument
                {
                    Id = 7,
                    Name = "Synthesizer",
                    CategoryId = 5,
                    Description = "Electronic instrument that generates audio signals",
                },
                new Instrument
                {
                    Id = 8,
                    Name = "Violin",
                    CategoryId = 1,
                    Description = "Four-stringed instrument played with a bow",
                },
                new Instrument
                {
                    Id = 9,
                    Name = "Trumpet",
                    CategoryId = 4,
                    Description = "Brass instrument with three valves and a bright tone",
                },
                new Instrument
                {
                    Id = 10,
                    Name = "Electronic Drum Pad",
                    CategoryId = 5,
                    Description = "Digital percussion controller that triggers sampled sounds",
                }
            );

        // Track seed data
        modelBuilder
            .Entity<Track>()
            .HasData(
                new Track
                {
                    Id = 1,
                    CreatorId = 2,
                    Title = "Sunset Dreams",
                    AudioUrl = "https://example.com/audio/sunset_dreams.mp3",
                    PercentageDone = 100,
                    UploadDate = new DateTime(2023, 6, 12, 15, 30, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/track1/300/300",
                },
                new Track
                {
                    Id = 2,
                    CreatorId = 3,
                    Title = "Midnight Rain",
                    AudioUrl = "https://example.com/audio/midnight_rain.mp3",
                    PercentageDone = 100,
                    UploadDate = new DateTime(2023, 6, 15, 18, 45, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/track2/300/300",
                },
                new Track
                {
                    Id = 3,
                    CreatorId = 2,
                    Title = "Urban Echoes",
                    AudioUrl = "https://example.com/audio/urban_echoes.mp3",
                    PercentageDone = 85,
                    UploadDate = new DateTime(2023, 7, 3, 10, 15, 0),
                    Deadline = new DateTime(2023, 8, 15, 23, 59, 59),
                    IsComplete = false,
                    CoverArtUrl = "https://picsum.photos/seed/track3/300/300",
                },
                new Track
                {
                    Id = 4,
                    CreatorId = 4,
                    Title = "Ocean Breeze",
                    AudioUrl = "https://example.com/audio/ocean_breeze.mp3",
                    PercentageDone = 100,
                    UploadDate = new DateTime(2023, 5, 28, 14, 20, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/track4/300/300",
                },
                new Track
                {
                    Id = 5,
                    CreatorId = 5,
                    Title = "Neon City",
                    AudioUrl = "https://example.com/audio/neon_city.mp3",
                    PercentageDone = 70,
                    UploadDate = new DateTime(2023, 7, 10, 11, 30, 0),
                    Deadline = new DateTime(2023, 8, 30, 23, 59, 59),
                    IsComplete = false,
                    CoverArtUrl = "https://picsum.photos/seed/track5/300/300",
                },
                new Track
                {
                    Id = 6,
                    CreatorId = 3,
                    Title = "Mountain High",
                    AudioUrl = "https://example.com/audio/mountain_high.mp3",
                    PercentageDone = 100,
                    UploadDate = new DateTime(2023, 4, 15, 9, 45, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/track6/300/300",
                },
                new Track
                {
                    Id = 7,
                    CreatorId = 4,
                    Title = "Desert Wind",
                    AudioUrl = "https://example.com/audio/desert_wind.mp3",
                    PercentageDone = 50,
                    UploadDate = new DateTime(2023, 7, 20, 16, 10, 0),
                    Deadline = new DateTime(2023, 9, 1, 23, 59, 59),
                    IsComplete = false,
                    CoverArtUrl = "https://picsum.photos/seed/track7/300/300",
                },
                new Track
                {
                    Id = 8,
                    CreatorId = 5,
                    Title = "Electric Dreams",
                    AudioUrl = "https://example.com/audio/electric_dreams.mp3",
                    PercentageDone = 100,
                    UploadDate = new DateTime(2023, 5, 5, 13, 25, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/track8/300/300",
                }
            );

        // Album seed data
        modelBuilder
            .Entity<Album>()
            .HasData(
                new Album
                {
                    Id = 1,
                    CreatorId = 2,
                    Title = "Ethereal Journeys",
                    PercentageDone = 100,
                    Description = "A collection of ambient tracks exploring dreamlike soundscapes",
                    CreatedDate = new DateTime(2023, 5, 15, 9, 0, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/album1/500/500",
                },
                new Album
                {
                    Id = 2,
                    CreatorId = 3,
                    Title = "Urban Chronicles",
                    PercentageDone = 75,
                    Description = "A narrative through the sounds of city life",
                    CreatedDate = new DateTime(2023, 6, 20, 14, 30, 0),
                    Deadline = new DateTime(2023, 9, 1, 23, 59, 59),
                    IsComplete = false,
                    CoverArtUrl = "https://picsum.photos/seed/album2/500/500",
                },
                new Album
                {
                    Id = 3,
                    CreatorId = 4,
                    Title = "Coastal Memories",
                    PercentageDone = 100,
                    Description = "Relaxing tracks inspired by ocean landscapes",
                    CreatedDate = new DateTime(2023, 4, 10, 10, 15, 0),
                    IsComplete = true,
                    CoverArtUrl = "https://picsum.photos/seed/album3/500/500",
                },
                new Album
                {
                    Id = 4,
                    CreatorId = 5,
                    Title = "Digital Horizons",
                    PercentageDone = 60,
                    Description = "Electronic music exploring futuristic themes",
                    CreatedDate = new DateTime(2023, 7, 5, 16, 45, 0),
                    Deadline = new DateTime(2023, 10, 15, 23, 59, 59),
                    IsComplete = false,
                    CoverArtUrl = "https://picsum.photos/seed/album4/500/500",
                }
            );

        // AlbumTrack seed data
        modelBuilder
            .Entity<AlbumTrack>()
            .HasData(
                new AlbumTrack
                {
                    Id = 1,
                    AlbumId = 1,
                    TrackId = 1,
                    TrackOrder = 1,
                },
                new AlbumTrack
                {
                    Id = 2,
                    AlbumId = 1,
                    TrackId = 3,
                    TrackOrder = 2,
                },
                new AlbumTrack
                {
                    Id = 3,
                    AlbumId = 2,
                    TrackId = 2,
                    TrackOrder = 1,
                },
                new AlbumTrack
                {
                    Id = 4,
                    AlbumId = 2,
                    TrackId = 6,
                    TrackOrder = 2,
                },
                new AlbumTrack
                {
                    Id = 5,
                    AlbumId = 3,
                    TrackId = 4,
                    TrackOrder = 1,
                },
                new AlbumTrack
                {
                    Id = 6,
                    AlbumId = 3,
                    TrackId = 7,
                    TrackOrder = 2,
                },
                new AlbumTrack
                {
                    Id = 7,
                    AlbumId = 4,
                    TrackId = 5,
                    TrackOrder = 1,
                },
                new AlbumTrack
                {
                    Id = 8,
                    AlbumId = 4,
                    TrackId = 8,
                    TrackOrder = 2,
                }
            );

        // TrackInstrument seed data
        modelBuilder
            .Entity<TrackInstrument>()
            .HasData(
                new TrackInstrument
                {
                    Id = 1,
                    TrackId = 1,
                    InstrumentId = 1,
                },
                new TrackInstrument
                {
                    Id = 2,
                    TrackId = 1,
                    InstrumentId = 5,
                },
                new TrackInstrument
                {
                    Id = 3,
                    TrackId = 2,
                    InstrumentId = 2,
                },
                new TrackInstrument
                {
                    Id = 4,
                    TrackId = 2,
                    InstrumentId = 4,
                },
                new TrackInstrument
                {
                    Id = 5,
                    TrackId = 3,
                    InstrumentId = 7,
                },
                new TrackInstrument
                {
                    Id = 6,
                    TrackId = 4,
                    InstrumentId = 1,
                },
                new TrackInstrument
                {
                    Id = 7,
                    TrackId = 4,
                    InstrumentId = 8,
                },
                new TrackInstrument
                {
                    Id = 8,
                    TrackId = 5,
                    InstrumentId = 7,
                },
                new TrackInstrument
                {
                    Id = 9,
                    TrackId = 5,
                    InstrumentId = 10,
                },
                new TrackInstrument
                {
                    Id = 10,
                    TrackId = 6,
                    InstrumentId = 6,
                },
                new TrackInstrument
                {
                    Id = 11,
                    TrackId = 6,
                    InstrumentId = 9,
                },
                new TrackInstrument
                {
                    Id = 12,
                    TrackId = 7,
                    InstrumentId = 1,
                },
                new TrackInstrument
                {
                    Id = 13,
                    TrackId = 8,
                    InstrumentId = 7,
                }
            );

        // Note seed data
        modelBuilder
            .Entity<Note>()
            .HasData(
                new Note
                {
                    Id = 1,
                    CreatorId = 2,
                    TrackId = 3,
                    Content = "Need to adjust the reverb on the second verse",
                    CreatedDate = new DateTime(2023, 7, 5, 11, 20, 0),
                },
                new Note
                {
                    Id = 2,
                    CreatorId = 3,
                    TrackId = 2,
                    Content = "Final mix approved by the whole team",
                    CreatedDate = new DateTime(2023, 6, 18, 15, 45, 0),
                },
                new Note
                {
                    Id = 3,
                    CreatorId = 4,
                    TrackId = 7,
                    Content = "Working on a new bridge section",
                    CreatedDate = new DateTime(2023, 7, 22, 9, 30, 0),
                },
                new Note
                {
                    Id = 4,
                    CreatorId = 5,
                    TrackId = 5,
                    Content = "Need to record additional synth layers",
                    CreatedDate = new DateTime(2023, 7, 15, 14, 10, 0),
                },
                new Note
                {
                    Id = 5,
                    CreatorId = 2,
                    AlbumId = 1,
                    Content = "Album ready for distribution",
                    CreatedDate = new DateTime(2023, 6, 1, 10, 0, 0),
                },
                new Note
                {
                    Id = 6,
                    CreatorId = 3,
                    AlbumId = 2,
                    Content = "Still need to finalize the last two tracks",
                    CreatedDate = new DateTime(2023, 7, 10, 16, 35, 0),
                },
                new Note
                {
                    Id = 7,
                    CreatorId = 4,
                    AlbumId = 3,
                    Content = "Released on all streaming platforms",
                    CreatedDate = new DateTime(2023, 5, 20, 11, 45, 0),
                },
                new Note
                {
                    Id = 8,
                    CreatorId = 5,
                    AlbumId = 4,
                    Content = "Working on album artwork options",
                    CreatedDate = new DateTime(2023, 7, 25, 13, 15, 0),
                }
            );

        // AlbumCollaborator seed data
        modelBuilder
            .Entity<AlbumCollaborator>()
            .HasData(
                new AlbumCollaborator
                {
                    Id = 1,
                    AlbumId = 1,
                    UserProfileId = 3,
                    CanAddTracks = true,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 5, 16, 14, 30, 0),
                },
                new AlbumCollaborator
                {
                    Id = 2,
                    AlbumId = 1,
                    UserProfileId = 4,
                    CanAddTracks = false,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 5, 18, 10, 15, 0),
                },
                new AlbumCollaborator
                {
                    Id = 3,
                    AlbumId = 2,
                    UserProfileId = 2,
                    CanAddTracks = true,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 6, 22, 9, 45, 0),
                },
                new AlbumCollaborator
                {
                    Id = 4,
                    AlbumId = 2,
                    UserProfileId = 5,
                    CanAddTracks = true,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 6, 25, 16, 20, 0),
                },
                new AlbumCollaborator
                {
                    Id = 5,
                    AlbumId = 3,
                    UserProfileId = 2,
                    CanAddTracks = false,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 4, 15, 11, 30, 0),
                },
                new AlbumCollaborator
                {
                    Id = 6,
                    AlbumId = 4,
                    UserProfileId = 3,
                    CanAddTracks = true,
                    CanAddNotes = true,
                    AddedDate = new DateTime(2023, 7, 8, 13, 40, 0),
                }
            );
    }
}
