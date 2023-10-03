using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class FundOutGetService : IGetService<FundOutQuery, FundResult>
{
    private readonly IGameApiClient _client;

    public FundOutGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<FundResult> GetAsync(FundOutQuery query, CancellationToken cancellationToken = default)
    {
        var resp = await _client
            .DepositAsync(
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
