using System.ComponentModel.DataAnnotations;
using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record GameWagerSettledViewModel
{
    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string Website { get; init; } = default!;
    [Required]
    public string Cert { get; init; } = default!;
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset StartTime { get; init; }
    [Required]
    [DataType(DataType.DateTime)]
    public DateTimeOffset EndTime { get; init; }
    [Required]
    public ReportTypes ReportType { get; init; }
    [Required]
    public int PageNumber { get; init; }
}
