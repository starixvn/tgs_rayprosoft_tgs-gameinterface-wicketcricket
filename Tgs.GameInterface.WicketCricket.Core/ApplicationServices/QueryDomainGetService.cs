using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class QueryDomainGetService(IGameApiClient client) : IGetService<QueryDomainQuery, QueryDomainResult>
{
	public async ValueTask<QueryDomainResult> GetAsync(QueryDomainQuery query, CancellationToken cancellationToken = default)
	{
		var resp = await client.QueryDomainAsync(
			apiUrl: query.ApiUrl,
			website: query.Website,
			cert: query.Cert,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		return new QueryDomainResult(
			PrivateDomains: resp.PrivateDomains,
			Domains: resp.Domains);
	}
}
