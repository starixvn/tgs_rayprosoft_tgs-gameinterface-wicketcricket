using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
	[Fact]
	public async Task PlayerLogout_Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerLogoutCommand
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			ExchangeApiUrl = new Uri("https://test81.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			PlayerCode = "4ue0ujotpmuu"
		};

		var exchangeApiUrl = new Uri("https://test81.ppkk77.net/");
		_ = fakeGameDomain.GetExchangeApiUrlDomainAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			exchangeApiUrl: Arg.Is(command.ExchangeApiUrl),
			website: Arg.Is(command.Website),
			cert: Arg.Is(command.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(exchangeApiUrl);

		var service = new PlayerLogoutCommandService(
			fakeGameDomain,
			fakeGameApiClient);

		await service
			.ExecuteAsync(
			 command);

		_ = fakeGameApiClient
			.Received(1)
			.LogoutAsync(
			exchangeApiUrl: Arg.Is(exchangeApiUrl),
			website: Arg.Is(command.Website),
			cert: Arg.Is(command.Cert),
			userId: Arg.Is(command.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
