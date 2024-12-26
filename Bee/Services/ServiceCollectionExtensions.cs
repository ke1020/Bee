using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Bee.Base.Abstractions.Navigation;
using Bee.Base.Abstractions.Plugin;
using Bee.Base.Impl.Navigation;
using Bee.Base.Models;
using Bee.Base.Models.Menu;
using Bee.Services.Impl.Navigation.Commands;
using Bee.ViewModels;
using Bee.ViewModels.Images;
using Ke.Bee.Localization.Extensions;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bee.Services;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<PosterGeneratorViewModel>();

        services.AddSettings();
        services.AddMenus();

        // 注册视图导航器
        services.AddSingleton<IViewNavigator, DefaultViewNavigator>();

        // 注册命令
        services.AddSingleton<INavigationCommand, PosterGeneratorNavigationCommand>();

        services.AddPlugins();

        // 注册本地化
        services.AddLocalization();

        return services;
    }

    /// <summary>
    /// 注册全局配置
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddSettings(this IServiceCollection services)
    {
        var appSettings = JsonSerializer.Deserialize<AppSettings>(
            File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Configs", "appsettings.json"))) ??
            new AppSettings
            {
                OutputPath = Path.Combine(AppContext.BaseDirectory, "Output")
            }
            ;


        if (!Path.IsPathRooted(appSettings.OutputPath))
        {
            appSettings.OutputPath = Path.Combine(AppContext.BaseDirectory, appSettings.OutputPath);
        }

        appSettings.PluginPath = Path.Combine(AppContext.BaseDirectory, "Plugins");
        services.AddSingleton(Options.Create(appSettings));
        return services;
    }

    /// <summary>
    /// 注册应用菜单
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddMenus(this IServiceCollection services)
    {
        // 从配置文件读取菜单注入到 DI 容器
        var menuItems = JsonSerializer.Deserialize<List<MenuItem>>(
            File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Configs", "menus.json"))
            );
        var menuContext = new MenuConfigurationContext(menuItems);
        services.AddSingleton(menuContext);
        return services;
    }

    /// <summary>
    /// 注册插件服务
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddPlugins(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var appSettings = serviceProvider.GetService<IOptions<AppSettings>>();
        var pluginPath = appSettings?.Value.PluginPath;
        if (!Directory.Exists(pluginPath))
        {
            return services;
        }

        var menuContext = serviceProvider.GetService<MenuConfigurationContext>();
        // 插件根目录数组
        var pluginDirectories = Directory.GetDirectories(pluginPath);

        List<string> files = [];
        if (pluginDirectories != null)
        {
            // 只加载插件根目录下的 dll 文件
            foreach (var pluginDir in pluginDirectories)
            {
                files.AddRange(Directory.GetFiles(pluginDir, "*.dll"));
            }
        }

        try
        {
            foreach (var file in files)
            {
                var assembly = Assembly.LoadFrom(file);
                var plugins = assembly.GetTypes()
                    // 所有 IPlugin 接口的非抽象类实现
                    .Where(t => typeof(PluginBase).IsAssignableFrom(t) && !t.IsAbstract)
                    // 创建实例
                    .Select(t => (IPlugin)Activator.CreateInstance(t, serviceProvider)!)
                    ;

                foreach (var plugin in plugins)
                {
                    plugin.RegisterServices(services);
                    plugin.ConfigureMenu(menuContext!);
                }
            }
        }
        catch (ReflectionTypeLoadException ex)
        {
            File.WriteAllText(Path.Combine(AppContext.BaseDirectory, "run.log"), ex.Message);
            foreach (var loaderEx in ex.LoaderExceptions)
            {
                throw new Exception(loaderEx?.Message);
            }
        }
        return services;
    }

    /// <summary>
    /// 注册本地化
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    private static IServiceCollection AddLocalization(this IServiceCollection services)
    {
        services.AddLocalization<AvaloniaJsonLocalizationProvider>(() =>
        {
            var options = new AvaloniaLocalizationOptions(
                // 支持的本地化语言文化
                [
                    new("en-US"),
                    new("zh-CN")
                ],
                // defaultCulture, 用于设置当前文化（currentCulture）不在 cultures 列表中时的情况以及作为缺失的本地化条目的备用文化（fallback culture）
                new CultureInfo("en-US"),
                // currentCulture 在基础设施加载时设置，可以从应用程序设置或其他地方获取
                Thread.CurrentThread.CurrentCulture,
                // 包含本地化 JSON 文件的资源路径
                $"{typeof(App).Namespace}/Assets/i18n");
            return options;
        });
        return services;
    }
}