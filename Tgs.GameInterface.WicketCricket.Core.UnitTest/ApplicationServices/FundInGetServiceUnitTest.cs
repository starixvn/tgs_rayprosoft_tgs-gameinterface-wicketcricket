using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundInGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundInQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            PlayerCode: "4ue0ujotpmuu",
            Txid: "1695630056",
            Amount: 1000.00m);

        var result = new WithdrawTransferResp(
            Resp: new JsonObject
            {
                ["status"] = "1",
                ["withdrawBalance"] = -1000.00m,
                ["remainingBalance"] = 0.000000m
            }.ToJsonString());
        _ = fakeGameApiClient
            .WithdrawAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            userId: query.PlayerCode,
            tsCode: query.Txid,
            amount: query.Amount,
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var service = new FundInGetService(
            fakeGameApiClient);

        var actual = await service
            .GetAsync(
            query);

        Assert.NotNull(actual);
        Assert.Equal(result.Resp, actual.Resp);
    }
}
