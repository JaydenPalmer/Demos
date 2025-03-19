namespace Demos.Models.DTOs;

public class TrackDTO
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string AudioUrl { get; set; }
    public int? PercentageDone { get; set; }
    public DateTime UploadDate { get; set; }
    public DateTime? Deadline { get; set; }
    public bool IsComplete { get; set; }
    public string CoverArtUrl { get; set; }
    public int? TrackOrder { get; set; }

    public UserProfileDTO Creator { get; set; }
    public AlbumDTO Album { get; set; }
    public List<InstrumentDTO> Instruments { get; set; } = new List<InstrumentDTO>();
}
