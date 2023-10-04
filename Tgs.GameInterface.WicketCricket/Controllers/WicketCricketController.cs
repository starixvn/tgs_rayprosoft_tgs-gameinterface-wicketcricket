using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.ViewModels;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.Controllers;

[Route("[controller]")]
[ApiController]
public class WicketCricketController : ControllerBase
{
    /// <summary>
    /// Funds the in asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "playerCode": "4ue0ujotpmuu",
    ///             "txid": "1695696162",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/in", Name = "FundIn")]
    public async ValueTask<FundResultViewModel> FundInAsync(
        [FromServices] IGetService<FundInQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundInQuery(
            source.ApiUrl,
            source.Website,
            source.Cert,
            source.PlayerCode,
            source.Txid,
            source.Amount);

        var result = await service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            result.Resp);
    }

    /// <summary>
    /// Funds the out asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "playerCode": "4ue0ujotpmuu",
    ///             "txid": "1695696155",
    ///             "amount": 1
    ///         }
    /// </remarks>
    [HttpPost("fund/out", Name = "FundOut")]
    public async ValueTask<FundResultViewModel> FundOutAsync(
        [FromServices] IGetService<FundOutQuery, FundResult> service,
        FundViewModel source)
    {
        var query = new FundOutQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            PlayerCode: source.PlayerCode,
            Txid: source.Txid,
            Amount: source.Amount);

        var result = await service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new FundResultViewModel(
            result.Resp);
    }

    /// <summary>
    /// Funds the status asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "playerCode": "4ue0ujotpmuu",
    ///             "txid": "1695696155"
    ///         }
    /// </remarks>
    [HttpPost("fund/Status", Name = "FundStatus")]
    public ValueTask<bool> FundStatusAsync(
        [FromServices] IGetService<FundStatusQuery, bool> service,
        FundStatusViewModel source)
        => service
        .GetAsync(
            query: new FundStatusQuery(
                ApiUrl: source.ApiUrl,
                Website: source.Website,
                Cert: source.Cert,
                PlayerCode: source.PlayerCode,
                Txid: source.Txid),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Games the URL asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "exchangeApiUrl": "https://test81.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "testpginrma",
    ///             "playerCode": "4ue0ujotpmuu",
    ///             "currency": 2
    ///         }
    /// </remarks>
    [HttpPost("game/url", Name = "GameUrl")]
    public async ValueTask<GameUrlResultViewModel> GameUrlAsync(
        [FromServices] IGetService<GameUrlQuery, GameUrlReslut> service,
        GameUrlViewModel source)
    {
        var query = new GameUrlQuery(
            ApiUrl: source.ApiUrl,
            ExchangeApiUrl: source.ExchangeApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            Agent: source.Agent,
            PlayerCode: source.PlayerCode,
            Currency: source.Currency);

        var result = await service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new GameUrlResultViewModel(
            result.GameUrl);
    }

    /// <summary>
    /// Games the wager settled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "zf055gpi",
    ///             "startTime": "2023-09-27T00:00:00+08:00",
    ///             "endTime": "2023-09-28T00:00:00+08:00",
    ///             "reportType": 7,
    ///             "pageNumber": 1
    ///         }
    /// </remarks>
    [HttpPost("game/wager/settled", Name = "GameWagerSettled")]
    public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerSettledAsync(
        [FromServices] IQueryService<GameWagerSettledQuery, GameWagerReslut> service,
        GameWagerSettledViewModel source)
    {
        var query = new GameWagerSettledQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            StartTime: source.StartTime,
            EndTime: source.EndTime,
            ReportType: source.ReportType,
            PageNumber: source.PageNumber);

        var results = await service
            .QueryAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync()
            .ConfigureAwait(false);

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameWagerResultViewModel(
                PlayerCode: result.PlayerCode,
                ExternalRoundId: result.ExternalRoundId,
                ExternalWagerId: result.ExternalWagerId,
                ExternalGameId: result.ExternalGameId,
                IsClosed: result.IsClosed,
                IsCancelled: result.IsCancelled,
                IsVoided: result.IsVoided,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                Settle: result.Settle,
                WinLoss: result.WinLoss,
                AdditionalData: result.AdditionalData);
        }
    }

    /// <summary>
    /// Games the wager unsettled asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "zf055gpi",
    ///             "lastUpdateTime": "2023-09-26T00:00:00+08:00",
    ///             "reportType": 7,
    ///             "pageNumber": 1
    ///         }
    /// </remarks>
    [HttpPost("game/wager/unsettled", Name = "GameWagerUnsettled")]
    public async IAsyncEnumerable<GameWagerResultViewModel> GameWagerUnsettledAsync(
        [FromServices] IQueryService<GameWagerUnsettledQuery, GameWagerReslut> service,
        GameWagerUnsettledViewModel source)
    {
        var query = new GameWagerUnsettledQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            LastUpdateTime: source.LastUpdateTime,
            ReportType: source.ReportType,
            PageNumber: source.PageNumber);

        var results = await service
            .QueryAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ToArrayAsync()
            .ConfigureAwait(false);

        if (!results.Any())
        {
            yield break;
        }

        foreach (var result in results)
        {
            yield return new GameWagerResultViewModel(
                PlayerCode: result.PlayerCode,
                ExternalRoundId: result.ExternalRoundId,
                ExternalWagerId: result.ExternalWagerId,
                ExternalGameId: result.ExternalGameId,
                IsClosed: result.IsClosed,
                IsCancelled: result.IsCancelled,
                IsVoided: result.IsVoided,
                ExternalCreatedOn: result.ExternalCreatedOn,
                ExternalClosedOn: result.ExternalClosedOn,
                Bet: result.Bet,
                Settle: result.Settle,
                WinLoss: result.WinLoss,
                AdditionalData: result.AdditionalData);
        }
    }

    /// <summary>
    /// Players the balance asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "playerCode": "4ue0ujotpmuu"
    ///         }
    /// </remarks>
    [HttpPost("player/balance", Name = "PlayerBalance")]
    public async ValueTask<PlayerBalanceResultViewModel> PlayerBalanceAsync(
        [FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
        PlayerBalanceViewModel source)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: source.ApiUrl,
            Website: source.Website,
            Cert: source.Cert,
            PlayerCode: source.PlayerCode);

        var result = await service
            .GetAsync(
            query: query,
            cancellationToken: HttpContext.RequestAborted)
            .ConfigureAwait(false);

        return new PlayerBalanceResultViewModel(
            result.IsPlayerExist,
            result.Balance,
            result.FreeBalance,
            result.OutstandingBalance);
    }

    /// <summary>
    /// Players the create asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "apiUrl": "https://apiinfo.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "agent": "testpginrma",
    ///             "playerCode": "4ue0ujotpmuu",
    ///             "currency": 2
    ///         }
    /// </remarks>
    [HttpPost("player/create", Name = "PlayerCreate")]
    public ValueTask PlayerCreateAsync(
        [FromServices] ICommandService<PlayerCreateCommand> service,
        PlayerCreateViewModel source)
        => service
        .ExecuteAsync(
            command: new PlayerCreateCommand(
                ApiUrl: source.ApiUrl,
                Website: source.Website,
                Cert: source.Cert,
                Agent: source.Agent,
                PlayerCode: source.PlayerCode,
                Currency: source.Currency),
            cancellationToken: HttpContext.RequestAborted);

    /// <summary>
    /// Players the logout asynchronous.
    /// </summary>
    /// <param name="service">The service.</param>
    /// <param name="source">The source.</param>
    /// <returns></returns>
    /// <remarks>
    /// <b>Example</b><br />
    ///
    ///     Post
    ///
    ///         {
    ///             "exchangeApiUrl": "https://test81.cckk77.net/",
    ///             "website": "WICKETSPRO",
    ///             "cert": "wkVwQ8sMVr252EA9",
    ///             "playerCode": "4ue0ujotpmuu"
    ///         }
    /// </remarks>
    [HttpPost("player/logout", Name = "PlayerLogout")]
    public ValueTask PlayerLogoutAsync(
        [FromServices] ICommandService<PlayerLogoutCommand> service,
        PlayerLogoutViewModel source)
    => service
        .ExecuteAsync(
            command: new PlayerLogoutCommand(
                ExchangeApiUrl: source.ExchangeApiUrl,
                Website: source.Website,
                Cert: source.Cert,
                PlayerCode: source.PlayerCode),
            cancellationToken: HttpContext.RequestAborted);
}
