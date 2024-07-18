using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record GameRecordUnsettledQuery
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
	/// The last update time
	/// </summary>
	/// <example>2023-09-26T00:00:00+08:00</example>
	public DateTimeOffset LastUpdateTime { get; init; } = default!;

	/// <summary>
	/// The report type
	/// </summary>
	/// <example>FANCY_BET</example>
	public ReportTypes ReportType { get; init; }

	/// <summary>
	/// The page number
	/// </summary>
	/// <example>1</example>
	public int PageNumber { get; init; }
}
