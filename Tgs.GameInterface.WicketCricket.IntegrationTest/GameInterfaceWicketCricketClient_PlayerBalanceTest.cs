using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceWicketCricketClient_PlayerBalanceTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var playerCode = "4ucmqplxb029";

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IGetService<PlayerBalanceQuery, PlayerBalanceResult>>();

            var query = new PlayerBalanceQuery
            {
                ApiUrl = apiUrl,
                Website = website,
                Cert = cert,
                PlayerCode = playerCode
            };

            _ = fakeService.GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new PlayerBalanceResult
            {
                IsPlayerExist = true,
                FreeBalance = 100,
                OutstandingBalance = 900
            });

            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        var actual = await sut.PlayerBalanceAsync(apiUrl, website, cert, playerCode);

        Assert.NotNull(actual);
        Assert.True(actual.IsPlayerExist);
        Assert.Equal(1000, actual.Balance);
        Assert.Equal(100, actual.FreeBalance);
        Assert.Equal(900, actual.OutstandingBalance);
    }
}
