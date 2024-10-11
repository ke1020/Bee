using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.Json;
using System.Threading;
using Bee.Models.Menu;
using Bee.ViewModels;
using Ke.Bee.Localization.Extensions;
using Ke.Bee.Localization.Options;
using Ke.Bee.Localization.Providers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bee;

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

        // 从配置文件读取菜单注入到 DI 容器
        var menuItems = JsonSerializer.Deserialize<MenuItem[]>(
            File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Configs", "menus.json"))
            );
        services.AddSingleton(Options.Create(menuItems!));

        // 注册本地化
        services.AddLocalization();

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
                new List<CultureInfo>
                {
                    new("en-US"),
                    new("zh-CN")
                },
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