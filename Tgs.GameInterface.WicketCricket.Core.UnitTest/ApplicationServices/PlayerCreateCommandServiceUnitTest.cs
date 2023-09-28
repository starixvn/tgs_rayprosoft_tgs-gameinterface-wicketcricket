using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class PlayerCreateCommandServiceUnitTest
{
    [Fact]
    public async Task Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerCreateCommand(
            ExchangeApiUrl: new Uri("https://test81.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            Agent: "testpginrma",
            PlayerCode: "4ue0ujotpmuu",
            Currency: 2);

        var client = new PlayerCreateCommandService(
            fakeGameApiClient);

        await client
            .ExecuteAsync(
             command);

        _ = fakeGameApiClient
            .Received(1)
            .GetKeyAsync(
            exchangeApiUrl: Arg.Is(command.ExchangeApiUrl),
            website: Arg.Is(command.Website),
            cert: Arg.Is(command.Cert),
            userId: Arg.Is(command.PlayerCode),
            userName: Arg.Is(command.PlayerCode),
            agent: Arg.Is(command.Agent),
            currency: Arg.Is(command.Currency),
            cancellationToken: Arg.Any<CancellationToken>());
    }
}
