using System.Text.Json;
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
    /// <summary>
    /// 应用全局配置对象
    /// </summary>
    protected AppSettings AppSettings { get; }
    /// <summary>
    /// 服务提供者
    /// </summary>
    protected IServiceProvider ServiceProvider { get; }
    /// <summary>
    /// 插件名称
    /// </summary>
    public abstract string PluginName { get; }
    /// <summary>
    /// 插件输出目录
    /// </summary>
    public virtual string OutputPath => Path.Combine(AppSettings.OutputPath, PluginName);
    /// <summary>
    /// 插件菜单注入菜单的键 （即注入到哪个 Toolbar 一级菜单下）
    /// </summary>
    public virtual string InjectMenuKey { get; } = "Home";

    public PluginBase(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        AppSettings = ServiceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
    }

    /// <summary>
    /// 配置插件菜单
    /// </summary>
    /// <param name="menuContext"></param>
    public virtual void ConfigureMenu(MenuConfigurationContext menuContext)
    {
        var item = menuContext.Menus.FirstOrDefault(x => x.Key == InjectMenuKey);
        if (item is null)
        {
            return;
        };

        // 从插件 Configs 目录下读取插件菜单配置
        var menus = JsonSerializer.Deserialize<List<MenuItem>>(
            File.ReadAllBytes(Path.Combine(AppSettings.PluginPath, PluginName, "Configs", "menu.json")))
            ;

        if (menus == null)
        {
            return;
        }

        foreach (var menu in menus)
        {
            item.Items.Add(menu);
        }
    }

    public abstract void RegisterServices(IServiceCollection services);
}