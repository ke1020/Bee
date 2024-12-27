using Avalonia;
using Avalonia.Media;
using Bee.Base;
using Bee.Services;
using Serilog;
using System;

namespace Bee;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // 配置 Serilog
        Log.Logger = new LoggerConfiguration()
            // .MinimumLevel.Debug() // 设置最低日志级别
            .WriteTo.File("logs/log.txt") // 日志文件路径
            .CreateLogger();

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Rapid Unscheduled Disassembly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<App>()
        .UsePlatformDetect()
        .ConfigureFonts(fontManager =>
        {
            fontManager.AddFontCollection(new HarmonyOSFontCollection());
        })
        .With(new FontManagerOptions()
        {
            DefaultFamilyName = "fonts:HarmonyOS Sans#HarmonyOS Sans SC"
        })
        .LogToSerilog(Avalonia.Logging.LogEventLevel.Warning)
        .LogToTrace()
        ;
}
