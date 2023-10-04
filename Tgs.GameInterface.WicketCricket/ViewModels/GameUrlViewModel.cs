using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record GameUrlViewModel
{
    [Required]
    [DataType(DataType.Url)]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    [DataType(DataType.Url)]
    public Uri ExchangeApiUrl { get; init; } = default!;
    [Required]
    public string Website { get; init; } = default!;
    [Required]
    public string Cert { get; init; } = default!;
    [Required]
    public string Agent { get; init; } = default!;
    [Required]
    public string PlayerCode { get; init; } = default!;
    [Required]
    public int Currency { get; init; } = default!;
}
