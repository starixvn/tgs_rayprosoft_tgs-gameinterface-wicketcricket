using System.Text.Json.Nodes;

using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceGlobalTechnologyClient_FundInTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var playerCode = "4ucmqplxb029";
        var orderId = "36B92C00-23E4-4209-B71E-2F7CB752759A";
        var amount = 10.0m;

        var result = new FundResult(
            Resp: new JsonObject
            {
                ["status"] = "1"
            }.ToJsonString());

        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            var fakeService = Substitute.For<IGetService<FundInQuery, FundResult>>();

            var query = new FundInQuery
            {
                ApiUrl = apiUrl,
                Website = website,
                Cert = cert,
                PlayerCode = playerCode,
                OrderId = orderId,
                Amount = amount
            };

            _ = fakeService.GetAsync(
                query: Arg.Is(query),
                cancellationToken: Arg.Any<CancellationToken>())
            .Returns(result);

            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        var actual = await sut.FundInAsync(apiUrl, website, cert, playerCode, orderId, amount);

        Assert.NotNull(actual);
        Assert.NotEmpty(actual.Resp);
    }
}
