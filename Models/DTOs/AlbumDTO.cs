namespace Demos.Models.DTOs;

public class AlbumDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CoverArtUrl { get; set; }
    public int? PercentageDone { get; set; }
    public bool IsComplete { get; set; }
    public int? TrackOrder { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public UserProfileDTO Creator { get; set; }
    public List<TrackDTO> Tracks { get; set; } = new List<TrackDTO>();
}
