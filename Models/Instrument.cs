using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class Instrument
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public int CategoryId { get; set; }

    [ForeignKey("CategoryId")]
    public InstrumentCategory Category { get; set; }
    public string? Description { get; set; }

    public List<TrackInstrument> TrackInstruments { get; set; } = new();
}
