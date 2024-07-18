namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerBalanceQuery
{
	/// <summary>
	/// The API URL
	/// </summary>
	/// <example>https://apiinfo.cckk77.net/</example>
	public Uri ApiUrl { get; init; } = default!;

	/// <summary>
	/// The website
	/// </summary>
	/// <example>WICKETSPRO</example>
	public string Website { get; init; } = default!;

	/// <summary>
	/// The cert
	/// </summary>
	/// <example>wkVwQ8sMVr252EA9</example>
	public string Cert { get; init; } = default!;

	/// <summary>
	/// The player code
	/// </summary>
	/// <example>4ue0ujotpmuu</example>
	public string PlayerCode { get; init; } = default!;
}
