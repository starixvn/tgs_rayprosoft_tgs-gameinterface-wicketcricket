using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class PlayerBalanceGetServiceUnitTest
{
	[Fact]
	public async Task PlayerBalance_Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new PlayerBalanceQuery
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			PlayerCode = "4ue0ujotpmuu"
		};

		var apiUrl = new Uri("https://apiinfo.ppkk77.net/");
		_ = fakeGameDomain.GetApiUrlDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(apiUrl);

		var result = new BalanceResp(
			IsPlayerExist: true,
			Balance: 8060.00m,
			Exposure: 1940.00m);
		_ = fakeGameApiClient.GetBalanceAsync(
			apiUrl: Arg.Is(apiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			userId: Arg.Is(query.PlayerCode),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var service = new PlayerBalanceGetService(
			fakeGameDomain,
			fakeGameApiClient);

		var actual = await service
			.GetAsync(
			query);

		Assert.NotNull(actual);
		Assert.Equal(10000m, actual.Balance);
		Assert.Equal(8060.00m, actual.FreeBalance);
		Assert.Equal(1940.00m, actual.OutstandingBalance);
	}
}
