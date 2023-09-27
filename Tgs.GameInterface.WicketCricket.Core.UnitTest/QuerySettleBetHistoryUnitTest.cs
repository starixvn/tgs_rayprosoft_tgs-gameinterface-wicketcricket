using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json.Nodes;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using NSubstitute;
using RichardSzalay.MockHttp;
using Tgs.GameInterface.WicketCricket.Common;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest;

public class QuerySettleBetHistoryUnitTest
{
    [Fact]
    public async Task BOOKMAKER_ReportType_Is_3_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T06:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T07:00:00+00:00");
        var reportType = ReportTypes.BOOKMAKER;
        var pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 14:00",
            ["endDate"] = "2023-09-26 15:00",
            ["reportType"] = 3.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["resultList"] = new JsonArray
            {
                new JsonObject
                {
                    ["betPlaced"] = "2023-09-26T13:52:19.228+08:00",
                    ["matchSettledDate"] = "2023-09-26T14:00:40.397+08:00",
                    ["marketSettledDate"] = "2023-09-26T14:00:40.398+08:00",
                    ["eventTypeName"] = "Book CRICKET",
                    ["eventName"] = "AutoTestMarket v 09-26 13",
                    ["eventId"] = -10020383,
                    ["site"] = 44,
                    ["currencyType"] = 2,
                    ["eventType"] = 4,
                    ["selectionId"] = 33765,
                    ["categoryType"] = 5,
                    ["ip"] = "114.33.41.237",
                    ["isp"] = "Chunghwa Telecom Co. Ltd.",
                    ["matchOddsReq"] = 30.00,
                    ["betId"] = 4247502,
                    ["userId"] = "4ue0ujotpmuu",
                    ["extension1"] = null,
                    ["agentId"] = "testpginrma",
                    ["oddsType"] = 1,
                    ["updateDate"] = 1695708040442,
                    ["marketName"] = "Bookmaker",
                    ["selectionName"] =  "Bookmaker Home",
                    ["matchType"] = "Back",
                    ["marketId"] =  15986,
                    ["transactionData"] = new JsonArray{
                        new JsonObject{
                            ["betTaken"] = "2023-09-26T13:52:19.228+08:00",
                            ["txStake"] = 1000.000000,
                            ["txOddsMatched"] = 30.00,
                            ["actualOdds"] = 30.00
                        }
                    },
                    ["matchAvgOdds"] = 30.00,
                    ["betStatus"] = "Settled",
                    ["profitLoss"] = 300.000000,
                    ["reduction"] =  0,
                    ["matchStake"] = 1000.000000
                }
            }
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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
            .QuerySettleBetHistoryAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            startTime: startTime,
            endTime: endTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal(-10020383, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(5, actual.ElementAt(0).CategoryType);
        Assert.Equal(4247502, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T13:52:19.228+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T14:00:40.397+08:00"), actual.ElementAt(0).MatchSettledDate);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T14:00:40.398+08:00"), actual.ElementAt(0).MarketSettledDate);
        Assert.Equal("Settled", actual.ElementAt(0).BetStatus);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchStake);
        Assert.Equal(300.000000m, actual.ElementAt(0).ProfitLoss);
        Assert.Equal(response["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task EXCHANGE_ReportType_Is_0_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T08:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T09:00:00+00:00");
        var reportType = ReportTypes.EXCHANGE;
        var pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 16:00",
            ["endDate"] = "2023-09-26 17:00",
            ["reportType"] = 0.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["resultList"] = new JsonArray
            {
                new JsonObject
                {
                    ["eventTypeName"] = "CRICKET",
                    ["eventName"] = "WA Women v Victoria Women",
                    ["eventId"] = 32664175,
                    ["site"] = 44,
                    ["currencyType"] = 2,
                    ["eventType"] = 4,
                    ["selectionId"] = 42077906,
                    ["categoryType"] = 1,
                    ["ip"] = "114.33.41.237",
                    ["isp"] = "Chunghwa Telecom Co. Ltd.",
                    ["matchOddsReq"] = 1.41,
                    ["userId"] = "4ue0ujotpmuu",
                    ["betId"] = 120848865,
                    ["updateDate"] = 1695715894316,
                    ["agentId"] = "testpginrma",
                    ["oddsType"] = 0,
                    ["marketName"] = "Match Odds",
                    ["selectionName"] =  "Victoria Women",
                    ["matchType"] = "Lay",
                    ["marketId"] =  "1.218837903",
                    ["fancyBetRuns"] = null,
                    ["betPlaced"] = "2023-09-26T11:16:23.373+08:00",
                    ["extension1"] = "",
                    ["matchSettledDate"] = "2023-09-26T16:11:34.311+08:00",
                    ["marketSettledDate"] = "2023-09-26T16:11:34.348+08:00",
                    ["transactionData"] = new JsonArray{
                        new JsonObject{
                            ["betTaken"] = "2023-09-26T11:16:23.374+08:00",
                            ["txStake"] = 1000.000000,
                            ["txLiability"] = -400.00000000,
                            ["txOddsMatched"] = 1.40,
                            ["actualOdds"] = 1.40,
                            ["txFancyBetRuns"] = null
                        }
                    },
                    ["matchAvgOdds"] = 1.40,
                    ["betStatus"] = "Settled",
                    ["reduction"] =  0.0000,
                    ["matchStake"] =  1000.000000,
                    ["profitLoss"] = 1000.000000
                }
            }
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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
            .QuerySettleBetHistoryAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            startTime: startTime,
            endTime: endTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal(32664175, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(1, actual.ElementAt(0).CategoryType);
        Assert.Equal(120848865, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:16:23.373+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:11:34.311+08:00"), actual.ElementAt(0).MatchSettledDate);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:11:34.348+08:00"), actual.ElementAt(0).MarketSettledDate);
        Assert.Equal("Settled", actual.ElementAt(0).BetStatus);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchStake);
        Assert.Equal(1000.000000m, actual.ElementAt(0).ProfitLoss);
        Assert.Equal(response["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task FANCY_BET_ReportType_Is_7_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T08:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T09:00:00+00:00");
        var reportType = ReportTypes.FANCY_BET;
        var pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 16:00",
            ["endDate"] = "2023-09-26 17:00",
            ["reportType"] = 7.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["resultList"] = new JsonArray
            {
                new JsonObject
                {
                    ["betPlaced"] = "2023-09-26T15:33:04.693+08:00",
                    ["matchSettledDate"] = "2023-09-26T16:00:52.069+08:00",
                    ["marketSettledDate"] = "2023-09-26T16:00:52.070+08:00",
                    ["eventTypeName"] = "CRICKET",
                    ["eventName"] = "AutoTestMarket v 09-26 15",
                    ["eventId"] = -10020389,
                    ["site"] = 44,
                    ["currencyType"] = 2,
                    ["eventType"] = 4,
                    ["selectionId"] = -1,
                    ["categoryType"] = 2,
                    ["ip"] = "114.33.41.237",
                    ["isp"] = "Chunghwa Telecom Co. Ltd.",
                    ["matchOddsReq"] = 100.00,
                    ["betId"] = 96935568,
                    ["userId"] = "4ue0ujotpmuu",
                    ["extension1"] = null,
                    ["agentId"] = "testpginrma",
                    ["oddsType"] = 1,
                    ["updateDate"] = 1695715252118,
                    ["marketName"] = "FANCY_BET",
                    ["selectionName"] =  "first inning 10 over runs",
                    ["matchType"] = "No",
                    ["marketId"] =  "46458",
                    ["transactionData"] = new JsonArray{
                        new JsonObject{
                            ["betTaken"] = "2023-09-26T15:33:04.693+08:00",
                            ["txStake"] = 1000.000000,
                            ["txLiability"] = -1000.0000000000000000,
                            ["txOddsMatched"] = 100.00,
                            ["actualOdds"] = 100.00,
                            ["txFancyBetRuns"] = 10,
                        }
                    },
                    ["matchAvgOdds"] = 100.00,
                    ["betStatus"] = "Settled",
                    ["profitLoss"] = 1000.000000,
                    ["reduction"] =  0,
                    ["matchStake"] = 1000.000000
                }
            }
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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
            .QuerySettleBetHistoryAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            startTime: startTime,
            endTime: endTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal(-10020389, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(2, actual.ElementAt(0).CategoryType);
        Assert.Equal(96935568, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:00:52.069+08:00"), actual.ElementAt(0).MatchSettledDate);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:00:52.070+08:00"), actual.ElementAt(0).MarketSettledDate);
        Assert.Equal("Settled", actual.ElementAt(0).BetStatus);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchStake);
        Assert.Equal(1000.000000m, actual.ElementAt(0).ProfitLoss);
        Assert.Equal(response["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task FANCY_BET_ReportType_Is_7_Voided()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T02:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T03:00:00+00:00");
        var reportType = ReportTypes.FANCY_BET;
        var pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 10:00",
            ["endDate"] = "2023-09-26 11:00",
            ["reportType"] = 7.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["resultList"] = new JsonArray
            {
                new JsonObject
                {
                    ["betPlaced"] = "2023-09-27T09:36:13.017+08:00",
                    ["matchSettledDate"] = "2023-09-27T10:20:52.302+08:00",
                    ["marketSettledDate"] = "2023-09-27T10:20:52.303+08:00",
                    ["eventTypeName"] = "CRICKET",
                    ["eventName"] = "AutoFancyBookMakerVoid v 09-27 09",
                    ["eventId"] = -10020452,
                    ["site"] = 44,
                    ["currencyType"] = 2,
                    ["eventType"] = 4,
                    ["selectionId"] = -1,
                    ["categoryType"] = 2,
                    ["ip"] = "114.33.41.237",
                    ["isp"] = "Chunghwa Telecom Co. Ltd.",
                    ["matchOddsReq"] = 100.00,
                    ["betId"] = 96935585,
                    ["userId"] = "4ue0ujotpmuu",
                    ["extension1"] = null,
                    ["agentId"] = "testpginrma",
                    ["oddsType"] = 1,
                    ["updateDate"] = 1695781368979,
                    ["marketName"] = "FANCY_BET",
                    ["selectionName"] =  "first inning 10 over runs",
                    ["matchType"] = "No",
                    ["marketId"] =  "46515",
                    ["fancyBetRuns"] =  10,
                    ["fancyBetExtendedRuns"] =  0,
                    ["fancyBetMarketType"] =  1,
                    ["transactionData"] = new JsonArray{}  ,
                    ["matchAvgOdds"] = 0,
                    ["betStatus"] = "Voided",
                    ["profitLoss"] = 0,
                    ["reduction"] =  0,
                    ["matchStake"] = 1000.000000
                }
            }
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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
            .QuerySettleBetHistoryAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            startTime: startTime,
            endTime: endTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal(-10020452, actual.ElementAt(0).EventId);
        Assert.Equal(4, actual.ElementAt(0).EventType);
        Assert.Equal(2, actual.ElementAt(0).CurrencyType);
        Assert.Equal(2, actual.ElementAt(0).CategoryType);
        Assert.Equal(96935585, actual.ElementAt(0).BetId);
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).UserId);
        Assert.Equal("testpginrma", actual.ElementAt(0).AgentId);
        Assert.Equal(DateTimeOffset.Parse("2023-09-27T09:36:13.017+08:00"), actual.ElementAt(0).BetPlaced);
        Assert.Equal(DateTimeOffset.Parse("2023-09-27T10:20:52.302+08:00"), actual.ElementAt(0).MatchSettledDate);
        Assert.Equal(DateTimeOffset.Parse("2023-09-27T10:20:52.303+08:00"), actual.ElementAt(0).MarketSettledDate);
        Assert.Equal("Voided", actual.ElementAt(0).BetStatus);
        Assert.Equal(1000.000000m, actual.ElementAt(0).MatchStake);
        Assert.Equal(0m, actual.ElementAt(0).ProfitLoss);
        Assert.Equal(response["resultList"]!.AsArray()!.ElementAt(0)!.ToJsonString(), actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task No_Data()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T02:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T03:00:00+00:00");
        var reportType = ReportTypes.FANCY_BET;
        var pageNumber = 2;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 10:00",
            ["endDate"] = "2023-09-26 11:00",
            ["reportType"] = 7.ToString(),
            ["pageNumber"] = pageNumber.ToString(),
            ["timeZone"] = 1.ToString(),
            ["isDateISO8601"] = 1.ToString()
        };
        var content = new FormUrlEncodedContent(parameters);
        var response = new JsonObject
        {
            ["status"] = "1",
            ["totalPage"] = 1,
            ["currentPage"] = 1,
            ["resultList"] = new JsonArray
            {
                new JsonObject
                {
                    ["betPlaced"] = "2023-09-27T09:36:13.017+08:00",
                    ["matchSettledDate"] = "2023-09-27T10:20:52.302+08:00",
                    ["marketSettledDate"] = "2023-09-27T10:20:52.303+08:00",
                    ["eventTypeName"] = "CRICKET",
                    ["eventName"] = "AutoFancyBookMakerVoid v 09-27 09",
                    ["eventId"] = -10020452,
                    ["site"] = 44,
                    ["currencyType"] = 2,
                    ["eventType"] = 4,
                    ["selectionId"] = -1,
                    ["categoryType"] = 2,
                    ["ip"] = "114.33.41.237",
                    ["isp"] = "Chunghwa Telecom Co. Ltd.",
                    ["matchOddsReq"] = 100.00,
                    ["betId"] = 96935585,
                    ["userId"] = "4ue0ujotpmuu",
                    ["extension1"] = null,
                    ["agentId"] = "testpginrma",
                    ["oddsType"] = 1,
                    ["updateDate"] = 1695781368979,
                    ["marketName"] = "FANCY_BET",
                    ["selectionName"] =  "first inning 10 over runs",
                    ["matchType"] = "No",
                    ["marketId"] =  "46515",
                    ["fancyBetRuns"] =  10,
                    ["fancyBetExtendedRuns"] =  0,
                    ["fancyBetMarketType"] =  1,
                    ["transactionData"] = new JsonArray{}  ,
                    ["matchAvgOdds"] = 0,
                    ["betStatus"] = "Voided",
                    ["profitLoss"] = 0,
                    ["reduction"] =  0,
                    ["matchStake"] = 1000.000000
                }
            }
        };
        var handler = new MockHttpMessageHandler();
        _ = handler
            .Expect(HttpMethod.Post, url.ToString())
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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
            .QuerySettleBetHistoryAsync(
            apiUrl: apiUrl,
            website: website,
            cert: cert,
            startTime: startTime,
            endTime: endTime,
            reportType: reportType,
            pageNumber: pageNumber)
            .ToArrayAsync();

        Assert.False(actual.Any());
    }

    [Fact]
    public async Task Query_Too_Fast()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-26T08:00:00+00:00");
        var endTime = DateTimeOffset.Parse("2023-09-26T09:00:00+00:00");
        var reportType = ReportTypes.FANCY_BET;
        var pageNumber = 1;

        var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
        var parameters = new Dictionary<string, string>
        {
            ["cert"] = cert,
            ["startDate"] = "2023-09-26 16:00",
            ["endDate"] = "2023-09-26 17:00",
            ["reportType"] = 7.ToString(),
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
            .WithContent(
            await content
            .ReadAsStringAsync(
                cancellationToken: Arg.Any<CancellationToken>()))
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

        _ = await Assert
            .ThrowsAsync<HttpRequestException>(() => client
            .QuerySettleBetHistoryAsync(
                apiUrl: apiUrl,
                website: website,
                cert: cert,
                startTime: startTime,
                endTime: endTime,
                reportType: reportType,
                pageNumber: pageNumber)
            .ToArrayAsync()
            .AsTask());
    }
}
