using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerCreateCommandService(
	IGameDomain domain,
	IGameApiClient client) : ICommandService<PlayerCreateCommand>
{
	public async ValueTask ExecuteAsync(PlayerCreateCommand command, CancellationToken cancellationToken = default)
	{
		var apiUrl = await domain.GetApiUrlDomainAsync(
			apiUrl: command.ApiUrl,
			website: command.Website,
			cert: command.Cert,
			priority: 1,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		_ = await client.GetKeyAsync(
			apiUrl: apiUrl,
			website: command.Website,
			cert: command.Cert,
			userId: command.PlayerCode,
			userName: command.PlayerCode,
			agent: command.Agent,
			currency: command.Currency,
			cancellationToken: cancellationToken)
		.ConfigureAwait(false);
	}
}
