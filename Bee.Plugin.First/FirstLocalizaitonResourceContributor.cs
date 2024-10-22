
using System.Globalization;
using System.Text.Json;
using Bee.Base.Models;
using Ke.Bee.Localization.Providers.Abstractions;
using Microsoft.Extensions.Options;

namespace Bee.Plugin.First;

public class FirstLocalizaitonResourceContributor : ILocalizaitonResourceContributor
{
    private readonly AppSettings _appSettings;
    public FirstLocalizaitonResourceContributor(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
    }

    public Dictionary<string, string>? GetResource(CultureInfo culture)
    {
        var resource = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllBytes(
            Path.Combine(_appSettings.PluginPath, "First", "i18n", $"{culture.IetfLanguageTag}.json")
        ));

        return resource;
    }
}
