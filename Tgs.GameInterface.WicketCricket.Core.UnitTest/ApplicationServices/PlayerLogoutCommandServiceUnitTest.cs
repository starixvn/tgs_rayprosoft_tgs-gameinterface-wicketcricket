using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class PlayerLogoutCommandServiceUnitTest
{
    [Fact]
    public async Task PlayerLogout_Normal_Process()
    {
        var fakeGameApiClient = Substitute.For<IGameApiClient>();

        var command = new PlayerLogoutCommand(
            ExchangeApiUrl: new Uri("https://test81.cckk77.net/"),
            Website: "WICKETSPRO",
            Cert: "wkVwQ8sMVr252EA9",
            PlayerCode: "4ue0ujotpmuu");

        var service = new PlayerLogoutCommandService(
            fakeGameApiClient);

        await service
            .ExecuteAsync(
             command);

        _ = fakeGameApiClient
            .Received(1)
            .LogoutAsync(
            exchangeApiUrl: Arg.Is(command.ExchangeApiUrl), website: Arg.Is(command.Website), cert: Arg.Is(command.Cert), userId: Arg.Is(command.PlayerCode), cancellationToken: Arg.Any<CancellationToken>());
    }
}
