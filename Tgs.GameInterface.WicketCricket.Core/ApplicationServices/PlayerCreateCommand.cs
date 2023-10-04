namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerCreateCommand(
    Uri ApiUrl,
    string Website,
    string Cert,
    string Agent,
    string PlayerCode,
    int Currency);
