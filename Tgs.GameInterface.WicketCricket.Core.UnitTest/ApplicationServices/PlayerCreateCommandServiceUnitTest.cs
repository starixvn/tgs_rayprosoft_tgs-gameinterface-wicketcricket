using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
	[Fact]
	public async Task PlayerCreate_Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var command = new PlayerCreateCommand
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			Agent = "testpginrma",
			PlayerCode = "4ue0ujotpmuu",
			Currency = 2
		};

		var apiUrl = new Uri("https://apiinfo.ppkk77.net/");
		_ = fakeGameDomain.GetApiUrlDomainAsync(
			apiUrl: Arg.Is(command.ApiUrl),
			website: Arg.Is(command.Website),
			cert: Arg.Is(command.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(apiUrl);

		var service = new PlayerCreateCommandService(
			fakeGameDomain,
			fakeGameApiClient);

		await service
			.ExecuteAsync(
			 command);

		_ = await fakeGameApiClient
			.Received(1)
			.GetKeyAsync(
			apiUrl: Arg.Is(apiUrl),
			website: Arg.Is(command.Website),
			cert: Arg.Is(command.Cert),
			userId: Arg.Is(command.PlayerCode),
			userName: Arg.Is(command.PlayerCode),
			agent: Arg.Is(command.Agent),
			currency: Arg.Is(command.Currency),
			cancellationToken: Arg.Any<CancellationToken>());
	}
}
