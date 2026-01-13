namespace Pandora.Services.Interfaces;

public enum WindowVisualState
{
    Idle,
    Running,
    Error,
    Indeterminate,
}

public interface IWindowStateService
{
    void SetVisualState(WindowVisualState state);

    void FlashWindow();

    void Shutdown();
}