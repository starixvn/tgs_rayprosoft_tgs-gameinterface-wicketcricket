using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameWagerUnsettledQueryService : IQueryService<GameWagerUnsettledQuery, GameWagerReslut>
{
    private readonly IGameApiClient _client;

    public GameWagerUnsettledQueryService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async IAsyncEnumerable<GameWagerReslut> QueryAsync(GameWagerUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var resps = await _client
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: query.ApiUrl, website: query.Website, cert: query.Cert, lastUpdateTime: query.LastUpdateTime, reportType: query.ReportType, pageNumber: query.PageNumber, cancellationToken: cancellationToken)
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
                ExternalRoundId: resp.EventId.ToString(),
                ExternalWagerId: $"{resp.CategoryType}_{resp.BetId}",
                ExternalGameId: resp.EventType.ToString(),
                IsClosed: false,
                IsCancelled: false,
                IsVoided: false,
                ExternalCreatedOn: resp.BetPlaced,
                ExternalClosedOn: DateTimeOffset.UnixEpoch,
                Bet: resp.MatchAmount,
                Settle: 0.0m,
                WinLoss: 0.0m,
                AdditionalData: resp.AdditionalData);
        }
    }
}
