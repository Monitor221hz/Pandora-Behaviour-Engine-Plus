using System.Text;
using System.Threading.Tasks;

namespace Pandora.Services;

public class LogService
{
    private readonly StringBuilder _logBuilder = new();

    public string LogText => _logBuilder.ToString();

    public Task ClearAsync()
    {
        _logBuilder.Clear();
        return Task.CompletedTask;
    }

    public Task WriteLineAsync(string message)
    {
        _logBuilder.AppendLine(message);
        return Task.CompletedTask;
    }

    public Task WriteAsync(string message)
    {
        _logBuilder.Append(message);
        return Task.CompletedTask;
    }
}
