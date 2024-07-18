using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameRecordUnsettledQueryService(
	IGameDomain domain,
	IGameApiClient client) : IQueryService<GameRecordUnsettledQuery, GameRecordReslut>
{
	public async IAsyncEnumerable<GameRecordReslut> QueryAsync(GameRecordUnsettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var resps = client.QueryCurrentBetsByLastUpdateDateAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			lastUpdateTime: query.LastUpdateTime,
			reportType: query.ReportType,
			pageNumber: query.PageNumber,
			cancellationToken: cancellationToken);

		await foreach (var resp in resps)
		{
			yield return new GameRecordReslut(
				PlayerCode: resp.UserId,
				ExternalRoundId: resp.EventId.ToString(),
				ExternalBetId: $"{resp.CategoryType}_{resp.BetId}",
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
