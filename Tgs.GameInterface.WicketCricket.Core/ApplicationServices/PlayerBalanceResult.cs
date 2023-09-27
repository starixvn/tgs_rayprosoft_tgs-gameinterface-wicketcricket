namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record PlayerBalanceResult
{
    public bool IsPlayerExist { get; init; }
    public decimal Balance => FreeBalance + OutstandingBalance;
    public decimal FreeBalance { get; init; }
    public decimal OutstandingBalance { get; init; }
}
