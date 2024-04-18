namespace Tgs.GameInterface.WicketCricket.Core.Models;

public record BetTransferUnsettledResp(
	long EventId,
	int EventType,
	int CurrencyType,
	int CategoryType,
	long BetId,
	string UserId,
	string AgentId,
	DateTimeOffset BetPlaced,
	decimal MatchAmount,
	decimal UnMatchAmount,
	DateTimeOffset DateMatched,
	string AdditionalData);
