using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class Note
{
    public int Id { get; set; }

    [Required]
    public int CreatorId { get; set; }

    [ForeignKey("CreatorId")]
    public UserProfile Creator { get; set; }
    public int? TrackId { get; set; }

    [ForeignKey("TrackId")]
    public Track Track { get; set; }
    public int? AlbumId { get; set; }

    [ForeignKey("AlbumId")]
    public Album Album { get; set; }

    [Required]
    public string Content { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
