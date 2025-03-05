namespace Demos.Models.DTOs;

public class TrackCreateDTO
{
    public string Title { get; set; }

    public string Description { get; set; }

    public string AudioUrl { get; set; }

    public string CoverArtUrl { get; set; }

    public int PercentageDone { get; set; }

    public DateTime? Deadline { get; set; }

    public int CreatorId { get; set; }

    public List<int> InstrumentIds { get; set; } = new List<int>();
}
