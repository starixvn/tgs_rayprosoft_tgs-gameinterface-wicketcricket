namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

public record GameWagerReslut(
    string PlayerCode,
    string ExternalRoundId,
    string ExternalWagerId,
    string ExternalGameId,
    bool IsClosed,
    bool IsCancelled,
    bool IsVoided,
    DateTimeOffset ExternalCreatedOn,
    DateTimeOffset ExternalClosedOn,
    decimal Bet,
    decimal Settle,
    decimal WinLoss,
    string AdditionalData);
