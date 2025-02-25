using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class Album
{
    public int Id { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public UserProfile Creator { get; set; }

    [Required]
    public string Title { get; set; }
    public int? PercentageDone { get; set; }
    public string Description { get; set; }

    [Required]
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; }

    [Required]
    public bool IsComplete { get; set; }
    public string CoverArtUrl { get; set; }

    public List<AlbumTrack> AlbumTracks { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
    public List<AlbumCollaborator> Collaborators { get; set; } = new();
}
