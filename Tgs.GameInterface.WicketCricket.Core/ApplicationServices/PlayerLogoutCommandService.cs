using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
    IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
    public ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
        => client.LogoutAsync(
            exchangeApiUrl: command.ExchangeApiUrl,
            website: command.Website,
            cert: command.Cert,
            userId: command.PlayerCode,
            cancellationToken: cancellationToken);
}
