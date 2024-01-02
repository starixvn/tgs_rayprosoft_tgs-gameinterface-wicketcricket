using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Tgs.GameInterface.WicketCricket.Core.ApplicationServices;
using Tgs.GameInterface.WicketCricket.Protos;
using Tgs.ServiceUtility;

namespace Tgs.GameInterface.WicketCricket.GrpcServices;

public class GameInterfaceWicketCricketGrpcService(
    ILogger<GameInterfaceWicketCricketGrpcService> logger,
    IGetService<FundInQuery, FundResult> fundInService,
    IGetService<FundOutQuery, FundResult> fundOutService,
    IGetService<FundStatusQuery, bool> fundStatusService,
    IGetService<GameUrlQuery, GameUrlReslut> gameUrlService,
    IQueryService<GameWagerSettledQuery, GameWagerReslut> gameWagerSettledService,
    IQueryService<GameWagerUnsettledQuery, GameWagerReslut> gameWagerUnsettledService,
    IGetService<PlayerBalanceQuery, PlayerBalanceResult> playerBalanceService,
    ICommandService<PlayerCreateCommand> playerCreateService,
    ICommandService<PlayerLogoutCommand> playerLogoutService) : GameInterfaceWicketCricket.GameInterfaceWicketCricketBase
{
    public override Task<CheckingServiceReply> CheckingService(CheckingServiceRequest request, ServerCallContext context)
    {
        logger.LogWarning("LogOn:{logOn} Message: Create a check connection.({machineName})", DateTime.Now.ToString("o"), request.MachineName);

        return Task.FromResult(new CheckingServiceReply
        {
            MachineName = Environment.MachineName
        });
    }

    public override async Task<FundReply> FundIn(FundRequest request, ServerCallContext context)
    {
        var query = new FundInQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            PlayerCode: request.PlayerCode,
            Txid: request.Txid,
            Amount: request.Amount);

        var reslut = await fundInService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Resp = reslut.Resp
        };
    }

    public override async Task<FundReply> FundOut(FundRequest request, ServerCallContext context)
    {
        var query = new FundOutQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            PlayerCode: request.PlayerCode,
            Txid: request.Txid,
            Amount: request.Amount);

        var reslut = await fundOutService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new FundReply()
        {
            Resp = reslut.Resp
        };
    }

    public override async Task<BaseReply> FundStatus(FundStatusRequest request, ServerCallContext context)
    {
        var query = new FundStatusQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            PlayerCode: request.PlayerCode,
            Txid: request.Txid);

        var reslut = await fundStatusService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = reslut
        };
    }

    public override async Task<GameUrlReply> GameUrl(GameUrlRequest request, ServerCallContext context)
    {
        var query = new GameUrlQuery(
            ApiUrl: new Uri(request.ApiUrl),
            ExchangeApiUrl: new Uri(request.ExchangeApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            Currency: request.Currency);

        var reslut = await gameUrlService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new GameUrlReply()
        {
            GameUrl = reslut.GameUrl
        };
    }

    public override async Task<GameWagerReply> GameWagerSettled(GameWagerSettledRequest request, ServerCallContext context)
    {
        var query = new GameWagerSettledQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            StartTime: request.StartTime.ToDateTimeOffset(),
            EndTime: request.EndTime.ToDateTimeOffset(),
            ReportType: (Common.ReportTypes)request.ReportType,
            PageNumber: request.PageNumber);

        var resluts = await gameWagerSettledService
            .QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameWagerReply();

        if (resluts.Length != 0)
        {
            foreach (var reslut in resluts)
            {
                reply.Items.Add(new GameWagerInfo
                {
                    PlayerCode = reslut.PlayerCode,
                    ExternalRoundId = reslut.ExternalRoundId,
                    ExternalWagerId = reslut.ExternalWagerId,
                    ExternalGameId = reslut.ExternalGameId,
                    IsClosed = reslut.IsClosed,
                    IsCancelled = reslut.IsCancelled,
                    IsVoided = reslut.IsVoided,
                    ExternalCreatedOn = reslut.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = reslut.ExternalClosedOn.ToTimestamp(),
                    Bet = reslut.Bet,
                    Settle = reslut.Settle,
                    WinLoss = reslut.WinLoss,
                    AdditionalData = reslut.AdditionalData
                });
            }
        }
        return reply;
    }

    public override async Task<GameWagerReply> GameWagerUnsettled(GameWagerUnsettledRequest request, ServerCallContext context)
    {
        var query = new GameWagerUnsettledQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            LastUpdateTime: request.LastUpdateTime.ToDateTimeOffset(),
            ReportType: (Common.ReportTypes)request.ReportType,
            PageNumber: request.PageNumber);

        var resluts = await gameWagerUnsettledService
            .QueryAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ToArrayAsync(
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        var reply = new GameWagerReply();

        if (resluts.Length != 0)
        {
            foreach (var reslut in resluts)
            {
                reply.Items.Add(new GameWagerInfo
                {
                    PlayerCode = reslut.PlayerCode,
                    ExternalRoundId = reslut.ExternalRoundId,
                    ExternalWagerId = reslut.ExternalWagerId,
                    ExternalGameId = reslut.ExternalGameId,
                    IsClosed = reslut.IsClosed,
                    IsCancelled = reslut.IsCancelled,
                    IsVoided = reslut.IsVoided,
                    ExternalCreatedOn = reslut.ExternalCreatedOn.ToTimestamp(),
                    ExternalClosedOn = reslut.ExternalClosedOn.ToTimestamp(),
                    Bet = reslut.Bet,
                    Settle = reslut.Settle,
                    WinLoss = reslut.WinLoss,
                    AdditionalData = reslut.AdditionalData
                });
            }
        }
        return reply;
    }

    public override async Task<PlayerBalanceReply> PlayerBalance(PlayerBalanceRequest request, ServerCallContext context)
    {
        var query = new PlayerBalanceQuery(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            PlayerCode: request.PlayerCode);

        var reslut = await playerBalanceService
            .GetAsync(
            query: query,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new PlayerBalanceReply()
        {
            IsPlayerExist = reslut.IsPlayerExist,
            Balance = reslut.Balance,
            FreeBalance = reslut.FreeBalance,
            OutstandingBalance = reslut.OutstandingBalance,
        };
    }

    public override async Task<BaseReply> PlayerCreate(PlayerCreateRequest request, ServerCallContext context)
    {
        var command = new PlayerCreateCommand(
            ApiUrl: new Uri(request.ApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            Agent: request.Agent,
            PlayerCode: request.PlayerCode,
            Currency: request.Currency);

        await playerCreateService
            .ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }

    public override async Task<BaseReply> PlayerLogout(PlayerLogoutRequest request, ServerCallContext context)
    {
        var command = new PlayerLogoutCommand(
            ExchangeApiUrl: new Uri(request.ExchangeApiUrl),
            Website: request.Website,
            Cert: request.Cert,
            PlayerCode: request.PlayerCode);

        await playerLogoutService
            .ExecuteAsync(
            command: command,
            cancellationToken: context.CancellationToken)
            .ConfigureAwait(false);

        return new BaseReply()
        {
            Status = true
        };
    }
}
