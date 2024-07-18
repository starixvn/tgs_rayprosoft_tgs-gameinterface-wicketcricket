using System.Runtime.CompilerServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameRecordSettledQueryService(
	IGameDomain domain,
	IGameApiClient client) : IQueryService<GameRecordSettledQuery, GameRecordReslut>
{
	public async IAsyncEnumerable<GameRecordReslut> QueryAsync(GameRecordSettledQuery query, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var resps = client.QuerySettleBetHistoryAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			startTime: query.StartTime,
			endTime: query.EndTime,
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
