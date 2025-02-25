using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class UserProfile
{
    public int Id { get; set; }

    [Required]
    public string FirstName { get; set; }

    [Required]
    public string LastName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public string UserName { get; set; }
    public bool IsArtist { get; set; }
    public DateTime JoinDate { get; set; }

    [Required]
    public string ProfileImage { get; set; }

    public string IdentityUserId { get; set; }

    public IdentityUser IdentityUser { get; set; }
    public List<Track> Tracks { get; set; } = new();
    public List<Album> Albums { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
    public List<AlbumCollaborator> AlbumCollaborations { get; set; } = new();
}
