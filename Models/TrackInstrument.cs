using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Demos.Models;

public class TrackInstrument
{
    public int Id { get; set; }

    [Required]
    public int TrackId { get; set; }

    [ForeignKey("TrackId")]
    public Track Track { get; set; }

    [Required]
    public int InstrumentId { get; set; }

    [ForeignKey("InstrumentId")]
    public Instrument Instrument { get; set; }
}
