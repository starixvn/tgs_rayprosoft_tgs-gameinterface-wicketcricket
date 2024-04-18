using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class LogoutAllUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://test81.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var agent = "testpginrma";

		var url = new Uri(apiUrl, $"/apiWallet/player/{website}/logoutAll");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["agentId"] = agent
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["status"] = "1"
		};
		var handler = new MockHttpMessageHandler();
		_ = handler
			.Expect(HttpMethod.Post, url.ToString())
			.WithContent(await content.ReadAsStringAsync())
			.Respond(HttpStatusCode.OK, new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

		var fakeLog = NullLogger<GameApiClient>.Instance;
		var fakeOptions = Options.Create(new PolicySettingsOptions
		{
			RetryCount = 1,
			RetryWaitSec = 1
		});
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeHttpClient);

		await client
			.LogoutAllAsync(
			exchangeApiUrl: apiUrl,
			website: website,
			cert: cert,
			agent: agent);
	}

	[Fact]
	public async Task Query_Too_Fast()
	{
		var apiUrl = new Uri("https://test81.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var agent = "testpginrma";

		var url = new Uri(apiUrl, $"/apiWallet/player/{website}/logoutAll");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["agentId"] = agent
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
			RetryCount = 1,
			RetryWaitSec = 1
		});
		var fakeHttpClient = handler.ToHttpClient();

		var client = new GameApiClient(
			fakeLog,
			fakeOptions,
			fakeHttpClient);

		_ = await Assert.ThrowsAsync<MockHttpMatchException>(
			() => client.LogoutAllAsync(
				exchangeApiUrl: apiUrl,
				website: website,
				cert: cert,
				agent: agent)
			.AsTask());
	}
}
