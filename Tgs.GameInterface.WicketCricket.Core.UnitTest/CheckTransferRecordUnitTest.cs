using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class CheckTransferRecordUnitTest
{
	[Fact]
	public async Task Query_Too_Fast()
	{
		var apiUrl = new Uri("https://apiinfo.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var userId = "4ue0ujotpmuu";
		var tsCode = "1695627094";

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/checkTransferRecord");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["tsCodes"] = new JsonArray{
				new JsonObject
				{
					["userId"] = userId,
					["tsCode"] = tsCode,
				}
			}.ToJsonString()
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
			() => client.CheckTransferRecordAsync(
				apiUrl: apiUrl,
				website: website,
				cert: cert,
				userId: userId,
				tsCode: tsCode)
			.AsTask());
	}

	[Fact]
	public async Task The_Order_Is_Established()
	{
		var apiUrl = new Uri("https://apiinfo.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var userId = "4ue0ujotpmuu";
		var tsCode = "1695627094";

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/checkTransferRecord");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["tsCodes"] = new JsonArray{
				new JsonObject
				{
					["userId"] = userId,
					["tsCode"] = tsCode,
				}
			}.ToJsonString()
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["result"] = new JsonObject
			{
				["1695627094"] = new JsonObject
				{
					["balance"] = 1000.0,
					["createdate"] = "09/25/2023 15:32:23",
					["tsCode"] = "1695627094",
					["userId"] = "4ue0ujotpmuu",
					["status"] = "1012"
				}
			},
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

		var actual = await client
			.CheckTransferRecordAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			userId: userId,
			tsCode: tsCode);

		Assert.True(actual);
	}

	[Fact]
	public async Task The_Order_Was_Not_Established()
	{
		var apiUrl = new Uri("https://apiinfo.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";
		var userId = "4ue0ujotpmuu";
		var tsCode = "1695627094";

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/checkTransferRecord");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["tsCodes"] = new JsonArray{
				new JsonObject
				{
					["userId"] = userId,
					["tsCode"] = tsCode,
				}
			}.ToJsonString()
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["result"] = new JsonObject
			{
				["1695627094"] = new JsonObject
				{
					["status"] = "1025"
				}
			},
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

		var actual = await client
			.CheckTransferRecordAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert,
			userId: userId,
			tsCode: tsCode);

		Assert.False(actual);
	}
}
