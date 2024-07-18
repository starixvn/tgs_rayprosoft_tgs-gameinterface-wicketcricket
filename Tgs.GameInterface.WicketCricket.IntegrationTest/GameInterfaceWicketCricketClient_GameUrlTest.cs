using System.Text.Json.Nodes;

using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;
public class GameInterfaceWicketCricketClient_GameUrlTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var exchangeApiUrl = new Uri("https://test81.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var agent = "testpginrma";
        var playerCode = "4ucmqplxb029";
        var currency = 2;

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IGetService<GameUrlQuery, GameUrlReslut>>();

            var query = new GameUrlQuery
            {
                ApiUrl = apiUrl,
                ExchangeApiUrl = exchangeApiUrl,
                Website = website,
                Cert = cert,
                Agent = agent,
                PlayerCode = playerCode,
                Currency = currency
            };

            _ = fakeService.GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(new GameUrlReslut(GameUrl: "https://test81.cckk77.net/"));

            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        var actual = await sut.GameUrlAsync(apiUrl, exchangeApiUrl, website, cert, agent, playerCode, currency);

        Assert.NotNull(actual);
        Assert.NotEmpty(actual);
    }
}
