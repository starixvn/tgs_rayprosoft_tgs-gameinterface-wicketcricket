using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class FundInGetService(
	IGameDomain domain,
	IGameApiClient client) : IGetService<FundInQuery, FundResult>
{
	public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var resp = await client
			.WithdrawAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			userId: query.PlayerCode,
			tsCode: query.OrderId,
			amount: query.Amount,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new FundResult(resp.Resp);
	}
}
