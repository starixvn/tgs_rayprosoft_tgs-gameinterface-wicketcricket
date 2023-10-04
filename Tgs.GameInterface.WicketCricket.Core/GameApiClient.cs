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

internal class GameApiClient : IGameApiClient
{
    private readonly HttpClient _client;
    private readonly ILogger<GameApiClient> _logger;
    private readonly PolicySettingsOptions _policySettings;

    public GameApiClient(
        ILogger<GameApiClient> logger,
        IOptions<PolicySettingsOptions> options,
        HttpClient client)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _policySettings = options.Value;
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

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
            logPrefix: "checkTransferRecord",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
        if ((Math.Floor(amount * 100) / 100) < amount)
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
            logPrefix: "deposit",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
            logPrefix: "getBalance",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
            logPrefix: "getBalance",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
            cancellationToken: cancellationToken).AsTask()
            .ConfigureAwait(false);

        var status = jsonNode["status"]!.GetValue<string>();
        if (!status.Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            throw new StatusCodeException(status);
        }

        var results = jsonNode["results"]!.AsArray();

        if (!results.Any())
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
            logPrefix: "getKey",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
            logPrefix: "logoutAll",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
            logPrefix: "logout",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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
            logPrefix: "queryCurrentBetsByLastUpdateDate",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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

        if (!resultList.Any())
        {
            yield break;
        }

        foreach (var result in resultList)
        {
            if (result is null)
            {
                continue;
            }
            yield return new BetTransferUnsettledResp(
                EventId: result["eventId"]!.GetValue<long>(),
                EventType: result["eventType"]!.GetValue<int>(),
                CurrencyType: result["currencyType"]!.GetValue<int>(),
                CategoryType: result["categoryType"]!.GetValue<int>(),
                BetId: result["betId"]!.GetValue<long>(),
                UserId: result["userId"]!.GetValue<string>(),
                AgentId: result["agentId"]!.GetValue<string>(),
                BetPlaced: result["betPlaced"]!.GetValue<DateTimeOffset>(),
                MatchAmount: result["matchAmount"]!.GetValue<decimal>(),
                UnMatchAmount: result["unMatchAmount"]!.GetValue<decimal>(),
                DateMatched: result["dateMatched"]!.GetValue<DateTimeOffset>(),
                AdditionalData: result.ToJsonString()
                );
        }
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
            logPrefix: "querySettleBetHistory",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
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

        var resultList = jsonNode["resultList"]!.AsArray()!;

        if (!resultList.Any())
        {
            yield break;
        }

        foreach (var result in resultList)
        {
            yield return new BetTransferSettledResp(
                EventId: result!["eventId"]!.GetValue<long>(),
                EventType: result["eventType"]!.GetValue<int>(),
                CurrencyType: result["currencyType"]!.GetValue<int>(),
                CategoryType: result["categoryType"]!.GetValue<int>(),
                BetId: result["betId"]!.GetValue<long>(),
                UserId: result["userId"]!.GetValue<string>(),
                AgentId: result["agentId"]!.GetValue<string>(),
                BetPlaced: result["betPlaced"]!.GetValue<DateTimeOffset>(),
                MatchSettledDate: result["matchSettledDate"]!.GetValue<DateTimeOffset>(),
                MarketSettledDate: result["marketSettledDate"]!.GetValue<DateTimeOffset>(),
                BetStatus: result["betStatus"]!.GetValue<string>(),
                MatchStake: result["matchStake"]!.GetValue<decimal>(),
                ProfitLoss: result["profitLoss"]!.GetValue<decimal>(),
                AdditionalData: result.ToJsonString());
        }
    }

    public async ValueTask<WithdrawTransferResp> WithdrawAsync(Uri apiUrl, string website, string cert, string userId, string tsCode, decimal amount, CancellationToken cancellationToken = default)
    {
        if ((Math.Floor(amount * 100) / 100) < amount)
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
            logPrefix: "withdraw",
            retryCount: _policySettings.RetryCount,
            retryWaitSec: _policySettings.RetryWaitSec,
            cancellationToken: cancellationToken).AsTask()
            .ConfigureAwait(false);

        return !jsonNode["status"]!.GetValue<string>().Equals("1", StringComparison.OrdinalIgnoreCase)
            ? throw new StatusCodeException(jsonNode["status"]!.GetValue<string>())
            : new WithdrawTransferResp(jsonNode.ToJsonString());
    }

    private async ValueTask<JsonNode> PolicyAsync(Uri apiUrl, IDictionary<string, string> parameters, string logPrefix, int retryCount, int retryWaitSec, CancellationToken cancellationToken = default)
    {
        var retryPolicy = Policy
            .HandleResult<JsonNode>(jsonNode => jsonNode["status"]!.ToString().Equals("1010", StringComparison.OrdinalIgnoreCase))
            .WaitAndRetryAsync(
            retryCount: retryCount,
            sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(retryWaitSec),
            onRetry: (result, retryTime, context) =>
            {
                _logger.LogWarning("LogOn:{logOn}  Parameter:{parameters} Result:{result} Delay:{retryTime}",
                    DateTime.Now.ToString("o"),
                    parameters,
                    result,
                    retryTime);
            });

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
        logPrefix = $"{nameof(GameApiClient)}.{logPrefix}".ToLower();

        var sw = Stopwatch.StartNew();

        var content = new FormUrlEncodedContent(parameters);
        var httpResponseMessage = await _client
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

        var apiLogEvent = new GameApiLogEvent
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
        _logger.LogInformation("{apiLogEvent}", JsonSerializer.Serialize(apiLogEvent));

        _ = httpResponseMessage.EnsureSuccessStatusCode();

        return JsonNode.Parse(responseMessage)!;
    }
}
