using NSubstitute;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.UnitTest.ApplicationServices;

public class QueryDomainGetServiceUnitTest
{
	[Fact]
	public async Task Normal_Process()
	{
		var fakeGameApiClient = Substitute.For<IGameApiClient>();

		var query = new QueryDomainQuery
		{
			ApiUrl = new Uri("https://apiinfo.cckk77.net/"),
			Website = "WICKETSPRO",
			Cert = "wkVwQ8sMVr252EA9"
		};

		var resp = new QueryDomainResp(
			PrivateDomains: [new(Domain: "cckk77.net", Priority: 1)],
			Domains: [new(Domain: "ppkk77.net", Priority: 1)]);
		_ = fakeGameApiClient.QueryDomainAsync(
			apiUrl: Arg.Is(query.ApiUrl),
			website: Arg.Is(query.Website),
			cert: Arg.Is(query.Cert),
			cancellationToken: Arg.Any<CancellationToken>())
			.Returns(resp);

		var service = new QueryDomainGetService(
			fakeGameApiClient);

		var actual = await service.GetAsync(query);

		Assert.NotNull(actual);
		Assert.Equal("cckk77.net", actual.PrivateDomains.First().Domain);
		Assert.Equal("ppkk77.net", actual.Domains.First().Domain);
	}
}
