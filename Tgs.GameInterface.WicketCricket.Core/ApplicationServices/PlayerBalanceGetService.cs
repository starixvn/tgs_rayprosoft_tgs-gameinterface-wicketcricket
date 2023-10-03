using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerBalanceGetService : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    private readonly IGameApiClient _client;

    public PlayerBalanceGetService(IGameApiClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }

    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var resp = await _client
            .GetBalanceAsync(
            apiUrl: query.ApiUrl, website: query.Website, cert: query.Cert, userId: query.PlayerCode, cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult
        {
            IsPlayerExist = resp.IsPlayerExist,
            FreeBalance = resp.Balance,
            OutstandingBalance = resp.Exposure
        };
    }
}
