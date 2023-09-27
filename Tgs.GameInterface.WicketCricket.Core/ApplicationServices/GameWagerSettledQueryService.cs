using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameWagerSettledQueryService : IQueryService<GameWagerSettledQuery, GameWagerReslut>
{
    private readonly IGameApiClient _client;

    public GameWagerSettledQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameWagerReslut> QueryAsync(GameWagerSettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var resps = await _client
            .QuerySettleBetHistoryAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            startTime: query.StartTime,
            endTime: query.EndTime,
            reportType: query.ReportType,
            pageNumber: query.PageNumber,
            cancellationToken: cancellationToken)
            .ToArrayAsync(
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!resps.Any())
        {
            yield break;
        }

        foreach (var resp in resps)
        {
            yield return new GameWagerReslut(
                PlayerCode: resp.UserId,
                ExternalWagerId: $"{resp.CategoryType}_{resp.BetId}",
                ExternalGameId: resp.EventType.ToString(),
                IsClosed: true,
                IsCancelled: resp.BetStatus.Equals("Cancelled", StringComparison.OrdinalIgnoreCase),
                IsVoided: resp.BetStatus.Equals("Voided", StringComparison.OrdinalIgnoreCase),
                ExternalCreatedOn: resp.BetPlaced,
                ExternalClosedOn: resp.MarketSettledDate,
                Bet: resp.MatchStake,
                Settle: resp.BetStatus.Equals("Settled", StringComparison.OrdinalIgnoreCase) ? resp.MatchStake + resp.ProfitLoss : 0.0m,
                WinLoss: resp.ProfitLoss,
                AdditionalData: resp.AdditionalData);
        }
    }
}
