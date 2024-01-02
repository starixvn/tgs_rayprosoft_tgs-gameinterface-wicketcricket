using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerCreateCommandService(IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
    public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
        => _ = await client.GetKeyAsync(
            apiUrl: command.ApiUrl,
            website: command.Website,
            cert: command.Cert,
            userId: command.PlayerCode,
            userName: command.PlayerCode,
            agent: command.Agent,
            currency: command.Currency,
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
