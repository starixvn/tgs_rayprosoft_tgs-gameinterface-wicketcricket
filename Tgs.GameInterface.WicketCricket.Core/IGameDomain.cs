namespace Tgs.GameInterface.WicketCricket.Core;

public interface IGameDomain
{
	public ValueTask<Uri> GetApiUrlDomainAsync(Uri apiUrl, string website, string cert, int priority = 1, CancellationToken cancellationToken = default);

	public ValueTask<Uri> GetExchangeApiUrlDomainAsync(Uri apiUrl, Uri exchangeApiUrl, string website, string cert, int priority = 1, CancellationToken cancellationToken = default);
}
