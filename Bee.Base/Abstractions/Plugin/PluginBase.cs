using Bee.Base.Models;
using Bee.Base.Models.Menu;
using Bee.Base.Models.Plugin;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bee.Base.Abstractions.Plugin;

/// <summary>
/// 插件基类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class PluginBase : IPlugin
{
    protected AppSettings AppSettings { get; }

    public PluginBase(IOptions<AppSettings> appSettings)
    {
        AppSettings = appSettings.Value;
    }

    /// <summary>
    /// 插件名称
    /// </summary>
    public abstract string PluginName { get; }

    public abstract void ConfigureMenu(MenuConfigurationContext menuContext);

    public abstract R? Execute<T, R>(string methodName, T? parameters) where R : PluginResult;

    public abstract void RegisterServices(IServiceCollection services);
}