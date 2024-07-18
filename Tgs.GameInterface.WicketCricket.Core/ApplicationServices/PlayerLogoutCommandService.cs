using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerLogoutCommandService(
	IGameDomain domain,
	IGameApiClient client) : ICommandService<PlayerLogoutCommand>
{
	public async ValueTask ExecuteAsync(PlayerLogoutCommand command, CancellationToken cancellationToken = default)
	{
		var exchangeApiUrl = await domain.GetExchangeApiUrlDomainAsync(
			apiUrl: command.ApiUrl,
			exchangeApiUrl: command.ExchangeApiUrl,
			website: command.Website,
			cert: command.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		await client.LogoutAsync(
		   exchangeApiUrl: exchangeApiUrl,
		   website: command.Website,
		   cert: command.Cert,
		   userId: command.PlayerCode,
		   cancellationToken: cancellationToken)
		   .ConfigureAwait(false);
	}
}
