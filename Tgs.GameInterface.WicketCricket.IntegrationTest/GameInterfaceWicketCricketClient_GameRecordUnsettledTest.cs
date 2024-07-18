using System.Text.Json.Nodes;

using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceWicketCricketClient_GameRecordUnsettledTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var lastUpdateTime = DateTimeOffset.Parse("2023-09-27T00:00:00+08:00");
        var reportType = ReportTypes.FancyBet;
        var pageNumber = 1;

        var result = new GameRecordReslut(
           PlayerCode: "4ue0ujotpmuu",
           ExternalRoundId: "32649630",
           ExternalBetId: "1_120848867",
           ExternalGameId: "4",
           IsClosed: false,
           IsCancelled: false,
           IsVoided: false,
           ExternalCreatedOn: DateTimeOffset.Parse("2023-09-26T15:33:04.693+08:00"),
           ExternalClosedOn: DateTimeOffset.UnixEpoch,
           Bet: 1000.0m,
           Settle: 0.0m,
           WinLoss: 0.0m,
           AdditionalData: new JsonObject
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
               ["transactionData"] = new JsonArray { },
               ["matchAvgOdds"] = 4.3,
               ["betStatus"] = "Not Settled",
               ["matchAmount"] = 1000.0,
               ["unMatchAmount"] = 0.0,
               ["dateMatched"] = "2023-09-26T11:17:14.907+08:00"
           }.ToJsonString());

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IQueryService<GameRecordUnsettledQuery, GameRecordReslut>>();

            var query = new GameRecordUnsettledQuery
            {
                ApiUrl = apiUrl,
                Website = website,
                Cert = cert,
                LastUpdateTime = lastUpdateTime,
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

        var actual = await sut.GameRecordUnsettledAsync(apiUrl, website, cert, lastUpdateTime, reportType, pageNumber).ToArrayAsync();

        Assert.NotNull(actual);
        Assert.Equal(result.PlayerCode, actual.ElementAt(0).PlayerCode);
        Assert.Equal(result.ExternalRoundId, actual.ElementAt(0).ExternalRoundId);
        Assert.Equal(result.ExternalBetId, actual.ElementAt(0).ExternalBetId);
        Assert.Equal(result.ExternalGameId, actual.ElementAt(0).ExternalGameId);
        Assert.False(actual.ElementAt(0).IsClosed);
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
