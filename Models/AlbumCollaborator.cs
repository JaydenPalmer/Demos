using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class AlbumCollaborator
{
    public int Id { get; set; }

    [Required]
    public int AlbumId { get; set; }

    [ForeignKey("AlbumId")]
    public Album Album { get; set; }

    [Required]
    public int UserProfileId { get; set; }

    [ForeignKey("UserProfileId")]
    public UserProfile UserProfile { get; set; }

    public bool CanAddTracks { get; set; } = true;

    public bool CanAddNotes { get; set; } = true;
    public DateTime AddedDate { get; set; } = DateTime.Now;
}
