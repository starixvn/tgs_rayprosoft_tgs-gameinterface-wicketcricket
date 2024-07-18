using System.Text.Json.Nodes;
using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class FundOutGetServiceUnitTest
{
	[Fact]
	public async Task FundOut_Normal_Process()
	{
		var fakeGameDomain = Substitute.For<IGameDomain>();
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new FundOutQuery
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9",
			PlayerCode = "4ue0ujotpmuu",
			OrderId = "1695630056",
			Amount = 1000.00m
		};

		var apiUrl = new Uri("https://apiinfo.ppkk77.net/");
		_ = fakeGameDomain.GetApiUrlDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(apiUrl);

		var result = new DepositTransferResp(
			Resp: new JsonObject
			{
				["status"] = "1"
			}.ToJsonString());
		_ = fakeGameApiClient.DepositAsync(
			apiUrl: Arg.Is(apiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			userId: Arg.Is(query.PlayerCode),
			tsCode: Arg.Is(query.OrderId),
			amount: Arg.Is(query.Amount),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(result);

		var service = new FundOutGetService(
			fakeGameDomain,
			fakeGameApiClient);

		var actual = await service
			.GetAsync(
			query);

		Assert.NotNull(actual);
		Assert.Equal(result.Resp, actual.Resp);
	}
}
