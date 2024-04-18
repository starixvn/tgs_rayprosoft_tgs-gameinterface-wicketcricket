namespace Tgs.GameInterface.WicketCricket.Core.Models;

public record BalancesResp(
	string UserId,
	decimal Balance,
	decimal Exposure);
