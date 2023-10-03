using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var query = new FundStatusQuery(
            ApiUrl: new Uri("https://apiinfo.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            PlayerCode: "4ue0ujotpmuu",
            Txid: "1695630056");

        _ = fakeGameApiClient
            .CheckTransferRecordAsync(
            Arg.Is(query.ApiUrl),
            Arg.Is(query.Website),
            Arg.Is(query.Cert),
            Arg.Is(query.PlayerCode),
            Arg.Is(query.Txid),
            Arg.Any<CancellationToken>())
            .Returns(ValueTask.FromResult(true));

        var service = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await service
            .GetAsync(
            query);

        Assert.True(actual);
    }
}
