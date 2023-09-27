using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class FundStatusGetService : IGetService<FundStatusQuery, bool>
{
    private readonly IGameApiClient _client;

    public FundStatusGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public ValueTask<bool> GetAsync(FundStatusQuery query, CancellationToken cancellationToken = default)
        => _client
        .CheckTransferRecordAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            userId: query.PlayerCode,
            tsCode: query.Txid,
            cancellationToken: cancellationToken);
}
