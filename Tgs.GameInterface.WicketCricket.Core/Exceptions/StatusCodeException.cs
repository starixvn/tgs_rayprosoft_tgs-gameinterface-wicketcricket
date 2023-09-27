namespace Tgs.GameInterface.WicketCricket.Core.Exceptions;

public class StatusCodeException : Exception
{
    public StatusCodeException(string status) : base(ConvertErrorMesaage(status))
    {
    }

    public StatusCodeException(int status) : base(ConvertErrorMesaage(status))
    {
    }

    private static string ConvertErrorMesaage(int status) => ConvertErrorMesaage(status.ToString());

    private static string ConvertErrorMesaage(string status) => status switch
    {
        "0" => "Fail(失敗)",
        "1" => "Success(成功)",
        "1001" => "Not Authorized.(未授權。)",
        "1002" => "Insufficient parameters.(參數不足。)",
        "1003" => "Invalid parameters.(無效參數。)",
        "1004" => "Invalid URL.(無效的網址。)",
        "1005" => "Userid is invalid.(用戶 ID 無效。)",
        "1006" => "Username is invalid.(無效的使用者名稱。)",
        "1007" => "Exposure limit is invalid.(暴露限制無效。)",
        "1008" => "Invalid Agent.(無效代理。)",
        "1009" => "Account Locked.(帳戶被鎖定。)",
        "1010" => "Query too fast.(查詢速度太快。)",
        "1011" => "System logout.(系統註銷。)",
        "1012" => "This action already completed.(這個動作已經完成了。)",
        "1013" => "Insufficient balance.(餘額不足。)",
        "1014" => "No data.(沒有數據。)",
        "1015" => "Invalid Date.(失效日期。)",
        "1016" => "Invalid bet status type.(無效的投注狀態類型。)",
        "1017" => "Query over two months.(查詢超過兩個月。)",
        "1018" => "Invalid Agent.(無效代理。)",
        "1019" => "Account profit loss over than max win limit.(帳戶利潤損失超過最大獲利限額。)",
        "1020" => "Invalid time zone.(時區無效。)",
        "1021" => "Invalid get key token.(取得密鑰令牌無效。)",
        "1022" => "Invalid Commission.(無效佣金。)",
        "1023" => "Update Commission Error.(更新佣金錯誤。)",
        "1024" => "Agent not active.(代理未啟動。)",
        "1025" => "This operation not exist.(該操作不存在。)",
        "1026" => "enter too many users.(輸入太多使用者。)",
        "1027" => "Invalid MarketId.(市場 ID 無效。)",
        "1028" => "Invalid EventId.(無效的事件 ID。)",
        "1029" => "Query over one day.(查詢超過一天。)",
        "1030" => "Invalid Agent PT.(代理 PT 無效。)",
        "1031" => "Account Suspended.(帳戶已暫停。)",
        "1032" => "Not Allow Betting.(不允許投注。)",
        "1033" => "Data Duplicate.(資料重複。)",
        "1034" => "UserId is not a player.(UserId 不是玩家。)",
        "1035" => "Query over 35 day.(查詢超過35天。)",
        "1036" => "Invalid currency.(貨幣無效。)",
        "1037" => "Invalid date hour time.(日期時間無效。)",
        "1038" => "Operation processing.(操作處理。)",
        "1039" => "Query over limit.(查詢超出限制。)",
        "1040" => "Invalid IP.(無效的IP。)",
        "1041" => "Under Maintenance.(正在維修。)",
        "1042" => "UpdateDate needs to be more than one minute away from the current time.(UpdateDate 需要與目前時間相差一分鐘以上。)",
        "1043" => "Invalid eventType.(無效的事件類型。)",
        _ => $"遊戲供應商未提供錯誤代碼{status}"
    };
}
