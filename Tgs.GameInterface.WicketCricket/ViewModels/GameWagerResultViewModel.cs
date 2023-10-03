namespace Tgs.GameInterface.WicketCricket.ViewModels;

public record GameWagerResultViewModel(
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
