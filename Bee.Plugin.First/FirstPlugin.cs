using Bee.Base.Abstractions.Navigation;
using Bee.Base.Abstractions.Plugin;
using Bee.Base.Models;
using Bee.Base.Models.Menu;
using Bee.Base.Models.Plugin;
using Bee.Plugin.First.ViewModels;
using Ke.Bee.Localization.Providers.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bee.Plugin.First;

public class MethodParameter
{
    public string Name { get; set; }
}

public class FirstPlugin : PluginBase
{
    public override string PluginName => "First";

    public FirstPlugin(IOptions<AppSettings> appSettings) : base (appSettings)
    {

    }

    public override void ConfigureMenu(MenuConfigurationContext menuContext)
    {
        var item = menuContext.Menus.FirstOrDefault(x => x.Key == "Home");
        if (item is null)
        {
            return;
        };

        item.Items.Add(new MenuItem
        {
            Key = "UmlGenerator",
            LocaleKey = "Menu.Sidebar.UmlGenerator",
            GroupLocaleKey = "Menu.Sidebar.DevTools",
            CommandType = "Navigate",
            CommandParameter = "First"
        });
    }

    public override R? Execute<T, R>(string methodName, T? parameters)
        where T : default
        where R : class
    {
        return methodName switch
        {
            "method1" => Method1(parameters as MethodParameter) as R,
            _ => throw new Exception("插件方法不存在")
        };
    }

    private PluginResult Method1(MethodParameter? parameter)
    {
        // 方法逻辑
        return new PluginResult(true);
    }

    public override void RegisterServices(IServiceCollection services)
    {
        services.AddTransient<IPlugin, FirstPlugin>();
        services.AddTransient<FirstViewModel>();
        services.AddSingleton<INavigationCommand, FirstPluginNavigationCommand>();
        services.AddSingleton<ILocalizaitonResourceContributor, FirstLocalizaitonResourceContributor>();
    }
}
