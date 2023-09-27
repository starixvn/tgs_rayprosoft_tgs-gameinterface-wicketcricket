namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ExchangeApiUrl,
    string Website,
    string Cert,
    string Agent,
    string PlayerCode,
    int Currency);
