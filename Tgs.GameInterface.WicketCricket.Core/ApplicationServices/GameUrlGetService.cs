using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameUrlGetService(
	IGameDomain domain,
	IGameApiClient client) : IGetService<GameUrlQuery, GameUrlReslut>
{
	public async ValueTask<GameUrlReslut> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var key = await client
			.GetKeyAsync(
			apiUrl: apiUrl,
			website: query.Website,
			cert: query.Cert,
			userId: query.PlayerCode,
			userName: query.PlayerCode,
			agent: query.Agent,
			currency: query.Currency,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var exchangeApiUrl = await domain.GetExchangeApiUrlDomainAsync(
			apiUrl: query.ApiUrl,
			exchangeApiUrl: query.ExchangeApiUrl,
			website: query.Website,
			cert: query.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		var gameUrl = await client
			.LoginAsync(
			exchangeApiUrl: exchangeApiUrl,
			website: query.Website,
			userId: query.PlayerCode,
			key: key,
			eventType: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new GameUrlReslut(gameUrl);
	}
}
