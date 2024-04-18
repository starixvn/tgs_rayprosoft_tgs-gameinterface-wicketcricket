using System.ComponentModel.DataAnnotations;

namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record FundViewModel
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
	[Required]
	public decimal Amount { get; init; } = default!;
}
