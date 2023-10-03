using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class GameWagerSettledQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameWagerSettledQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow,
            ReportType: Common.ReportTypes.FANCY_BET,
            PageNumber: 1);

        var resps = new BetTransferSettledResp[] {
            new BetTransferSettledResp(
                EventId: 32649630,
                EventType: 4,
                CurrencyType: 2,
                CategoryType: 2,
                BetId: 96935568,
                UserId: "4ue0ujotpmuu",
                AgentId: "testpginrma",
                BetPlaced: DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"),
                MatchSettledDate: DateTimeOffset.Parse("2023-09-26T16:00:52.069+08:00"),
                MarketSettledDate: DateTimeOffset.Parse("2023-09-26T16:00:52.070+08:00"),
                BetStatus: "Settled",
                MatchStake:1000.000000m,
                ProfitLoss:1000.000000m,
                AdditionalData:new JsonObject
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
                }.ToJsonString())
        };
        _ = fakeGameApiClient
            .QuerySettleBetHistoryAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            reportType: Arg.Is(query.ReportType),
            pageNumber: Arg.Is(query.PageNumber),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(resps.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
            query)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).PlayerCode);
        Assert.Equal("32649630", actual.ElementAt(0).ExternalRoundId);
        Assert.Equal("2_96935568", actual.ElementAt(0).ExternalWagerId);
        Assert.Equal("4", actual.ElementAt(0).ExternalGameId);
        Assert.True(actual.ElementAt(0).IsClosed);
        Assert.False(actual.ElementAt(0).IsCancelled);
        Assert.False(actual.ElementAt(0).IsVoided);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"), actual.ElementAt(0).ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T16:00:52.070+08:00"), actual.ElementAt(0).ExternalClosedOn);
        Assert.Equal(1000.000000m, actual.ElementAt(0).Bet);
        Assert.Equal(2000.000000m, actual.ElementAt(0).Settle);
        Assert.Equal(1000.000000m, actual.ElementAt(0).WinLoss);
        Assert.Equal(resps.ElementAt(0).AdditionalData, actual.ElementAt(0).AdditionalData);
    }

    [Fact]
    public async Task Voided()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameWagerSettledQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            StartTime: DateTime.UtcNow.AddMinutes(-10),
            EndTime: DateTimeOffset.UtcNow,
            ReportType: Common.ReportTypes.FANCY_BET,
            PageNumber: 1);

        var resps = new BetTransferSettledResp[] {
            new BetTransferSettledResp(
                EventId: -10020452,
                EventType: 4,
                CurrencyType: 2,
                CategoryType: 2,
                BetId: 96935585,
                UserId: "4ue0ujotpmuu",
                AgentId: "testpginrma",
                BetPlaced: DateTimeOffset.Parse("2023-09-27T09:36:13.017+08:00"),
                MatchSettledDate: DateTimeOffset.Parse( "2023-09-27T10:20:52.302+08:00"),
                MarketSettledDate: DateTimeOffset.Parse( "2023-09-27T10:20:52.303+08:00"),
                BetStatus: "Voided",
                MatchStake:1000.000000m,
                ProfitLoss:0m,
                AdditionalData:new JsonObject
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
                }.ToJsonString())
        };
        _ = fakeGameApiClient
            .QuerySettleBetHistoryAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            startTime: Arg.Is(query.StartTime),
            endTime: Arg.Is(query.EndTime),
            reportType: Arg.Is(query.ReportType),
            pageNumber: Arg.Is(query.PageNumber),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(resps.ToAsyncEnumerable());

        var service = new GameWagerSettledQueryService(
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
            query)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).PlayerCode);
        Assert.Equal("-10020452", actual.ElementAt(0).ExternalRoundId);
        Assert.Equal("2_96935585", actual.ElementAt(0).ExternalWagerId);
        Assert.Equal("4", actual.ElementAt(0).ExternalGameId);
        Assert.True(actual.ElementAt(0).IsClosed);
        Assert.False(actual.ElementAt(0).IsCancelled);
        Assert.True(actual.ElementAt(0).IsVoided);
        Assert.Equal(DateTimeOffset.Parse("2023-09-27T09:36:13.017+08:00"), actual.ElementAt(0).ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.Parse("2023-09-27T10:20:52.303+08:00"), actual.ElementAt(0).ExternalClosedOn);
        Assert.Equal(1000.000000m, actual.ElementAt(0).Bet);
        Assert.Equal(0m, actual.ElementAt(0).Settle);
        Assert.Equal(0m, actual.ElementAt(0).WinLoss);
        Assert.Equal(resps.ElementAt(0).AdditionalData, actual.ElementAt(0).AdditionalData);
    }
}
