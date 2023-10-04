using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerLogoutCommandService : ICommandService<PlayerLogoutCommand>
{
    private readonly IGameApiClient _client;

    public PlayerLogoutCommandService(
        IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => _client.LogoutAsync(
            exchangeApiUrl: command.ExchangeApiUrl,
            website: command.Website,
            cert: command.Cert,
            userId: command.PlayerCode,
            cancellationToken: cancellationToken);
}
