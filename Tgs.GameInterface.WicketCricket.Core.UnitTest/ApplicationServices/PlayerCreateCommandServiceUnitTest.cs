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

        var service = new PlayerCreateCommandService(
            fakeGameApiClient);

        await service
            .ExecuteAsync(
             command);

        _ = fakeGameApiClient
            .Received(1)
            .GetKeyAsync(
             Arg.Is(command.ExchangeApiUrl),
             Arg.Is(command.Website),
             Arg.Is(command.Cert),
             Arg.Is(command.PlayerCode),
             Arg.Is(command.PlayerCode),
             Arg.Is(command.Agent),
             Arg.Is(command.Currency),
             Arg.Any<CancellationToken>());
    }
}
