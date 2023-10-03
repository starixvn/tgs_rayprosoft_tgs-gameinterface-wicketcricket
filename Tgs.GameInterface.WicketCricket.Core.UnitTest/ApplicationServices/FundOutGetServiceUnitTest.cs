using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
    [Fact]
    public async Task FundOut_Normal_Process()
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
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Website),
            Arg.Is(query.Cert),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.Txid),
            Arg.Is(query.Amount))
            .Returns(result);

        var service = new FundOutGetService(
            fakeGameApiClient);

        var actual = await service
            .GetAsync(
            query);

        Assert.NotNull(actual);
        Assert.Equal(result.Resp, actual.Resp);
    }
}
