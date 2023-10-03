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
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            userId: query.PlayerCode,
            tsCode: query.Txid,
            amount: query.Amount,
            cancellationToken: Arg.Any<CancellationToken>())
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
