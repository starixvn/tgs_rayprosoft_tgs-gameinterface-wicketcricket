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
            apiUrl: Arg.Is(query.ApiUrl),
            website: Arg.Is(query.Website),
            cert: Arg.Is(query.Cert),
            userId: Arg.Is(query.PlayerCode),
            tsCode: Arg.Is(query.Txid))
            .Returns(true);

        var client = new FundStatusGetService(
            fakeGameApiClient);

        var actual = await client
            .GetAsync(
               query);

        Assert.True(actual);
    }
}
