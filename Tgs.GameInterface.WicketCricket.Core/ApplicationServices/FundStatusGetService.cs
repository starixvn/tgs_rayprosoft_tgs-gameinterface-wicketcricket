using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class FundStatusGetService(
	IGameDomain domain,
	IGameApiClient client) : IGetService<FundStatusQuery, bool>
{
	public async ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return await client.CheckTransferRecordAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			userId: query.PlayerCode,
			tsCode: query.OrderId,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);
	}
}
