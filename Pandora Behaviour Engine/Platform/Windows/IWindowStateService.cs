namespace Pandora.Platform.Windows;

public interface IWindowStateService
{
    void SetVisualState(WindowVisualState state);

    void FlashWindow();

    void Shutdown();
}