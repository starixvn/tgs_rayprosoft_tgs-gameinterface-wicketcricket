using System.Diagnostics;

namespace Tgs.GameInterface.WicketCricket.Client;

public class DiagnosticsInterceptorOptions
{
	public ActivitySource InterceptorActivitySource { get; set; } = new ActivitySource(typeof(DiagnosticsInterceptorOptions).FullName!);
}
