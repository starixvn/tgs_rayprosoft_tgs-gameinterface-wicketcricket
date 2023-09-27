using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record FundStatusViewModel
{
    [Required]
    public Uri ApiUrl { get; init; } = default!;
    [Required]
    public string Website { get; init; } = default!;
    [Required]
    public string Cert { get; init; } = default!;
    [Required]
    public string PlayerCode { get; init; } = default!;
    [Required]
    public string Txid { get; init; } = default!;
}
