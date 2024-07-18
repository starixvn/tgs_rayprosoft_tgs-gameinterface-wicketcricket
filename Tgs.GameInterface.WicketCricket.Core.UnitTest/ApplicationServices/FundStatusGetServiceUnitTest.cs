using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundStatusGetServiceUnitTest
{
	[Fact]
	public async Task FundStatus_Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundStatusQuery
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			PlayerCode = "4ue0ujotpmuu",
			OrderId = "1695630056"
		};

		var apiUrl = new Uri("https://apiinfo.ppkk77.net/");
		_ = fakeGameDomain.GetApiUrlDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(apiUrl);

		_ = fakeGameApiClient.CheckTransferRecordAsync(
			apiUrl: Arg.Is(apiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			userId: Arg.Is(query.PlayerCode),
			tsCode: Arg.Is(query.OrderId),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(true);

		var service = new FundStatusGetService(
			fakeGameDomain,
			fakeGameApiClient);

		var actual = await service
			.GetAsync(
			query);

		Assert.True(actual);
	}
}
