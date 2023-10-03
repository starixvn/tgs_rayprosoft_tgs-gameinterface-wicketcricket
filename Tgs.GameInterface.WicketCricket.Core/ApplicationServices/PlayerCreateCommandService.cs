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
        command.ExchangeApiUrl,
        command.Website,
        command.Cert,
        command.PlayerCode,
        command.PlayerCode,
        command.Agent,
        command.Currency,
        cancellationToken)
        .ConfigureAwait(false);
}
