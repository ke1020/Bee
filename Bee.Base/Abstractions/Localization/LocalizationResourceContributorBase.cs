using System.Globalization;
using System.Text.Json;
using Bee.Base.Models;

using Ke.Bee.Localization.Providers.Abstractions;

using Microsoft.Extensions.Options;

namespace Bee.Base.Abstractions.Localization;

/// <summary>
/// 本地化资源贡献基类
/// </summary>
public abstract class LocalizationResourceContributorBase : ILocalizationResourceContributor
{
    /// <summary>
    /// 插件根目录
    /// </summary>
    private readonly string _pluginRoot;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appSettings">全局配置</param>
    /// <param name="pluginName">插件名称</param>
    public LocalizationResourceContributorBase(IOptions<AppSettings> appSettings, string pluginName)
    {
        _pluginRoot = Path.Combine(appSettings.Value.PluginPath, pluginName);
    }

    /// <summary>
    /// 获取本地化资源的默认实现
    /// </summary>
    /// <param name="culture"></param>
    /// <returns></returns>
    public virtual Dictionary<string, string>? GetResource(CultureInfo culture)
    {
        var resource = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllBytes(
            Path.Combine(_pluginRoot, "i18n", $"{culture.IetfLanguageTag}.json")
        ));

        return resource;
    }
}