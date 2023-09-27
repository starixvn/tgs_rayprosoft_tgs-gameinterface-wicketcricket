using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundOutQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            PlayerCode: "4ue0ujotpmuu",
            Txid: "1695630056",
            Amount: 1000.00m);

        var result = new DepositTransferResp(
            Resp: new JsonObject
            {
                ["status"] = "1"
            }.ToJsonString());
        _ = fakeGameApiClient
            .DepositAsync(
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            userId: Arg.Is(query.PlayerCode),
            tsCode: Arg.Is(query.Txid),
            amount: Arg.Is(query.Amount),
            cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

        var client = new FundOutGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query)
           .ConfigureAwait(false);

        Assert.NotNull(actual);
        Assert.Equal(result.Resp, actual.Resp);
    }
}
