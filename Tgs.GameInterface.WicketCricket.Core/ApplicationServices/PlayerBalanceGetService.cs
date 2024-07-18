using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerBalanceGetService(
	IGameDomain domain,
	IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
	public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var resp = await client.GetBalanceAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			userId: query.PlayerCode,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new PlayerBalanceResult
		{
			IsPlayerExist = resp.IsPlayerExist,
			FreeBalance = resp.Balance,
			OutstandingBalance = resp.Exposure
		};
	}
}
