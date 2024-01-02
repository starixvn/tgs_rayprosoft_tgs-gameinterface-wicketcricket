namespace Tgs.GameInterface.WicketCricket.Core.Exceptions;

public class DepositWithdrawalAmountException(decimal amount) : Exception($"Deposit and withdrawal amount format with two decimal places.{amount}(存取款金額格式小數點兩位)")
{
}
