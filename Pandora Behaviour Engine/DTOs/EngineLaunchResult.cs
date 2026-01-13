using System;

namespace Pandora.DTOs;

public record EngineResult(bool IsSuccess, TimeSpan Duration, string? Message)
{
    public static EngineResult Fail(string? message, TimeSpan duration = default) =>
        new(false, duration, message);

    public static EngineResult Success(string? message, TimeSpan duration) =>
        new(true, duration, message);
}