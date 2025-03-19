namespace Demos.Models.DTOs;

public class TrackUpdateDTO
{
    public int Id { get; set; }

    public string Title { get; set; }

    public int PercentageDone { get; set; }

    public DateTime? Deadline { get; set; }

    public string AudioUrl { get; set; }

    public string CoverArtUrl { get; set; }

    public List<int> InstrumentIds { get; set; } = new List<int>();
}
