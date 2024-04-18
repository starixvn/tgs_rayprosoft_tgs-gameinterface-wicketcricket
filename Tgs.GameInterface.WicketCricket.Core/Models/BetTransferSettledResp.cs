namespace Tgs.GameInterface.WicketCricket.Core.Models;

public record BetTransferSettledResp(
	long EventId,
	int EventType,
	int CurrencyType,
	int CategoryType,
	long BetId,
	string UserId,
	string AgentId,
	DateTimeOffset BetPlaced,
	DateTimeOffset MatchSettledDate,
	DateTimeOffset MarketSettledDate,
	string BetStatus,
	decimal MatchStake,
	decimal ProfitLoss,
	string AdditionalData);
