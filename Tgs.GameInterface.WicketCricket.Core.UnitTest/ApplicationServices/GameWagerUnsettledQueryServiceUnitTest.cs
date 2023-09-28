using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class GameWagerUnsettledQueryServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new GameWagerUnsettledQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            LastUpdateTime: DateTimeOffset.UtcNow,
            ReportType: Common.ReportTypes.EXCHANGE,
            PageNumber: 1);

        var resps = new BetTransferUnsettledResp[] {
            new BetTransferUnsettledResp(
                EventId: 32649630,
                EventType: 4,
                CurrencyType: 2,
                CategoryType: 1,
                BetId: 120848867,
                UserId: "4ue0ujotpmuu",
                AgentId: "testpginrma",
                BetPlaced: DateTimeOffset.Parse("2023-09-26T11:17:14.906+08:00"),
                MatchAmount: 1000.0m,
                UnMatchAmount: 0.0m,
                DateMatched: DateTimeOffset.Parse( "2023-09-26T11:17:14.907+08:00"),
                AdditionalData:new JsonObject
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
                }.ToJsonString())
        };
        _ = fakeGameApiClient
            .QueryCurrentBetsByLastUpdateDateAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            lastUpdateTime: Arg.Is(query.LastUpdateTime),
            reportType: Arg.Is(query.ReportType),
            pageNumber: Arg.Is(query.PageNumber),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(resps.ToAsyncEnumerable());

        var service = new GameWagerUnsettledQueryService(
            fakeGameApiClient);

        var actual = await service
            .QueryAsync(
            query)
            .ToArrayAsync();

        Assert.True(actual.Any());
        Assert.Equal("4ue0ujotpmuu", actual.ElementAt(0).PlayerCode);
        Assert.Equal("1_120848867", actual.ElementAt(0).ExternalWagerId);
        Assert.Equal("4", actual.ElementAt(0).ExternalGameId);
        Assert.False(actual.ElementAt(0).IsClosed);
        Assert.False(actual.ElementAt(0).IsCancelled);
        Assert.False(actual.ElementAt(0).IsVoided);
        Assert.Equal(DateTimeOffset.Parse("2023-09-26T11:17:14.906+08:00"), actual.ElementAt(0).ExternalCreatedOn);
        Assert.Equal(DateTimeOffset.UnixEpoch, actual.ElementAt(0).ExternalClosedOn);
        Assert.Equal(1000.0m, actual.ElementAt(0).Bet);
        Assert.Equal(0, actual.ElementAt(0).Settle);
        Assert.Equal(0, actual.ElementAt(0).WinLoss);
        Assert.Equal(resps.ElementAt(0).AdditionalData, actual.ElementAt(0).AdditionalData);
    }
}
