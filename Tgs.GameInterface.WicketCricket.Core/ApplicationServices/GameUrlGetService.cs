using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameUrlGetService(IGameApiClient client) : IGetService<GameUrlQuery, GameUrlReslut>
{
    public async ValueTask<GameUrlReslut> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var key = await client
            .GetKeyAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            userId: query.PlayerCode,
            userName: query.PlayerCode,
            agent: query.Agent,
            currency: query.Currency,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameUrl = await client
            .LoginAsync(
            exchangeApiUrl: query.ExchangeApiUrl,
            website: query.Website,
            userId: query.PlayerCode,
            key: key,
            eventType: 1,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReslut(gameUrl);
    }
}
