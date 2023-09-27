using Tgs.GameInterface.WicketCricket.Common;
using Tgs.GameInterface.WicketCricket.Core.Models;

namespace Tgs.GameInterface.WicketCricket.Core;

public interface IGameApiClient
{
    public ValueTask<bool> CheckTransferRecordAsync(
        Uri apiUrl,
        string website,
        string cert,
        string userId,
        string tsCode,
        CancellationToken cancellationToken = default);

    public ValueTask<DepositTransferResp> DepositAsync(
        Uri apiUrl,
        string website,
        string cert,
        string userId,
        string tsCode,
        decimal amount,
        CancellationToken cancellationToken = default);

    public ValueTask<BalanceResp> GetBalanceAsync(
        Uri apiUrl,
        string website,
        string cert,
        string userId,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<BalancesResp> GetBalancesAsync(
        Uri apiUrl,
        string website,
        string cert,
        CancellationToken cancellationToken = default);

    public ValueTask<string> GetKeyAsync(
        Uri exchangeApiUrl,
        string website,
        string cert,
        string userId,
        string userName,
        string agent,
        int currency,
        CancellationToken cancellationToken = default);

    public ValueTask<string> LoginAsync(
        Uri exchangeApiUrl,
        string website,
        string userId,
        string key,
        int eventType = 1,
        CancellationToken cancellationToken = default);

    public ValueTask LogoutAllAsync(
        Uri exchangeApiUrl,
        string website,
        string cert,
        string agent,
        CancellationToken cancellationToken = default);

    public ValueTask LogoutAsync(
        Uri exchangeApiUrl,
        string website,
        string cert,
        string userId,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<BetTransferUnsettledResp> QueryCurrentBetsByLastUpdateDateAsync(
        Uri apiUrl,
        string website,
        string cert,
        DateTimeOffset lastUpdateTime,
        ReportTypes reportType,
        int pageNumber,
        CancellationToken cancellationToken = default);

    public IAsyncEnumerable<BetTransferSettledResp> QuerySettleBetHistoryAsync(
        Uri apiUrl,
        string website,
        string cert,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        ReportTypes reportType,
        int pageNumber,
        CancellationToken cancellationToken = default);

    public ValueTask<WithdrawTransferResp> WithdrawAsync(
        Uri apiUrl,
        string website,
        string cert,
        string userId,
        string tsCode,
        decimal amount,
        CancellationToken cancellationToken = default);
}
