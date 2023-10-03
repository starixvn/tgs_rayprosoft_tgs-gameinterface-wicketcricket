using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerCreateCommandService : ICommandService<PlayerCreateCommand>
{
    private readonly IGameApiClient _client;

    public PlayerCreateCommandService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _ = await _client
        .GetKeyAsync(
            exchangeApiUrl: command.ExchangeApiUrl,
            website: command.Website, 
            cert: command.Cert,
            userId: command.PlayerCode,
            userName: command.PlayerCode,
            agent: command.Agent,
            currency: command.Currency,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
