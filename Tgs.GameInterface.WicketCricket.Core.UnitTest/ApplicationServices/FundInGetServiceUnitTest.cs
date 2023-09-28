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
            apiUrl: Arg.Any<Uri>(),
            website: Arg.Any<string>(),
            cert: Arg.Any<string>(),
            userId: Arg.Any<string>(),
            tsCode: Arg.Any<string>(),
            amount: Arg.Any<decimal>(),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new FundInGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query);

        Assert.NotNull(actual);
        Assert.Equal(result.Resp, actual.Resp);
    }
}
