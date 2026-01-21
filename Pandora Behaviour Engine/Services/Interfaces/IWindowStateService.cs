using Pandora.Enums;

namespace Pandora.Services.Interfaces;

public interface IWindowStateService
{
    void SetVisualState(WindowVisualState state);

    void FlashWindow();

    void Shutdown();
}