using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RichardSzalay.MockHttp;

using Tgs.GameInterface.WicketCricket.Core.Exceptions;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class GameApiClient_QueryDomainUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var apiUrl = new Uri("https://apiinfo.cckk77.net/");
		var website = "WICKETSPRO";
		var cert = "wkVwQ8sMVr252EA9";

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryDomain");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert
		};
		var content = new FormUrlEncodedContent(parameters);
		var response = new JsonObject
		{
			["status"] = "1",
			["privateDomains"] = new JsonArray
			{
				new JsonObject
				{
					["domain"]="cckk77.net",
					["priority"]=1
				},
				new JsonObject
				{
					["domain"]="cckk77.live",
					["priority"]=2
				}
			},
			["domains"] = new JsonArray
			{
				new JsonObject
				{
					["domain"]="ppkk77.net",
					["priority"]=1
				},
				new JsonObject
				{
					["domain"]="ppkk77.live",
					["priority"]=2
				}
			}
		};
		var handler = new MockHttpMessageHandler();
		_ = handler.Expect(HttpMethod.Post, url.ToString())
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

		var actual = await client.QueryDomainAsync(
			apiUrl: apiUrl,
			website: website,
			cert: cert);

		Assert.NotNull(actual);
		Assert.Equal("cckk77.net", actual.PrivateDomains.First().Domain);
		Assert.Equal("ppkk77.net", actual.Domains.First().Domain);
	}
}
