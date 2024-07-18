namespace Tgs.GameInterface.WicketCricket.Core.Models;
public record QueryDomainResp(
	IEnumerable<QueryDomain> PrivateDomains,
	IEnumerable<QueryDomain> Domains);
