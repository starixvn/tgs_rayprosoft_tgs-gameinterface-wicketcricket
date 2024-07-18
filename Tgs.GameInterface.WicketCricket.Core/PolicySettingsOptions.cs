namespace Tgs.GameInterface.WicketCricket.Core;

public record PolicySettingsOptions
{
	/// <summary>
	/// The global retry count
	/// </summary>
	public int GlobalRetryCount { get; init; }
	/// <summary>
	/// The global retry wait sec
	/// </summary>
	public int GlobalRetryWaitSec { get; init; }
}
