namespace Demos.Models.DTOs;

public class AlbumDetailDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public string CoverArtUrl { get; set; }
    public int? PercentageDone { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsComplete { get; set; }

    public UserProfileDTO Creator { get; set; }
    public List<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
}
