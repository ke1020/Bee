using System;
using System.IO;
using System.Text.Json;
using Bee.Models.Menu;
using Bee.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bee;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<MainWindowViewModel>();

        // 从配置文件读取菜单注入到 DI 容器
        var menuItems = JsonSerializer.Deserialize<MenuItem[]>(
            File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "Configs", "menus.json"))
            );
        services.AddSingleton(Options.Create(menuItems!));

        return services;
    }
}