namespace Tgs.GameInterface.WicketCricket.Core.Models;

public record BalanceResp(
    bool IsPlayerExist,
    decimal Balance,
    decimal Exposure);
