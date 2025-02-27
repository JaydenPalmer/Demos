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
    public IActionResult GetAll([FromQuery] int? userId)
    {
        try
        {
            //base query getting all of everything
            IQueryable<Track> query = _dbContext
                .Tracks.Include(t => t.Creator)
                .Include(t => t.AlbumTracks)
                .ThenInclude(at => at.Album)
                .ThenInclude(a => a.Collaborators)
                .ThenInclude(c => c.UserProfile);

            if (userId.HasValue)
            {
                //check to see if user exsists
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
}
