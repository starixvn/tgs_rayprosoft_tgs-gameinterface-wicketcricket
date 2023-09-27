namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record GameUrlQuery(
    Uri ExchangeApiUrl,
    string Website,
    string Cert,
    string Agent,
    string PlayerCode,
    int Currency);
