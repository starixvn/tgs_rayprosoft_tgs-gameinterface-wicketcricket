using System.Text.Json.Nodes;

using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceWicketCricketClient_GameRecordSettled
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var startTime = DateTimeOffset.Parse("2023-09-27T00:00:00+08:00");
        var endTime = DateTimeOffset.Parse("2023-09-28T00:00:00+08:00");
        var reportType = ReportTypes.FancyBet;
        var pageNumber = 1;

        var result = new GameRecordReslut(
           PlayerCode: "4ue0ujotpmuu",
           ExternalRoundId: "32649630",
           ExternalBetId: "2_96935568",
           ExternalGameId: "4",
           IsClosed: true,
           IsCancelled: false,
           IsVoided: false,
           ExternalCreatedOn: DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"),
           ExternalClosedOn: DateTimeOffset.Parse("2023-09-26T16:00:52.070+08:00"),
           Bet: 1000.000000m,
           Settle: 2000.000000m,
           WinLoss: 1000.000000m,
           AdditionalData: new JsonObject
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
               ["selectionName"] = "first inning 10 over runs",
               ["matchType"] = "No",
               ["marketId"] = "46458",
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
               ["reduction"] = 0,
               ["matchStake"] = 1000.000000
           }.ToJsonString());

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameRecordSettledQuery, GameRecordReslut>>();

            var query = new GameRecordSettledQuery
            {
                ApiUrl = apiUrl,
                Website = website,
                Cert = cert,
                StartTime = startTime,
                EndTime = endTime,
                ReportType = Common.ReportTypes.FANCY_BET,
                PageNumber = pageNumber,
            };

            _ = fakeService.QueryAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new[] { result }.ToAsyncEnumerable());

            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        var actual = await sut.GameRecordSettledAsync(apiUrl, website, cert, startTime, endTime, reportType, pageNumber).ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Equal(result.PlayerCode, actual.ElementAt(0).PlayerCode);
        Assert.Equal(result.ExternalRoundId, actual.ElementAt(0).ExternalRoundId);
        Assert.Equal(result.ExternalBetId, actual.ElementAt(0).ExternalBetId);
        Assert.Equal(result.ExternalGameId, actual.ElementAt(0).ExternalGameId);
        Assert.True(actual.ElementAt(0).IsClosed);
        Assert.False(actual.ElementAt(0).IsCancelled);
        Assert.False(actual.ElementAt(0).IsVoided);
        Assert.Equal(result.ExternalCreatedOn, actual.ElementAt(0).ExternalCreatedOn.ToDateTimeOffset());
        Assert.Equal(result.ExternalClosedOn, actual.ElementAt(0).ExternalClosedOn.ToDateTimeOffset());
        Assert.Equal(result.Bet, actual.ElementAt(0).Bet.ToDecimal());
        Assert.Equal(result.Settle, actual.ElementAt(0).Settle.ToDecimal());
        Assert.Equal(result.WinLoss, actual.ElementAt(0).WinLoss.ToDecimal());
        Assert.Equal(result.AdditionalData, actual.ElementAt(0).AdditionalData);
    }
}
