using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class GameUrlGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new GameUrlQuery
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			ExchangeApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			Agent = "testpginrma",
			PlayerCode = "4ue0ujotpmuu",
			Currency = 2
		};

		var apiUrl = new Uri("https://apiinfo.ppkk77.net/");
		_ = fakeGameDomain.GetApiUrlDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(apiUrl);

		var key = "nH9drDC2V74+mqa+sbtf28QRcUVu84rFvnKZCzbHsgIcVZRVNl37PngNX41AGspU";
		_ = fakeGameApiClient.GetKeyAsync(
		apiUrl: Arg.Is(apiUrl),
		website: Arg.Is(query.Website),
		cert: Arg.Is(query.Cert),
		userId: Arg.Is(query.PlayerCode),
		userName: Arg.Is(query.PlayerCode),
		agent: Arg.Is(query.Agent),
		currency: Arg.Is(query.Currency),
		cancellationToken: Arg.Any<CancellationToken>())
		.Returns(key);

		var exchangeApiUrl = new Uri("https://test81.ppkk77.net/");
		_ = fakeGameDomain.GetExchangeApiUrlDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			exchangeApiUrl: Arg.Is(query.ExchangeApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(exchangeApiUrl);

		var gameUrl = "https://test81.cckk77.net:443/apiWallet/player/WICKETSPRO/login?userId=4ue0ujotpmuu&key=nH9drDC2V74%2bmqa%2bsbtf28WmiHIwnkXv09F4IYvhYQAWdNKqiLHupZg8k%2bBwEtJy&eventType=1";
		_ = fakeGameApiClient.LoginAsync(
			exchangeApiUrl: Arg.Is(exchangeApiUrl),
			website: Arg.Is(query.Website),
			userId: Arg.Is(query.PlayerCode),
			key: Arg.Is(key),
			eventType: Arg.Is(1),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(gameUrl);

		var service = new GameUrlGetService(
			fakeGameDomain,
			fakeGameApiClient);

		var actual = await service
			.GetAsync(
			query);

		Assert.NotNull(actual);
		Assert.NotEmpty(actual.GameUrl);
	}
}
