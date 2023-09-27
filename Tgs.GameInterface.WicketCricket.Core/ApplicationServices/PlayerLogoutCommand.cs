namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerLogoutCommand(
    Uri ExchangeApiUrl,
    string Website,
    string Cert,
    string PlayerCode);
