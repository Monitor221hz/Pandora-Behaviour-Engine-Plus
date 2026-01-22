namespace Pandora.Logging.NLogger.UI;

public abstract record LogUiEvent
{
	public sealed record Message(string Text) : LogUiEvent;
	public sealed record Clear : LogUiEvent;
}
