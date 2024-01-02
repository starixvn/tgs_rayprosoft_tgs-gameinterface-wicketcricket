using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class QueryCurrentBetsByLastUpdateDateUnitTest
{
    [Fact]
    public async Task BOOKMAKER_ReportType_Is_3_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.BOOKMAKER;
        int pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 3.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = 1,
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["matched"] = new JsonObject
            {
                ["resultList"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["eventTypeName"] = "Book CRICKET",
                        ["eventName"] = "AutoFancyBookMakerResettle v 09-22 13",
                        ["eventId"] = -10020074,
                        ["site"] = 44,
                        ["currencyType"] = 2,
                        ["eventType"] = 4,
                        ["selectionId"] = 33192,
                        ["categoryType"] = 5,
                        ["ip"] = "114.33.41.237",
                        ["isp"] = "Chunghwa Telecom Co. Ltd.",
                        ["matchOddsReq"] = 30.00,
                        ["betId"] = 4247500,
                        ["userId"] = "4ue0ujotpmuu",
                        ["extension1"] = null,
                        ["agentId"] = "testpginrma",
                        ["oddsType"] = 1,
                        ["updateDate"] = 1695698234905,
                        ["betPlaced"] = "2023-09-26T11:40:15.726+08:00",
                        ["matchSettledDate"] = "",
                        ["marketSettledDate"] = "",
                        ["fancyBetRuns"] = null,
                        ["marketName"] = "Bookmaker",
                        ["selectionName"] = "Bookmaker Home",
                        ["matchType"] = "Back",
                        ["marketId"] = 15700,
                        ["transactionData"] = null,
                        ["matchAvgOdds"] =  30.00,
                        ["betStatus"] = "Not Settled",
                        ["matchAmount"] =1000.000000,
                        ["unMatchAmount"] =0,
                        ["dateMatched"] = "2023-09-26T11:40:15.726+08:00"
                    }
                }
            },
            ["unmatched"] = new JsonObject
            {
                ["resultList"] = new JsonArray { }
            },
            ["lastUpdateDate"] = 1695698234905
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
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            lastUpdateTime: lastUpdateTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.NotEmpty(actual);
        Assert.Equal(-10020074, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(5, actual.ElementAt(0).CategoryType);
        Assert.Equal(4247500, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:40:15.726+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchAmount);
        Assert.Equal(0.0m, actual.ElementAt(0).UnMatchAmount);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:40:15.726+08:00"), actual.ElementAt(0).DateMatched);
        Assert.Equal(response["matched"]!["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task EXCHANGE_ReportType_Is_0_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.EXCHANGE;
        int pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 0.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = 1,
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["matched"] = new JsonObject
            {
                ["resultList"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["eventTypeName"] = "CRICKET",
                        ["eventName"] = "South Australia v Western Australia",
                        ["eventId"] = 32649630,
                        ["site"] = 44,
                        ["currencyType"] = 2,
                        ["eventType"] = 4,
                        ["selectionId"] = 86351,
                        ["categoryType"] = 1,
                        ["ip"] = "114.33.41.237",
                        ["isp"] = "Chunghwa Telecom Co. Ltd.",
                        ["matchOddsReq"] = 4.3,
                        ["userId"] = "4ue0ujotpmuu",
                        ["betId"] = 120848867,
                        ["updateDate"] = 1695698234905,
                        ["agentId"] = "testpginrma",
                        ["oddsType"] = 0,
                        ["betPlaced"] = "2023-09-26T11:17:14.906+08:00",
                        ["matchSettledDate"] = "",
                        ["marketName"] = "Match Odds",
                        ["selectionName"] = "South Australia",
                        ["matchType"] = "Lay",
                        ["marketId"] = "1.218645672",
                        ["fancyBetRuns"] = null,
                        ["marketSettledDate"] = "",
                        ["extension1"] = "",
                        ["transactionData"] = new JsonArray{ },
                        ["matchAvgOdds"] = 4.3,
                        ["betStatus"] = "Not Settled",
                        ["matchAmount"] = 1000.0,
                        ["unMatchAmount"] = 0.0,
                        ["dateMatched"] = "2023-09-26T11:17:14.907+08:00"
                    }
                }
            },
            ["unmatched"] = new JsonObject
            {
                ["resultList"] = new JsonArray { }
            },
            ["lastUpdateDate"] = 1695698234905
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
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            lastUpdateTime: lastUpdateTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.NotEmpty(actual);
        Assert.Equal(32649630, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(1, actual.ElementAt(0).CategoryType);
        Assert.Equal(120848867, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:17:14.906+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(1000.0m, actual.ElementAt(0).MatchAmount);
        Assert.Equal(0.0m, actual.ElementAt(0).UnMatchAmount);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:17:14.907+08:00"), actual.ElementAt(0).DateMatched);
        Assert.Equal(response["matched"]!["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task FANCY_BET_ReportType_Is_7_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.FANCY_BET;
        int pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 7.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = 1,
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["matched"] = new JsonObject
            {
                ["resultList"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["eventTypeName"] = "CRICKET",
                        ["eventName"] = "AutoTestMarket v 09-26 15",
                        ["eventId"] =  -10020389,
                        ["site"] = 44,
                        ["currencyType"] = 2,
                        ["eventType"] = 4,
                        ["selectionId"] = -1,
                        ["categoryType"] =  2,
                        ["ip"] = "114.33.41.237",
                        ["isp"] = "Chunghwa Telecom Co. Ltd.",
                        ["matchOddsReq"] = 100.00,
                        ["betId"] = 96935568,
                        ["userId"] = "4ue0ujotpmuu",
                        ["extension1"] = null,
                        ["agentId"] = "testpginrma",
                        ["oddsType"] = 1,
                        ["updateDate"] = 1695713584695,
                        ["betPlaced"] = "2023-09-26T15:33:04.693+08:00",
                        ["matchSettledDate"] = "",
                        ["marketName"] = "FANCY_BET",
                        ["selectionName"] = "first inning 10 over runs",
                        ["matchType"] = "No",
                        ["marketId"] = 46458,
                        ["fancyBetRuns"] = 10,
                        ["fancyBetExtendedRuns"] = 0,
                        ["fancyBetMarketType"] = 1,
                        ["transactionData"] = null,
                        ["matchAvgOdds"] =  100.00,
                        ["betStatus"] = "Not Settled",
                        ["matchAmount"] = 1000.000000,
                        ["unMatchAmount"] = 0,
                        ["dateMatched"] = "2023-09-26T15:33:04.693+08:00"
                    }
                }
            },
            ["unmatched"] = new JsonObject
            {
                ["resultList"] = new JsonArray { }
            },
            ["lastUpdateDate"] = 1695698234905
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
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            lastUpdateTime: lastUpdateTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.NotEmpty(actual);
        Assert.Equal(-10020389, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(2, actual.ElementAt(0).CategoryType);
        Assert.Equal(96935568, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchAmount);
        Assert.Equal(0m, actual.ElementAt(0).UnMatchAmount);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"), actual.ElementAt(0).DateMatched);
        Assert.Equal(response["matched"]!["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task No_Data()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.SPORTSBOOK;
        var pageNumber = 2;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 2.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = 1,
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["matched"] = new JsonObject
            {
                ["resultList"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["eventTypeName"] = "S/R CRICKET",
                        ["eventName"] = "AutoTest 09-26 14 v Pretoria Capitals Srl vs. Paarl Royals Srl",
                        ["eventId"] =  -10020371,
                        ["site"] = 44,
                        ["currencyType"] = 2,
                        ["eventType"] = 4,
                        ["selectionId"] = 4337705606,
                        ["categoryType"] =  4,
                        ["ip"] = "114.33.41.237",
                        ["isp"] = "Chunghwa Telecom Co. Ltd.",
                        ["matchOddsReq"] = 19.50,
                        ["userId"] = "4ue0ujotpmuu",
                        ["betId"] = 120832607,
                        ["updateDate"] = 1695715470719,
                        ["agentId"] = "testpginrma",
                        ["oddsType"] = 0,
                        ["betPlaced"] = "2023-09-26T16:04:30.719+08:00",
                        ["matchSettledDate"] = "",
                        ["marketName"] = "Winner (incl. super over)",
                        ["selectionName"] = "Pretoria Capitals Srl",
                        ["matchType"] = "Back",
                        ["marketId"] = "530279440",
                        ["fancyBetRuns"] = null,
                        ["marketSettledDate"] = "",
                        ["extension1"] = "",
                        ["transactionData"] = new JsonArray{ },
                        ["matchAvgOdds"] =  19.50,
                        ["betStatus"] = "Not Settled",
                        ["matchAmount"] = 867.000000,
                        ["unMatchAmount"] = 0.000000,
                        ["dateMatched"] = "2023-09-26T16:04:30.724+08:00"
                    }
                }
            },
            ["unmatched"] = new JsonObject
            {
                ["resultList"] = new JsonArray { }
            },
            ["lastUpdateDate"] = 1695698234905
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
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            lastUpdateTime: lastUpdateTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.Empty(actual);
    }

    [Fact]
    public async Task Query_Too_Fast()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.SPORTSBOOK;
        int pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 2.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
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
            .ThrowsAsync<MockHttpMatchException>(
            () => client.QueryCurrentBetsByLastUpdateDateAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                lastUpdateTime: lastUpdateTime,
                reportType: reportType,
                pageNumber: pageNumber)
            .ToArrayAsync()
            .AsTask());
    }

    [Fact]
    public async Task SPORTSBOOK_ReportType_Is_2_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-25T16:00:00+00:00");
        var reportType = ReportTypes.SPORTSBOOK;
        int pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["lastUpdateDate"] = 1695657600000.ToString(),
            ["reportType"] = 2.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = 1,
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["matched"] = new JsonObject
            {
                ["resultList"] = new JsonArray
                {
                    new JsonObject
                    {
                        ["eventTypeName"] = "S/R CRICKET",
                        ["eventName"] = "AutoTest 09-26 14 v Pretoria Capitals Srl vs. Paarl Royals Srl",
                        ["eventId"] =  -10020371,
                        ["site"] = 44,
                        ["currencyType"] = 2,
                        ["eventType"] = 4,
                        ["selectionId"] = 4337705606,
                        ["categoryType"] =  4,
                        ["ip"] = "114.33.41.237",
                        ["isp"] = "Chunghwa Telecom Co. Ltd.",
                        ["matchOddsReq"] = 19.50,
                        ["userId"] = "4ue0ujotpmuu",
                        ["betId"] = 120832607,
                        ["updateDate"] = 1695715470719,
                        ["agentId"] = "testpginrma",
                        ["oddsType"] = 0,
                        ["betPlaced"] = "2023-09-26T16:04:30.719+08:00",
                        ["matchSettledDate"] = "",
                        ["marketName"] = "Winner (incl. super over)",
                        ["selectionName"] = "Pretoria Capitals Srl",
                        ["matchType"] = "Back",
                        ["marketId"] = "530279440",
                        ["fancyBetRuns"] = null,
                        ["marketSettledDate"] = "",
                        ["extension1"] = "",
                        ["transactionData"] = new JsonArray{ },
                        ["matchAvgOdds"] =  19.50,
                        ["betStatus"] = "Not Settled",
                        ["matchAmount"] = 867.000000,
                        ["unMatchAmount"] = 0.000000,
                        ["dateMatched"] = "2023-09-26T16:04:30.724+08:00"
                    }
                }
            },
            ["unmatched"] = new JsonObject
            {
                ["resultList"] = new JsonArray { }
            },
            ["lastUpdateDate"] = 1695698234905
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
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            lastUpdateTime: lastUpdateTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.NotEmpty(actual);
        Assert.Equal(-10020371, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(4, actual.ElementAt(0).CategoryType);
        Assert.Equal(120832607, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:04:30.719+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(867.000000m, actual.ElementAt(0).MatchAmount);
        Assert.Equal(0m, actual.ElementAt(0).UnMatchAmount);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:04:30.724+08:00"), actual.ElementAt(0).DateMatched);
        Assert.Equal(response["matched"]!["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }
}
