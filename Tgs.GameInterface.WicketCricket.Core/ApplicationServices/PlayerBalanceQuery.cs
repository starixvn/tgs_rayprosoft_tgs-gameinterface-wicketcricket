namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerBalanceQuery(
	Uri ApiUrl,
	string Website,
	string Cert,
	string PlayerCode);
