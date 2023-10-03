using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class FundInGetService : IGetService<FundInQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundInGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundInQuery query, CancellationToken cancellationToken = default)
    {
        var resp = await _client
            .WithdrawAsync(
            query.ApiUrl,
            query.Website,
            query.Cert,
            query.PlayerCode,
            query.Txid,
            query.Amount,
            cancellationToken)
            .ConfigureAwait(false);

        return new FundResult(resp.Resp);
    }
}
