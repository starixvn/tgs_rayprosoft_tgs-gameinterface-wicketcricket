using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameInterface.WicketCricket.Client;

public class GameInterfaceWicketCricketClient : IGameInterfaceWicketCricketClient
{
    private readonly GameInterfaceWicketCricket.GameInterfaceWicketCricketClient _client;

    public GameInterfaceWicketCricketClient(GameInterfaceWicketCricket.GameInterfaceWicketCricketClient client)
        => _client = client;

    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await _client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string website, string cert, string playerCode, string txid, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundInAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode,
                Txid = txid,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<FundReply> FundOutAsync(Uri apiUrl, string website, string cert, string playerCode, string txid, decimal amount, CancellationToken cancellationToken = default)
        => await _client.FundOutAsync(
            request: new FundRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode,
                Txid = txid,
                Amount = amount
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask<bool> FundStatusAsync(Uri apiUrl, string website, string cert, string playerCode, string txid, CancellationToken cancellationToken = default)
        => (await _client.FundStatusAsync(
            request: new FundStatusRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode,
                Txid = txid,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false))
        .Status;

    public async ValueTask<string> GameUrlAsync(Uri exchangeApiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default)
        => (await _client.GameUrlAsync(
            request: new GameUrlRequest
            {
                ExchangeApiUrl = exchangeApiUrl.ToString(),
                Website = website,
                Cert = cert,
                Agent = agent,
                PlayerCode = playerCode,
                Currency = currency,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false))
        .GameUrl;

    public IAsyncEnumerable<GameWagerInfo> GameWagerSettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset startTime, DateTimeOffset endTime, ReportTypes reportType, int pageNumber, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public IAsyncEnumerable<GameWagerInfo> GameWagerUnsettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset lastUpdateTime, ReportTypes reportType, int pageNumber, CancellationToken cancellationToken = default) => throw new NotImplementedException();

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default)
        => await _client.PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(Uri exchangeApiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default)
        => _ = await _client.PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ExchangeApiUrl = exchangeApiUrl.ToString(),
                Website = website,
                Cert = cert,
                Agent = agent,
                PlayerCode = playerCode,
                Currency = currency,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(Uri exchangeApiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default)
        => _ = await _client.PlayerLogoutAsync(
            request: new PlayerLogoutRequest
            {
                ExchangeApiUrl = exchangeApiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);
}
