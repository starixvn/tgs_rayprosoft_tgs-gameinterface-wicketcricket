using System.Runtime.CompilerServices;
using Google.Protobuf.WellKnownTypes;
using Tgs.GameInterface.WicketCricket.Protos;

namespace Tgs.GameInterface.WicketCricket.Client;

public class GameInterfaceWicketCricketClient(
    GameInterfaceWicketCricket.GameInterfaceWicketCricketClient client) : IGameInterfaceWicketCricketClient
{
    public async ValueTask<string> CheckingServiceAsync(string machineName, CancellationToken cancellationToken = default)
    {
        var request = new CheckingServiceRequest
        {
            MachineName = machineName,
        };

        var reply = await client
            .CheckingServiceAsync(
                request: request,
                cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return reply.MachineName;
    }

    public async ValueTask<FundReply> FundInAsync(Uri apiUrl, string website, string cert, string playerCode, string txid, decimal amount, CancellationToken cancellationToken = default)
        => await client.FundInAsync(
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
        => await client.FundOutAsync(
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
        => (await client.FundStatusAsync(
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

    public async ValueTask<string> GameUrlAsync(Uri apiUrl, Uri exchangeApiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default)
        => (await client.GameUrlAsync(
            request: new GameUrlRequest
            {
                ApiUrl = apiUrl.ToString(),
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

    public async IAsyncEnumerable<GameWagerInfo> GameWagerSettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset startTime, DateTimeOffset endTime, ReportTypes reportType, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = await client.GameWagerSettledAsync(
            request: new GameWagerSettledRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                StartTime = startTime.ToTimestamp(),
                EndTime = endTime.ToTimestamp(),
                ReportType = reportType,
            },
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!(reply.Items ?? Enumerable.Empty<GameWagerInfo>()).Any())
        {
            yield break;
        }

        foreach (var item in reply.Items ?? Enumerable.Empty<GameWagerInfo>())
        {
            yield return item;
        }
    }

    public async IAsyncEnumerable<GameWagerInfo> GameWagerUnsettledAsync(Uri apiUrl, string website, string cert, DateTimeOffset lastUpdateTime, ReportTypes reportType, int pageNumber, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var reply = await client.GameWagerUnsettledAsync(
            request: new GameWagerUnsettledRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                LastUpdateTime = lastUpdateTime.ToTimestamp(),
                ReportType = reportType,
            },
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        if (!(reply.Items ?? Enumerable.Empty<GameWagerInfo>()).Any())
        {
            yield break;
        }

        foreach (var item in reply.Items ?? Enumerable.Empty<GameWagerInfo>())
        {
            yield return item;
        }
    }

    public async ValueTask<PlayerBalanceReply> PlayerBalanceAsync(Uri apiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default)
        => await client.PlayerBalanceAsync(
            request: new PlayerBalanceRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                PlayerCode = playerCode
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerCreateAsync(Uri apiUrl, string website, string cert, string agent, string playerCode, int currency, CancellationToken cancellationToken = default)
        => _ = await client.PlayerCreateAsync(
            request: new PlayerCreateRequest
            {
                ApiUrl = apiUrl.ToString(),
                Website = website,
                Cert = cert,
                Agent = agent,
                PlayerCode = playerCode,
                Currency = currency,
            },
            cancellationToken: cancellationToken)
        .ConfigureAwait(false);

    public async ValueTask PlayerLogoutAsync(Uri exchangeApiUrl, string website, string cert, string playerCode, CancellationToken cancellationToken = default)
        => _ = await client.PlayerLogoutAsync(
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
