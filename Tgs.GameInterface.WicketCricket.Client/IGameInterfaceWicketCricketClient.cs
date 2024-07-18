using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameInterface.WicketCricket.Client;

public interface IGameInterfaceWicketCricketClient
{
	ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundInAsync(Uri apiUrl, string website, string cert, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<FundReply> FundOutAsync(Uri apiUrl, string website, string cert, string playerCode, string orderId, decimal amount, CancellationToken cancellationToken = default);

	ValueTask<bool> FundStatusAsync(Uri apiUrl, string website, string cert, string playerCode, string orderId, CancellationToken cancellationToken = default);

	ValueTask<string> GameUrlAsync(Uri apiUrl, Uri exchangeApiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordSettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset startTime, DateTimeOffset endTime, ReportTypes reportType, int pageNumber, CancellationToken cancellationToken = default);

	IAsyncEnumerable<GameRecordInfo> GameRecordUnsettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset lastUpdateTime, ReportTypes reportType, int pageNumber, CancellationToken cancellationToken = default);

	ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default);

	ValueTask PlayerCreateAsync(Uri apiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default);

	ValueTask PlayerLogoutAsync(Uri apiUrl, Uri exchangeApiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default);
}
