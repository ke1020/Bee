using Bee.Base.Models.Menu;
using Bee.Base.Models.Plugin;
using Microsoft.Extensions.DependencyInjection;

namespace Bee.Base.Abstractions.Plugin;

/// <summary>
/// 插件接口
/// </summary>
public interface IPlugin
{
    /// <summary>
    /// 插件名称
    /// </summary>
    string PluginName { get; }
    /// <summary>
    /// 执行方法
    /// </summary>
    /// <param name="methodName">方法名称</param>
    /// <param name="parameters">参数</param>
    /// <returns>执行结果对象</returns>
    R? Execute<T, R>(string methodName, T? parameters) where R : PluginResult;
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="services"></param>
    void RegisterServices(IServiceCollection services);
    /// <summary>
    /// 配置菜单
    /// </summary>
    /// <param name="menuContext"></param>
    void ConfigureMenu(MenuConfigurationContext menuContext);
}
