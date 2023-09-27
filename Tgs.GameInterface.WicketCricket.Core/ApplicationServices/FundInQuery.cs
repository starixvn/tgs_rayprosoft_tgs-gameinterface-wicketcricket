namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record FundInQuery(
    Uri ApiUrl,
    string Website,
    string Cert,
    string PlayerCode,
    string Txid,
    decimal Amount);
