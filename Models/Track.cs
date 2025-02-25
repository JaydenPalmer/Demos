using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class Track
{
    public int Id { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public UserProfile Creator { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string AudioUrl { get; set; }
    public int? PercentageDone { get; set; }

    [Required]
    public DateTime UploadDate { get; set; }
    public DateTime? Deadline { get; set; }

    [Required]
    public bool IsComplete { get; set; }
    public string CoverArtUrl { get; set; }

    public List<AlbumTrack> AlbumTracks { get; set; } = new();
    public List<TrackInstrument> TrackInstruments { get; set; } = new();
    public List<Note> Notes { get; set; } = new();
}
