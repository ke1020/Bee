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
    /// 插件输出目录
    /// </summary>
    string OutputPath { get; }
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
