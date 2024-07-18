using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core;
internal class GameDomain(
	IMemoryCache memoryCache,
	IGameApiClient client) : IGameDomain
{
	private readonly SemaphoreSlim _semaphore = new(1, 1);
	public async ValueTask<Uri> GetApiUrlDomainAsync(Uri apiUrl, string website, string cert, int priority = 1, CancellationToken cancellationToken = default)
	{
		var key = string.Concat(typeof(GameDomain).Namespace!.Replace(".", "-").ToLower(), ":", nameof(GetApiUrlDomainAsync), ":", website);
		if (!memoryCache.TryGetValue(key, out QueryDomainResp? resp))
		{
			await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
			try
			{
				if (!memoryCache.TryGetValue(key, out resp))
				{

					resp = await client.QueryDomainAsync(
					apiUrl: apiUrl,
					website: website,
					cert: cert,
					cancellationToken: cancellationToken)
					.ConfigureAwait(false)
					?? throw new ArgumentNullException(website);

					memoryCache.Set(key, resp, TimeSpan.FromHours(24));
				}
			}
			finally
			{
				_semaphore.Release();
			}
		}

		var secondLevelDomain = GetSecondLevelDomain(apiUrl.Host);
		var replaceDomain = resp!.Domains.Single(x => x.Priority == priority);
		return new Uri(apiUrl.ToString().Replace(secondLevelDomain, replaceDomain.Domain));
	}

	public async ValueTask<Uri> GetExchangeApiUrlDomainAsync(Uri apiUrl, Uri exchangeApiUrl, string website, string cert, int priority = 1, CancellationToken cancellationToken = default)
	{
		var key = string.Concat(typeof(GameDomain).Namespace!.Replace(".", "-").ToLower(), ":", nameof(GetExchangeApiUrlDomainAsync), ":", website);
		if (!memoryCache.TryGetValue(key, out QueryDomainResp? resp))
		{
			await _semaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
			try
			{
				if (!memoryCache.TryGetValue(key, out resp))
				{

					resp = await client.QueryDomainAsync(
					apiUrl: apiUrl,
					website: website,
					cert: cert,
					cancellationToken: cancellationToken)
					.ConfigureAwait(false)
					?? throw new ArgumentNullException(website);

					memoryCache.Set(key, resp, TimeSpan.FromHours(24));
				}
			}
			finally
			{
				_semaphore.Release();
			}
		}

		var secondLevelDomain = GetSecondLevelDomain(exchangeApiUrl.Host);
		var replaceDomain = resp!.Domains.Where(x => x.Priority == priority).Single();
		return new Uri(exchangeApiUrl.ToString().Replace(secondLevelDomain, replaceDomain.Domain));
	}

	private static string GetSecondLevelDomain(string domain)
	{
		string pattern = @"([a-zA-Z0-9\-]+)\.([a-zA-Z0-9\-]+)$";

		Regex regex = new Regex(pattern);
		Match match = regex.Match(domain);

		if (match.Success && match.Groups.Count == 3)
		{
			return match.ToString();
		}
		else
		{
			// Handle invalid domain format or no match
			throw new ArgumentException("Invalid domain format or no second-level domain found.");
		}
	}
}
