using System.Text.Json.Nodes;

using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;
public class GameInterfaceWicketCricketClient_FundStatusTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var playerCode = "4ucmqplxb029";
        var orderId = "36B92C00-23E4-4209-B71E-2F7CB752759A";

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IGetService<FundStatusQuery, bool>>();

            var query = new FundStatusQuery
            {
                ApiUrl = apiUrl,
                Website = website,
                Cert = cert,
                PlayerCode = playerCode,
                OrderId = orderId
            };

            _ = fakeService.GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(true);

            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        var actual = await sut.FundStatusAsync(apiUrl, website, cert, playerCode, orderId);

        Assert.True(actual);
    }
}
