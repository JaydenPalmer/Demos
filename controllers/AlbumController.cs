using System.Security.Claims;
using System.Text;
using Demos.Data;
using Demos.Models;
using Demos.Models.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController : ControllerBase
{
    private DemosDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public AlbumController(DemosDbContext context)
    {
        _dbContext = context;
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateAlbumWithTracks(AlbumWithTracksDTO albumWithTracksDTO)
    {
        try
        {
            // Check if user exists
            var user = _dbContext.UserProfiles.Find(albumWithTracksDTO.Album.CreatorId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Create the album entity
            var newAlbum = new Album
            {
                Title = albumWithTracksDTO.Album.Title,
                CoverArtUrl = albumWithTracksDTO.Album.CoverArtUrl,
                Description = albumWithTracksDTO.Album.Description,
                IsComplete = albumWithTracksDTO.Album.IsComplete,
                CreatedDate = DateTime.Now,
                CreatorId = albumWithTracksDTO.Album.CreatorId,
            };

            _dbContext.Albums.Add(newAlbum);
            _dbContext.SaveChanges();

            // Now create all the tracks
            var createdTracks = new List<TrackDTO>();
            if (albumWithTracksDTO.Tracks != null && albumWithTracksDTO.Tracks.Count > 0)
            {
                for (int i = 0; i < albumWithTracksDTO.Tracks.Count; i++)
                {
                    var trackDTO = albumWithTracksDTO.Tracks[i];

                    // Create the track entity
                    var newTrack = new Track
                    {
                        Title = trackDTO.Title,
                        AudioUrl = trackDTO.AudioUrl,
                        CoverArtUrl = trackDTO.CoverArtUrl ?? newAlbum.CoverArtUrl,
                        PercentageDone =
                            trackDTO.PercentageDone > 100 ? 100 : trackDTO.PercentageDone,
                        Deadline = trackDTO.Deadline,
                        UploadDate = DateTime.Now,
                        IsComplete = trackDTO.PercentageDone >= 100 ? true : false,
                        CreatorId = trackDTO.CreatorId,
                    };

                    _dbContext.Tracks.Add(newTrack);
                    _dbContext.SaveChanges();

                    // Create the album-track relationship with track order
                    var albumTrack = new AlbumTrack
                    {
                        AlbumId = newAlbum.Id,
                        TrackId = newTrack.Id,
                        TrackOrder = i + 1, // Set track order based on array position
                    };
                    _dbContext.AlbumTracks.Add(albumTrack);

                    // Add instruments if provided
                    if (trackDTO.InstrumentIds != null && trackDTO.InstrumentIds.Count > 0)
                    {
                        foreach (var instrumentId in trackDTO.InstrumentIds)
                        {
                            // Check if instrument exists
                            if (_dbContext.Instruments.Any(i => i.Id == instrumentId))
                            {
                                var trackInstrument = new TrackInstrument
                                {
                                    TrackId = newTrack.Id,
                                    InstrumentId = instrumentId,
                                };
                                _dbContext.TrackInstruments.Add(trackInstrument);
                            }
                        }
                    }

                    // Add to created tracks list
                    createdTracks.Add(
                        new TrackDTO
                        {
                            Id = newTrack.Id,
                            Title = newTrack.Title,
                            AudioUrl = newTrack.AudioUrl,
                            CoverArtUrl = newTrack.CoverArtUrl,
                            PercentageDone = newTrack.PercentageDone,
                            Deadline = newTrack.Deadline,
                            UploadDate = newTrack.UploadDate,
                            IsComplete = newTrack.IsComplete,
                            Creator = new UserProfileDTO
                            {
                                Id = user.Id,
                                FirstName = user.FirstName,
                                LastName = user.LastName,
                                UserName = user.UserName,
                                Email = user.Email,
                                ProfileImage = user.ProfileImage,
                                IsArtist = user.IsArtist,
                            },
                            TrackOrder = i + 1,
                        }
                    );
                }

                _dbContext.SaveChanges(); // Save all the relationships at once
            }

            // Create a response that includes both the album and its tracks
            var response = new
            {
                Album = new AlbumDTO
                {
                    Id = newAlbum.Id,
                    Title = newAlbum.Title,
                    CoverArtUrl = newAlbum.CoverArtUrl,
                    IsComplete = newAlbum.IsComplete,
                    CreatedDate = newAlbum.CreatedDate,
                    Creator = new UserProfileDTO
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        UserName = user.UserName,
                        Email = user.Email,
                        ProfileImage = user.ProfileImage,
                        IsArtist = user.IsArtist,
                    },
                },
                Tracks = createdTracks,
            };

            return Created($"/api/Album/{newAlbum.Id}", response);
        }
        catch (Exception ex)
        {
            string innerExceptionMessage =
                ex.InnerException != null
                    ? ex.InnerException.Message
                    : "No inner exception details";
            return StatusCode(
                500,
                $"An error occurred while processing your request. Details: {innerExceptionMessage}"
            );
        }
    }
}
