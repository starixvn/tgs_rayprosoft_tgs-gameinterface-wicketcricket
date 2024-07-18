using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Web;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using Tgs.GameInterface.WicketCricket.Common;
using Tgs.GameInterface.WicketCricket.Core.Exceptions;
using Tgs.GameInterface.WicketCricket.Core.Extensions;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core;

internal class GameApiClient(
	ILogger<GameApiClient> logger,
	IOptions<PolicySettingsOptions> policySettingsOptions,
	HttpClient client) : IGameApiClient
{
	private readonly PolicySettingsOptions _policySettingsOptions = policySettingsOptions.Value;

	public async ValueTask<bool> CheckTransferRecordAsync(Uri apiUrl, string website, string cert, string userId, string tsCode, CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/checkTransferRecord");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["tsCodes"] = new JsonArray{
				new JsonObject
				{
					["userId"] = userId,
					["tsCode"] = tsCode,
				}
			}.ToJsonString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(CheckTransferRecordAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		var status = jsonNode["status"]!.GetValue<string>();
		if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new StatusCodeException(status);
		}

		var transfer = jsonNode["result"]![tsCode]!;

		return transfer["status"]!.GetValue<string>().Equals("1012", StringComparison.OrdinalIgnoreCase);
	}

	public async ValueTask<DepositTransferResp> DepositAsync(Uri apiUrl, string website, string cert, string userId, string tsCode, decimal amount, CancellationToken cancellationToken = default)
	{
		if (Math.Floor(amount * 100) / 100 < amount)
		{
			throw new DepositWithdrawalAmountException(amount);
		}

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/deposit");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userId"] = userId,
			["tsCode"] = tsCode,
			["balance"] = amount.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(DepositAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		return !jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
			? throw new StatusCodeException(jsonNode["status"]!.GetValue<string>())
			: new DepositTransferResp(jsonNode.ToJsonString());
	}

	public async ValueTask<BalanceResp> GetBalanceAsync(Uri apiUrl, string website, string cert, string userId, CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getBalance");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["alluser"] = 0.ToString(),
			["userIds"] = userId
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(GetBalanceAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		var status = jsonNode["status"]!.GetValue<string>();
		if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			return !status.Equals("1014", StringComparison.OrdinalIgnoreCase)
				? throw new StatusCodeException(status)
				: new BalanceResp(false, 0.0m, 0.0m);
		}

		var result = jsonNode["results"]!.AsArray().Single(x => x!["userId"]!.GetValue<string>().Equals(userId, StringComparison.InvariantCultureIgnoreCase))!;

		return new BalanceResp(true, result["balance"]!.GetValue<decimal>(), -result["exposure"]!.GetValue<decimal>());
	}

	public async IAsyncEnumerable<BalancesResp> GetBalancesAsync(Uri apiUrl, string website, string cert, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getBalance");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["alluser"] = 1.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(GetBalancesAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		var status = jsonNode["status"]!.GetValue<string>();
		if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new StatusCodeException(status);
		}

		var results = jsonNode["results"]!.AsArray();

		if (results.Count == 0)
		{
			yield break;
		}

		foreach (var result in results)
		{
			if (result is null)
			{
				continue;
			}

			yield return new BalancesResp(
				UserId: result["userId"]!.GetValue<string>(),
				Balance: result["balance"]!.GetValue<decimal>(),
				Exposure: -result["exposure"]!.GetValue<decimal>());
		}
	}

	public async ValueTask<string> GetKeyAsync(Uri apiUrl, string website, string cert, string userId, string userName, string agent, int currency, CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/getKey");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userId"] = userId,
			["userName"] = userId,
			["agent"] = agent,
			["currency"] = currency.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(GetKeyAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		return !jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.InvariantCultureIgnoreCase)
			? throw new StatusCodeException(jsonNode["status"]!.GetValue<string>()) :
			jsonNode["key"]!.GetValue<string>();
	}

	public ValueTask<string> LoginAsync(Uri exchangeApiUrl, string website, string userId, string key, int eventType = 1, CancellationToken cancellationToken = default)
	{
		var path = new Uri(exchangeApiUrl, $"/apiWallet/player/{website}/login");
		var builder = new UriBuilder(path);
		var query = HttpUtility.ParseQueryString(builder.Query);
		query["userId"] = userId;
		query["key"] = key;
		query["eventType"] = eventType.ToString();
		builder.Query = query.ToString();
		return ValueTask.FromResult(builder.ToString());
	}

	public async ValueTask LogoutAllAsync(Uri exchangeApiUrl, string website, string cert, string agent, CancellationToken cancellationToken = default)
	{
		var url = new Uri(exchangeApiUrl, $"/apiWallet/player/{website}/logoutAll");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["agentId"] = agent,
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(LogoutAllAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		if (!jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.InvariantCultureIgnoreCase))
		{
			throw new StatusCodeException(jsonNode["status"]!.GetValue<string>());
		}
	}

	public async ValueTask LogoutAsync(Uri exchangeApiUrl, string website, string cert, string userId, CancellationToken cancellationToken = default)
	{
		var url = new Uri(exchangeApiUrl, $"/apiWallet/player/{website}/logout");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userIds"] = userId,
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(LogoutAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		if (!jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.InvariantCultureIgnoreCase))
		{
			throw new StatusCodeException(jsonNode["status"]!.GetValue<string>());
		}
	}

	public async IAsyncEnumerable<BetTransferUnsettledResp> QueryCurrentBetsByLastUpdateDateAsync(Uri apiUrl, string website, string cert, DateTimeOffset lastUpdateTime, ReportTypes reportType, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryCurrentBetsByLastUpdateDate");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["lastUpdateDate"] = lastUpdateTime.ConvertToUnixTimeMilliseconds().ToString(),
			["reportType"] = ((int)reportType).ToString(),
			["pageNumber"] = pageNumber.ToString(),
			["timeZone"] = 1.ToString(),
			["isDateISO8601"] = 1.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(QueryCurrentBetsByLastUpdateDateAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		var status = jsonNode["status"]!.ToString();
		if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new StatusCodeException(status);
		}

		if (jsonNode["totalPage"]!.GetValue<int>() < pageNumber)
		{
			yield break;
		}

		var resultList = jsonNode["matched"]!["resultList"]!.AsArray()!;

		foreach (var recordNode in jsonNode["matched"]![propertyName: "resultList"]!.AsArray()!)
		{
			if (recordNode is JsonObject record)
				yield return new BetTransferUnsettledResp(
					EventId: record["eventId"]!.GetValue<long>(),
					EventType: record["eventType"]!.GetValue<int>(),
					CurrencyType: record["currencyType"]!.GetValue<int>(),
					CategoryType: record["categoryType"]!.GetValue<int>(),
					BetId: record["betId"]!.GetValue<long>(),
					UserId: record["userId"]!.GetValue<string>(),
					AgentId: record["agentId"]!.GetValue<string>(),
					BetPlaced: record["betPlaced"]!.GetValue<DateTimeOffset>(),
					MatchAmount: record["matchAmount"]!.GetValue<decimal>(),
					UnMatchAmount: record["unMatchAmount"]!.GetValue<decimal>(),
					DateMatched: record["dateMatched"]!.GetValue<DateTimeOffset>(),
					AdditionalData: record.ToJsonString()
					);
		}
	}

	public async ValueTask<QueryDomainResp> QueryDomainAsync(Uri apiUrl, string website, string cert, CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/queryDomain");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(QueryDomainAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		if (!jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new StatusCodeException(jsonNode["status"]!.GetValue<string>());
		}

		var privateDomains = new List<QueryDomain>();

		foreach (var privateDomain in jsonNode["privateDomains"]!.AsArray())
		{
			if (privateDomain is null)
			{
				continue;
			}

			privateDomains.Add(new QueryDomain(
				Domain: privateDomain["domain"]!.ToString(),
				Priority: privateDomain["priority"]!.GetValue<int>()));
		}

		var domains = new List<QueryDomain>();

		foreach (var domain in jsonNode["domains"]!.AsArray())
		{
			if (domain is null)
			{
				continue;
			}

			domains.Add(new QueryDomain(
				Domain: domain["domain"]!.ToString(),
				Priority: domain["priority"]!.GetValue<int>()));
		}

		return new QueryDomainResp(
			PrivateDomains: privateDomains,
			Domains: domains);
	}

	public async IAsyncEnumerable<BetTransferSettledResp> QuerySettleBetHistoryAsync(Uri apiUrl, string website, string cert, DateTimeOffset startTime, DateTimeOffset endTime, ReportTypes reportType, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken = default)
	{
		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/querySettleBetHistory");
		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["startDate"] = startTime.ConvertToString(),
			["endDate"] = endTime.ConvertToString(),
			["reportType"] = ((int)reportType).ToString(),
			["pageNumber"] = pageNumber.ToString(),
			["timeZone"] = 1.ToString(),
			["isDateISO8601"] = 1.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(QuerySettleBetHistoryAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		var status = jsonNode["status"]!.ToString();
		if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
		{
			throw new StatusCodeException(status);
		}

		if (jsonNode["totalPage"]!.GetValue<int>() < pageNumber)
		{
			yield break;
		}

		foreach (var recordNode in jsonNode["resultList"]!.AsArray())
		{
			if (recordNode is JsonObject record)
				yield return new BetTransferSettledResp(
					EventId: record!["eventId"]!.GetValue<long>(),
					EventType: record["eventType"]!.GetValue<int>(),
					CurrencyType: record["currencyType"]!.GetValue<int>(),
					CategoryType: record["categoryType"]!.GetValue<int>(),
					BetId: record["betId"]!.GetValue<long>(),
					UserId: record["userId"]!.GetValue<string>(),
					AgentId: record["agentId"]!.GetValue<string>(),
					BetPlaced: record["betPlaced"]!.GetValue<DateTimeOffset>(),
					MatchSettledDate: record["matchSettledDate"]!.GetValue<DateTimeOffset>(),
					MarketSettledDate: record["marketSettledDate"]!.GetValue<DateTimeOffset>(),
					BetStatus: record["betStatus"]!.GetValue<string>(),
					MatchStake: record["matchStake"]!.GetValue<decimal>(),
					ProfitLoss: record["profitLoss"]!.GetValue<decimal>(),
					AdditionalData: record.ToJsonString());
		}
	}

	public async ValueTask<WithdrawTransferResp> WithdrawAsync(Uri apiUrl, string website, string cert, string userId, string tsCode, decimal amount, CancellationToken cancellationToken = default)
	{
		if (Math.Floor(amount * 100) / 100 < amount)
		{
			throw new DepositWithdrawalAmountException(amount);
		}

		var url = new Uri(apiUrl, $"/api/apiWallet/{website}/withdraw");

		var parameters = new Dictionary<string, string>
		{
			["cert"] = cert,
			["userId"] = userId,
			["withdrawtype"] = 0.ToString(),
			["tsCode"] = tsCode,
			["balance"] = amount.ToString()
		};

		var jsonNode = await PolicyAsync(
			apiUrl: url,
			parameters: parameters,
			logPrefix: nameof(WithdrawAsync),
			cancellationToken: cancellationToken).AsTask()
			.ConfigureAwait(false);

		return !jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
			? throw new StatusCodeException(jsonNode["status"]!.GetValue<string>())
			: new WithdrawTransferResp(jsonNode.ToJsonString());
	}

	private async ValueTask<JsonNode> PolicyAsync(Uri apiUrl, IDictionary<string, string> parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		var retryPolicy = Policy
			.HandleResult<JsonNode>(jsonNode => jsonNode["status"]!.ToString().Equals("1010", StringComparison.OrdinalIgnoreCase))
			.WaitAndRetryAsync(
			retryCount: _policySettingsOptions.GlobalRetryCount,
			sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(_policySettingsOptions.GlobalRetryWaitSec, retryAttempt)),
			onRetry: (result, retryTime, retryCount, context) => logger.LogWarning("{logWarning} ",
					new
					{
						LogOn = DateTime.Now.ToString("u"),
						Parameters = JsonSerializer.Serialize(parameters),
						RetryCount = retryCount,
						RetryTime = retryTime,
						Result = JsonSerializer.Serialize(result)
					}));

		return await retryPolicy
			.ExecuteAsync(() => PostAsync(
				apiUrl: apiUrl,
				parameters: parameters,
				logPrefix: logPrefix,
				cancellationToken: cancellationToken)
			.AsTask())
			.ConfigureAwait(false);
	}

	private async ValueTask<JsonNode> PostAsync(Uri apiUrl, IDictionary<string, string> parameters, string logPrefix, CancellationToken cancellationToken = default)
	{
		var sw = Stopwatch.StartNew();

		var content = new FormUrlEncodedContent(parameters);
		var httpResponseMessage = await client
			.PostAsync(
			requestUri: apiUrl,
			content: content,
			cancellationToken: cancellationToken)
			.ConfigureAwait(false);

		sw.Stop();

		var responseMessage = default(string);

		using (var sr = new StreamReader(httpResponseMessage.Content.ReadAsStreamAsync(cancellationToken).Result))
		{
			responseMessage = await sr.ReadToEndAsync(cancellationToken).ConfigureAwait(false) ?? string.Empty;
		}

		var apiLogEvent = new
		{
			Path = httpResponseMessage.RequestMessage?.RequestUri?.AbsoluteUri.NoLongerThan(512) ?? string.Empty,
			Method = httpResponseMessage.RequestMessage?.Method.ToString().NoLongerThan(8) ?? string.Empty,
			Prefix = logPrefix,
			ServerName = Environment.MachineName.NoLongerThan(128),
			RequestHeaders = httpResponseMessage.RequestMessage?.Headers + httpResponseMessage.RequestMessage?.Content?.Headers?.ToString(),
			RequestContent = (await content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false)).NoLongerThan(4096),
			ResponseHeaders = httpResponseMessage.Headers.ToString(),
			ResponseContent = responseMessage.NoLongerThan(4096),
			ResponseCode = (short)httpResponseMessage.StatusCode,
			ExecutionTimeMs = (int)sw.ElapsedMilliseconds,
			RecordedOn = DateTimeOffset.UtcNow
		};
		logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

		_ = httpResponseMessage.EnsureSuccessStatusCode();

		return JsonNode.Parse(responseMessage)!;
	}
}
