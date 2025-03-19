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
public class TrackController : ControllerBase
{
    private DemosDbContext _dbContext;
    private UserManager<IdentityUser> _userManager;

    public TrackController(DemosDbContext context)
    {
        _dbContext = context;
    }

    [HttpGet]
    [Authorize]
    public IActionResult GetAll(
        [FromQuery] int? userId,
        [FromQuery] int? trackId,
        [FromQuery] int? albumId
    )
    {
        try
        {
            //base query getting all of everything
            IQueryable<Track> query = _dbContext
                .Tracks.Include(t => t.Creator)
                .Include(t => t.AlbumTracks)
                .ThenInclude(at => at.Album)
                .ThenInclude(a => a.Collaborators)
                .ThenInclude(c => c.UserProfile)
                .Include(t => t.TrackInstruments)
                .ThenInclude(ti => ti.Instrument);

            // If a specific track ID is requested
            if (trackId.HasValue)
            {
                var track = query.FirstOrDefault(t => t.Id == trackId.Value);

                if (track == null)
                {
                    return NotFound($"Track with ID {trackId.Value} not found");
                }

                return Ok(
                    new TrackDTO
                    {
                        Id = track.Id,
                        Title = track.Title,
                        AudioUrl = track.AudioUrl,
                        PercentageDone = track.PercentageDone,
                        UploadDate = track.UploadDate,
                        Deadline = track.Deadline,
                        IsComplete = track.IsComplete,
                        CoverArtUrl = track.CoverArtUrl,

                        Creator = new UserProfileDTO
                        {
                            Id = track.Creator.Id,
                            FirstName = track.Creator.FirstName,
                            LastName = track.Creator.LastName,
                            UserName = track.Creator.UserName,
                            Email = track.Creator.Email,
                            ProfileImage = track.Creator.ProfileImage,
                            IsArtist = track.Creator.IsArtist,
                        },

                        Album =
                            track.AlbumTracks.FirstOrDefault() != null
                                ? new AlbumDTO
                                {
                                    Id = track.AlbumTracks.FirstOrDefault().Album.Id,
                                    Title = track.AlbumTracks.FirstOrDefault().Album.Title,
                                    CoverArtUrl = track
                                        .AlbumTracks.FirstOrDefault()
                                        .Album.CoverArtUrl,
                                    PercentageDone = track
                                        .AlbumTracks.FirstOrDefault()
                                        .Album.PercentageDone,
                                    IsComplete = track
                                        .AlbumTracks.FirstOrDefault()
                                        .Album.IsComplete,
                                    TrackOrder = track.AlbumTracks.FirstOrDefault().TrackOrder,
                                }
                                : null,

                        Instruments = track
                            .TrackInstruments.Select(ti => new InstrumentDTO
                            {
                                Id = ti.Instrument.Id,
                                Name = ti.Instrument.Name,
                                Description = ti.Instrument.Description,
                                CategoryId = ti.Instrument.CategoryId,
                                CategoryName = ti.Instrument.Category?.Name,
                            })
                            .ToList(),
                    }
                );
            }

            // If a specific album ID is requested
            if (albumId.HasValue)
            {
                var album = _dbContext
                    .Albums.Include(a => a.Creator)
                    .Include(a => a.AlbumTracks)
                    .ThenInclude(at => at.Track)
                    .ThenInclude(t => t.TrackInstruments)
                    .ThenInclude(ti => ti.Instrument)
                    .FirstOrDefault(a => a.Id == albumId.Value);

                if (album == null)
                {
                    return NotFound($"Album with ID {albumId.Value} not found");
                }

                return Ok(
                    new AlbumDetailDTO
                    {
                        Id = album.Id,
                        Title = album.Title,
                        Description = album.Description,
                        CoverArtUrl = album.CoverArtUrl,
                        PercentageDone = album.PercentageDone,
                        CreatedDate = album.CreatedDate,
                        Deadline = album.Deadline,
                        IsComplete = album.IsComplete,

                        Creator = new UserProfileDTO
                        {
                            Id = album.Creator.Id,
                            FirstName = album.Creator.FirstName,
                            LastName = album.Creator.LastName,
                            UserName = album.Creator.UserName,
                            Email = album.Creator.Email,
                            ProfileImage = album.Creator.ProfileImage,
                            IsArtist = album.Creator.IsArtist,
                        },

                        Tracks = album
                            .AlbumTracks.OrderBy(at => at.TrackOrder)
                            .Select(at => new TrackDTO
                            {
                                Id = at.Track.Id,
                                Title = at.Track.Title,
                                AudioUrl = at.Track.AudioUrl,
                                PercentageDone = at.Track.PercentageDone,
                                UploadDate = at.Track.UploadDate,
                                Deadline = at.Track.Deadline,
                                IsComplete = at.Track.IsComplete,
                                CoverArtUrl = at.Track.CoverArtUrl,
                                TrackOrder = at.TrackOrder,

                                Instruments = at
                                    .Track.TrackInstruments.Select(ti => new InstrumentDTO
                                    {
                                        Id = ti.Instrument.Id,
                                        Name = ti.Instrument.Name,
                                        CategoryId = ti.Instrument.CategoryId,
                                    })
                                    .ToList(),
                            })
                            .ToList(),
                    }
                );
            }

            // If userId is provided, filter by user as before
            if (userId.HasValue)
            {
                //check to see if user exists
                bool userExists = _dbContext.UserProfiles.Any(u => u.Id == userId.Value);
                if (!userExists)
                {
                    return NotFound("User does not exist.");
                }

                //getting the tracks the user owns
                var ownedTracks = query.Where(t => t.CreatorId == userId);

                //getting albums where the user is a collaborator
                var collaborativeAlbumIds = _dbContext
                    .AlbumCollaborators.Where(ac => ac.UserProfileId == userId.Value)
                    .Select(ac => ac.AlbumId);

                //getting the tracks that are a part of that album
                var collaborativeTracks = query.Where(t =>
                    t.AlbumTracks.Any(at => collaborativeAlbumIds.Contains(at.AlbumId))
                );

                //combining owned and collaborative tracks
                query = ownedTracks.Union(collaborativeTracks);
            }

            query = query.OrderByDescending(t => t.UploadDate);

            return Ok(
                query.Select(t => new TrackDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    AudioUrl = t.AudioUrl,
                    PercentageDone = t.PercentageDone,
                    UploadDate = t.UploadDate,
                    Deadline = t.Deadline,
                    IsComplete = t.IsComplete,
                    CoverArtUrl = t.CoverArtUrl,

                    Creator = new UserProfileDTO
                    {
                        Id = t.Creator.Id,
                        FirstName = t.Creator.FirstName,
                        LastName = t.Creator.LastName,
                        UserName = t.Creator.UserName,
                        Email = t.Creator.Email,
                        ProfileImage = t.Creator.ProfileImage,
                        IsArtist = t.Creator.IsArtist,
                    },

                    Album =
                        t.AlbumTracks.FirstOrDefault() != null
                            ? new AlbumDTO
                            {
                                Id = t.AlbumTracks.FirstOrDefault().Album.Id,
                                Title = t.AlbumTracks.FirstOrDefault().Album.Title,
                                CoverArtUrl = t.AlbumTracks.FirstOrDefault().Album.CoverArtUrl,
                                PercentageDone = t
                                    .AlbumTracks.FirstOrDefault()
                                    .Album.PercentageDone,
                                IsComplete = t.AlbumTracks.FirstOrDefault().Album.IsComplete,
                                TrackOrder = t.AlbumTracks.FirstOrDefault().TrackOrder,
                            }
                            : null,
                })
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize]
    public IActionResult CreateTrack(TrackCreateDTO trackDTO)
    {
        try
        {
            //check if user exists
            var user = _dbContext.UserProfiles.Find(trackDTO.CreatorId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            //create the track entity
            var newTrack = new Track
            {
                Title = trackDTO.Title,
                AudioUrl = trackDTO.AudioUrl,
                CoverArtUrl = trackDTO.CoverArtUrl,
                PercentageDone = trackDTO.PercentageDone > 100 ? 100 : trackDTO.PercentageDone,
                Deadline = trackDTO.Deadline,
                UploadDate = DateTime.Now,
                IsComplete = trackDTO.PercentageDone >= 100 ? true : false,
                CreatorId = trackDTO.CreatorId,
            };

            _dbContext.Tracks.Add(newTrack);
            _dbContext.SaveChanges();

            //add instruments if provided
            if (trackDTO.InstrumentIds != null && trackDTO.InstrumentIds.Count > 0)
            {
                foreach (var instrumentId in trackDTO.InstrumentIds)
                {
                    //check if instrument exists
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
                _dbContext.SaveChanges();
            }

            return Created(
                $"/api/Track/{newTrack.Id}",
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
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"An error occurred while processing your request {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public IActionResult Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("TrackId Must Be A Positive Integer");
            }

            Track track = _dbContext.Tracks.SingleOrDefault(t => t.Id == id);

            if (track == null)
            {
                return NotFound("This Track Doesn't Exist");
            }

            _dbContext.Tracks.Remove(track);
            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Error processing your request {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public IActionResult UpdateTrack(int id, TrackUpdateDTO trackDTO)
    {
        try
        {
            Track trackToEdit = _dbContext
                .Tracks.Include(t => t.TrackInstruments)
                .Include(t => t.Creator)
                .SingleOrDefault(t => t.Id == id);

            if (trackToEdit == null)
            {
                return NotFound("That track does not exist");
            }

            // Check if current user is the track creator
            var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Get the UserProfile for the current user
            var currentUserProfile = _dbContext.UserProfiles.FirstOrDefault(up =>
                up.IdentityUserId == userIdClaim
            );

            if (currentUserProfile == null)
            {
                return Unauthorized("User profile not found");
            }

            // Check if current user is the track creator
            if (trackToEdit.CreatorId != currentUserProfile.Id)
            {
                return Forbid("Only the track creator can edit this track");
            }

            trackToEdit.Title = trackDTO.Title;
            trackToEdit.PercentageDone =
                trackDTO.PercentageDone > 100 ? 100 : trackDTO.PercentageDone;
            trackToEdit.Deadline = trackDTO.Deadline;
            trackToEdit.IsComplete = trackDTO.PercentageDone >= 100;

            if (!string.IsNullOrEmpty(trackDTO.AudioUrl))
            {
                trackToEdit.AudioUrl = trackDTO.AudioUrl;
            }

            if (!string.IsNullOrEmpty(trackDTO.CoverArtUrl))
            {
                trackToEdit.CoverArtUrl = trackDTO.CoverArtUrl;
            }

            if (trackDTO.InstrumentIds != null)
            {
                _dbContext.TrackInstruments.RemoveRange(trackToEdit.TrackInstruments);

                foreach (var instrumentId in trackDTO.InstrumentIds)
                {
                    _dbContext.TrackInstruments.Add(
                        new TrackInstrument { TrackId = id, InstrumentId = instrumentId }
                    );
                }
            }

            _dbContext.SaveChanges();
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"There was an error processing your request {ex.Message}");
        }
    }
}
