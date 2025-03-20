namespace Demos.Models.DTOs;

public class AlbumWithTracksDTO
{
    public AlbumCreateDTO Album { get; set; }
    public List<TrackCreateDTO> Tracks { get; set; } = new List<TrackCreateDTO>();
}
