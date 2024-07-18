using Grpc.Net.Client;

using Microsoft.Extensions.DependencyInjection;

using NSubstitute;

using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.IntegrationTest;

public class GameInterfaceWicketCricketClient_PlayerLogoutTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var apiUrl = new Uri("https://apiinfo.cckk77.net/");
        var exchangeApiUrl = new Uri("https://test81.cckk77.net/");
        var website = "WICKETSPRO";
        var cert = "wkVwQ8sMVr252EA9";
        var playerCode = "4ucmqplxb029";

        var fakeService = Substitute.For<ICommandService<PlayerLogoutCommand>>();
        var web = new GameInterfaceWicketCricketApplication(builder => _ = builder.ConfigureServices(services =>
        {
            _ = services.AddTransient(_ => fakeService);
        }));

        var sut = new Client.GameInterfaceWicketCricketClient(new Protos.GameInterfaceWicketCricket.GameInterfaceWicketCricketClient(
            GrpcChannel.ForAddress(
                address: web.Server.BaseAddress,
                channelOptions: new GrpcChannelOptions { HttpHandler = web.Server.CreateHandler() })));

        await sut.PlayerLogoutAsync(apiUrl, exchangeApiUrl, website, cert, playerCode);

        _ = fakeService
            .Received(1)
            .ExecuteAsync(
            command: Arg.Is<PlayerLogoutCommand>(
                command => command.ApiUrl == apiUrl &&
                command.ExchangeApiUrl == exchangeApiUrl &&
                command.Website == website &&
                command.Cert == cert &&
                command.PlayerCode == playerCode),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
