using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record GameWagerSettledQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	DateTimeOffset StartTime,
	DateTimeOffset EndTime,
	ReportTypes ReportType,
	int PageNumber);
