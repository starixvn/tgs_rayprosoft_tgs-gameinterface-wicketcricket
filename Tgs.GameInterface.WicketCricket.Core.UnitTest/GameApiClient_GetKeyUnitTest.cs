using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class GameApiClient_GetKeyUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://apiinfo.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var userId = "4ue0ujotpmuu";
		var userName = "4ue0ujotpmuu";
		var agent = "testpginrma";
		var currency = 2;

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getKey");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userId"] = userId,
			["userName"] = userId,
			["agent"] = agent,
			["currency"] = currency.ToString()
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["status"] = "1",
			["key"] = "nH9drDC2V74+mqa+sbtf28QRcUVu84rFvnKZCzbHsgIcVZRVNl37PngNX41AGspU"
		};
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, url.ToString())
			.WithContent(await content.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			GlobalRetryCount = 1,
			GlobalRetryWaitSec = 1
		});
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeHttpClient);

		var actual = await client
			.GetKeyAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			userId: userId,
			userName: userName,
			agent: agent,
			currency: currency);

		Assert.Equal(response["key"]!.GetValue<string>(), actual);
	}

	[Fact]
	public async Task Query_Too_Fast()
	{
		var apiUrl = new Uri("https://test81.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var userId = "4ue0ujotpmuu";
		var userName = "4ue0ujotpmuu";
		var agent = "testpginrma";
		var currency = 2;

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getKey");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userId"] = userId,
			["userName"] = userId,
			["agent"] = agent,
			["currency"] = currency.ToString()
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["status"] = "1010"
		};
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, url.ToString())
			.WithContent(await content.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			GlobalRetryCount = 1,
			GlobalRetryWaitSec = 1
		});
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<MockHttpMatchException>(
			() => client.GetKeyAsync(
				apiUrl: apiUrl,
				website: website,
				cert: cert,
				userId: userId,
				userName: userName,
				agent: agent,
				currency: currency)
			.AsTask());
	}
}
