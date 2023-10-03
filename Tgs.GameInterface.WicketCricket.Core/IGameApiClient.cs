using Tgs.GameInterface.WicketCricket.Common;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core;

interface IGameApiClient
{
    ValueTask<bool> CheckTransferRecordAsync(
       Uri apiUrl,
       string website,
       string cert,
       string userId,
       string tsCode,
       CancellationToken cancellationToken = default);

    ValueTask<DepositTransferResp> DepositAsync(
       Uri apiUrl,
       string website,
       string cert,
       string userId,
       string tsCode,
       decimal amount,
       CancellationToken cancellationToken = default);

    ValueTask<BalanceResp> GetBalanceAsync(
       Uri apiUrl,
       string website,
       string cert,
       string userId,
       CancellationToken cancellationToken = default);

    IAsyncEnumerable<BalancesResp> GetBalancesAsync(
       Uri apiUrl,
       string website,
       string cert,
       CancellationToken cancellationToken = default);

    ValueTask<string> GetKeyAsync(
       Uri exchangeApiUrl,
       string website,
       string cert,
       string userId,
       string userName,
       string agent,
       int currency,
       CancellationToken cancellationToken = default);

    ValueTask<string> LoginAsync(
       Uri exchangeApiUrl,
       string website,
       string userId,
       string key,
       int eventType = 1,
       CancellationToken cancellationToken = default);

    ValueTask LogoutAllAsync(
       Uri exchangeApiUrl,
       string website,
       string cert,
       string agent,
       CancellationToken cancellationToken = default);

    ValueTask LogoutAsync(
       Uri exchangeApiUrl,
       string website,
       string cert,
       string userId,
       CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetTransferUnsettledResp> QueryCurrentBetsByLastUpdateDateAsync(
       Uri apiUrl,
       string website,
       string cert,
       DateTimeOffset lastUpdateTime,
       ReportTypes reportType,
       int pageNumber,
       CancellationToken cancellationToken = default);

    IAsyncEnumerable<BetTransferSettledResp> QuerySettleBetHistoryAsync(
       Uri apiUrl,
       string website,
       string cert,
       DateTimeOffset startTime,
       DateTimeOffset endTime,
       ReportTypes reportType,
       int pageNumber,
       CancellationToken cancellationToken = default);

    ValueTask<WithdrawTransferResp> WithdrawAsync(
       Uri apiUrl,
       string website,
       string cert,
       string userId,
       string tsCode,
       decimal amount,
       CancellationToken cancellationToken = default);
}
