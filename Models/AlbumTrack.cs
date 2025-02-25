using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class AlbumTrack
{
    public int Id { get; set; }

    [Required]
    public int AlbumId { get; set; }

    [ForeignKey("AlbumId")]
    public Album Album { get; set; }

    [Required]
    public int TrackId { get; set; }

    [ForeignKey("TrackId")]
    public Track Track { get; set; }

    public int? TrackOrder { get; set; }
}
