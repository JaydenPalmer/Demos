namespace Demos.Models.DTOs;

public class AlbumCreateDTO
{
    public string Title { get; set; }
    public string CoverArtUrl { get; set; }
    public bool IsComplete { get; set; }
    public int CreatorId { get; set; }
    public string Description { get; set; }
}
