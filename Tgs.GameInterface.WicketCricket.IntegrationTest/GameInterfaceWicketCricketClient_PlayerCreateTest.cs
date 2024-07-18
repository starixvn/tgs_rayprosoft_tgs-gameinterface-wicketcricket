using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceWicketCricketClient_PlayerCreateTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var agent = "testpginrma";
        var playerCode = "4ucmqplxb029";
        var currency = 2;

        var fakeService = Substitute.For<ICommandService<PlayerCreateCommand>>();
        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        await sut.PlayerCreateAsync(apiUrl, website, cert, agent, playerCode, currency);

        _ = fakeService
            .Received(1)
            .ExecuteAsync(
            command: Arg.Is<PlayerCreateCommand>(command => command.ApiUrl == apiUrl &&
            command.Website == website &&
            command.Cert == cert &&
            command.Agent == agent &&
            command.PlayerCode == playerCode &&
            command.Currency == currency),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
