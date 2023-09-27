using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record GameWagerUnsettledQuery(
    Uri ApiUrl,
    string Website,
    string Cert,
    DateTimeOffset LastUpdateTime,
    ReportTypes ReportType,
    int PageNumber);
