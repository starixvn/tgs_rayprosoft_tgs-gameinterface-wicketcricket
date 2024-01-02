using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Core.ApplicationServices;

internal class PlayerBalanceGetService(IGameApiClient client) : IGetService<PlayerBalanceQuery, PlayerBalanceResult>
{
    public async ValueTask<PlayerBalanceResult> GetAsync(PlayerBalanceQuery query, CancellationToken cancellationToken = default)
    {
        var resp = await client
            .GetBalanceAsync(
            apiUrl: query.ApiUrl,
            website: query.Website,
            cert: query.Cert,
            userId: query.PlayerCode,
            cancellationToken: cancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceResult
        {
            IsPlayerExist = resp.IsPlayerExist,
            FreeBalance = resp.Balance,
            OutstandingBalance = resp.Exposure
        };
    }
}
