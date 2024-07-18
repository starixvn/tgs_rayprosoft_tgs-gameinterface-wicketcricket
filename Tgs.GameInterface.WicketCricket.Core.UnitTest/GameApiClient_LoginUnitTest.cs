using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class GameApiClient_LoginUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://test81.cckk77.net/");
		var website = "WICKETSPRO";
		var userId = "4ue0ujotpmuu";
		var key = "nH9drDC2V74+mqa+sbtf28WmiHIwnkXv09F4IYvhYQAWdNKqiLHupZg8k+BwEtJy";
		var eventType = 1;

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			GlobalRetryCount = 1,
			GlobalRetryWaitSec = 1
		});
		var fakeHttpClient = new MockHttpMessageHandler().ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeHttpClient);

		var actual = await client
			.LoginAsync(
			exchangeApiUrl: apiUrl,
			website: website,
			userId: userId,
			key: key,
			eventType: eventType);

		Assert.Equal("https://test81.cckk77.net:443/apiWallet/player/WICKETSPRO/login?userId=4ue0ujotpmuu&key=nH9drDC2V74%2bmqa%2bsbtf28WmiHIwnkXv09F4IYvhYQAWdNKqiLHupZg8k%2bBwEtJy&eventType=1", actual);
	}
}
