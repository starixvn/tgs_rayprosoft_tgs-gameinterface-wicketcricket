using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class GameUrlGetService : IGetService<GameUrlQuery, GameUrlReslut>
{
    private readonly IGameApiClient _client;

    public GameUrlGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<GameUrlReslut> GetAsync(GameUrlQuery query, CancellationToken cancellationToken = default)
    {
        var key = await _client
            .GetKeyAsync(
            exchangeApiUrl: query.ExchangeApiUrl, website: query.Website, cert: query.Cert, userId: query.PlayerCode, userName: query.PlayerCode, agent: query.Agent, currency: query.Currency, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameUrl = await _client
            .LoginAsync(
            exchangeApiUrl: query.ExchangeApiUrl, website: query.Website, userId: query.PlayerCode, key: key, eventType: 1, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReslut(gameUrl);
    }
}
