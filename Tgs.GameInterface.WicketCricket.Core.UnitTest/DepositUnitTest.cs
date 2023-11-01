using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;

using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

using RichardSzalay.MockHttp;

using Tgs.GameInterface.WicketCricket.Core.Exceptions;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class DepositUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var userId = "4ue0ujotpmuu";
        var tsCode = "1695627094";
        var amount = 1000.00m;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/deposit");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["userId"] = userId,
            ["tsCode"] = tsCode,
            ["balance"] = 1000.00m.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1"
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync())
            .Respond(
            HttpStatusCode.OK,
            new StringContent(response.ToJsonString(), Encoding.UTF8, MediaTypeNames.Application.Json));

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
            .DepositAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            userId: userId,
            tsCode: tsCode,
            amount: amount);

        Assert.Equal(response.ToJsonString(), actual.Resp);
    }

    [Fact]
    public async Task Query_Too_Fast()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var userId = "4ue0ujotpmuu";
        var tsCode = "1695627094";
        var amount = 1000.23m;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/deposit");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["userId"] = userId,
            ["tsCode"] = tsCode,
            ["balance"] = 1000.23m.ToString()
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

        _ = await Assert.ThrowsAsync<MockHttpMatchException>
            (() => client.DepositAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                userId: userId,
                tsCode: tsCode,
                amount: amount)
            .AsTask());
    }

    [Fact]
    public async Task The_Deposit_And_Withdrawal_Amount_Format_Has_Two_Decimal_Places()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var userId = "4ue0ujotpmuu";
        var tsCode = "1695627094";
        var amount = 1000.236m;

        var fakeLog = NullLogger<GameApiClient>.Instance;
        var fakeOptions = Options.Create(new PolicySettingsOptions
        {
            RetryCount = 1,
            RetryWaitSec = 1
        });
        var fakeHttpClient = new MockHttpMessageHandler().ToHttpClient();

        var client = new GameApiClient(
            fakeLog,
            fakeOptions,
            fakeHttpClient);

        _ = await Assert
            .ThrowsAsync<DepositWithdrawalAmountException>(
            () => client
            .DepositAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                userId: userId,
                tsCode: tsCode,
                amount: amount)
            .AsTask());
    }

    [Fact]
    public async Task The_Deposit_Order_Has_Been_Repeated()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var userId = "4ue0ujotpmuu";
        var tsCode = "1695627094";
        var amount = 1000.23m;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/deposit");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["userId"] = userId,
            ["tsCode"] = tsCode,
            ["balance"] = 1000.23m.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1012"
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
            .ThrowsAsync<StatusCodeException>(
            () => client
            .DepositAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                userId: userId,
                tsCode: tsCode,
                amount: amount)
            .AsTask());
    }
}
