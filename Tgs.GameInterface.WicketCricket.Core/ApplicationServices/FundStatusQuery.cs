namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record FundStatusQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string PlayerCode,
	string Txid);
