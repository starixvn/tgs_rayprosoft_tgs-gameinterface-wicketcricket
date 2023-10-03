using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RichardSzalay.MockHttp;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class GetBalancesUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getBalance");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["alluser"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["count"] = 1,
            ["querytime"] = "09/25/2023 14:36:02",
            ["results"] = new JsonArray
            {
                new JsonObject
                {
                    ["userId"] = "4ue0ujotpmuu",
                    ["balance"] = 0.0m,
                    ["exposure"] = 0.00m
                }
            }
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
            .GetBalancesAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal(0.0m, actual.ElementAt(0).Balance);
        Assert.Equal(0.0m, actual.ElementAt(0).Exposure);
    }

    [Fact]
    public async Task Outstanding_Balance()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getBalance");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["alluser"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["count"] = 1,
            ["querytime"] = "09/25/2023 14:36:02",
            ["results"] = new JsonArray
            {
                new JsonObject
                {
                    ["userId"] = "4ue0ujotpmuu",
                    ["balance"] = 8060.00m,
                    ["exposure"] = -1940.00m
                }
            }
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
            .GetBalancesAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal(8060.00m, actual.ElementAt(0).Balance);
        Assert.Equal(1940.00m, actual.ElementAt(0).Exposure);
    }

    [Fact]
    public async Task Query_Too_Fast()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getBalance");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["alluser"] = 1.ToString()
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

        _ = await Assert
            .ThrowsAsync<HttpRequestException>(() => client
            .GetBalancesAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert)
            .ToArrayAsync()
            .AsTask());
    }
}
