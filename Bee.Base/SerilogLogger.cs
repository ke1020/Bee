using Avalonia;
using Avalonia.Logging;

namespace Bee.Base;

/// <summary>
/// Serilog 日志接收器
/// </summary>
/// <param name="minimumLevel"></param>
/// <param name="areas"></param>
public class SerilogLogSink(LogEventLevel minimumLevel, IEnumerable<string> areas) : ILogSink
{
    private readonly LogEventLevel _minimumLevel = minimumLevel;
    private readonly IEnumerable<string> _areas = areas;

    public bool IsEnabled(LogEventLevel level, string area) =>
         level >= _minimumLevel && (_areas?.Contains(area) ?? true);

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate)
    {
        if (IsEnabled(level, area))
        {
            Serilog.Log.Write(
                (Serilog.Events.LogEventLevel)level,
                messageTemplate,
                source?.ToString())
                ;
        }
    }

    public void Log(LogEventLevel level, string area, object? source, string messageTemplate,
        params object?[] propertyValues)
    {
        if (IsEnabled(level, area))
        {
            Serilog.Log.Write(
                (Serilog.Events.LogEventLevel)level,
                messageTemplate,
                source?.ToString(),
                propertyValues)
                ;
        }
    }
}

public static class LoggingExtensions
{
    /// <summary>
    /// Logs Avalonia events to the <see cref="System.Diagnostics.Trace"/> sink.
    /// </summary>
    /// <param name="builder">The app builder instance.</param>
    /// <param name="level">The minimum level to log.</param>
    /// <param name="areas">The areas to log. Valid values are listed in <see cref="LogArea"/>.</param>
    /// <returns>The app builder instance.</returns>
    public static AppBuilder LogToSerilog(
        this AppBuilder builder,
        LogEventLevel level = LogEventLevel.Warning,
        params string[] areas)
    {
        Logger.Sink = new SerilogLogSink(level, areas);
        return builder;
    }
}