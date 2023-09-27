namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record PlayerBalanceResultViewModel(
    bool IsPlayerExist,
    decimal Balance,
    decimal FreeBalance,
    decimal OutstandingBalance);
