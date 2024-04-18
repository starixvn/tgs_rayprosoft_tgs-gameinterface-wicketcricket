namespace Tgs.GameInterface.WicketCricket.Core;

public record PolicySettingsOptions
{
	/// <summary>
	/// The retry count
	/// </summary>
	public int RetryCount { get; init; }
	/// <summary>
	/// The retry wait sec
	/// </summary>
	public int RetryWaitSec { get; init; }
}
