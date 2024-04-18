using System.Diagnostics;

namespace Tgs.GameInterface.WicketCricket;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } =
		new ActivitySource($"Tgs.GameInterface.WicketCricket.DiagnosticsInterceptor");
}
