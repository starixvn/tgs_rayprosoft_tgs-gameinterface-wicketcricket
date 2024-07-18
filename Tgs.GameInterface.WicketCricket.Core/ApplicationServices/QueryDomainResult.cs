using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
public record QueryDomainResult(
	IEnumerable<QueryDomain> PrivateDomains,
	IEnumerable<QueryDomain> Domains);
