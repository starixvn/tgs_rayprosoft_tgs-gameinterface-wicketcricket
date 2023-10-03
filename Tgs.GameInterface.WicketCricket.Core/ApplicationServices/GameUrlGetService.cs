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
            query.ExchangeApiUrl,
            query.Website,
            query.Cert,
            query.PlayerCode,
            query.PlayerCode,
            query.Agent,
            query.Currency,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        var gameUrl = await _client
            .LoginAsync(
            query.ExchangeApiUrl,
            query.Website,
            query.PlayerCode,
            key: key,
            eventType: 1,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReslut(gameUrl);
    }
}
