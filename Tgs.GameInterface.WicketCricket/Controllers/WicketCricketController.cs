using Microsoft.AspNetCore.Mvc;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
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
	[HttpPost("fund/in", Name = "FundIn")]
	public ValueTask<FundResult> FundInAsync(
		[FromServices] IGetService<FundInQuery, FundResult> service,
		[FromBody] FundInQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the out asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("fund/out", Name = "FundOut")]
	public ValueTask<FundResult> FundOutAsync(
		[FromServices] IGetService<FundOutQuery, FundResult> service,
		[FromBody] FundOutQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Funds the status asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("fund/Status", Name = "FundStatus")]
	public ValueTask<bool> FundStatusAsync(
		[FromServices] IGetService<FundStatusQuery, bool> service,
		[FromBody] FundStatusQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the URL asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/url", Name = "GameUrl")]
	public ValueTask<GameUrlReslut> GameUrlAsync(
		[FromServices] IGetService<GameUrlQuery, GameUrlReslut> service,
		[FromBody] GameUrlQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record settled asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/record/settled", Name = "GameRecordSettled")]
	public IAsyncEnumerable<GameRecordReslut> GameRecordSettledAsync(
		[FromServices] IQueryService<GameRecordSettledQuery, GameRecordReslut> service,
		GameRecordSettledQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Games the record unsettled asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("game/record/unsettled", Name = "GameRecordUnsettled")]
	public IAsyncEnumerable<GameRecordReslut> GameRecordUnsettledAsync(
		[FromServices] IQueryService<GameRecordUnsettledQuery, GameRecordReslut> service,
		GameRecordUnsettledQuery source)
		=> service.QueryAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the balance asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("player/balance", Name = "PlayerBalance")]
	public ValueTask<PlayerBalanceResult> PlayerBalanceAsync(
		[FromServices] IGetService<PlayerBalanceQuery, PlayerBalanceResult> service,
		PlayerBalanceQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the create asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("player/create", Name = "PlayerCreate")]
	public ValueTask PlayerCreateAsync(
		[FromServices] ICommandService<PlayerCreateCommand> service,
		PlayerCreateCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Players the logout asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("player/logout", Name = "PlayerLogout")]
	public ValueTask PlayerLogoutAsync(
		[FromServices] ICommandService<PlayerLogoutCommand> service,
		PlayerLogoutCommand source)
		=> service.ExecuteAsync(
			command: source,
			cancellationToken: HttpContext.RequestAborted);

	/// <summary>
	/// Queries the domain asynchronous.
	/// </summary>
	/// <param name="service">The service.</param>
	/// <param name="source">The source.</param>
	/// <returns></returns>
	[HttpPost("query/domain", Name = "QueryDomain")]
	public ValueTask<QueryDomainResult> QueryDomainAsync(
		[FromServices] IGetService<QueryDomainQuery, QueryDomainResult> service,
		QueryDomainQuery source)
		=> service.GetAsync(
			query: source,
			cancellationToken: HttpContext.RequestAborted);
}
