using Bee.Base.Abstractions.Plugin;
using Bee.Base.Models;
using Bee.Base.Models.Plugin;
using Bee.Base.ViewModels;
using CommunityToolkit.Mvvm.Input;
using Ke.Bee.Localization.Localizer.Abstractions;
using Microsoft.Extensions.Options;

namespace Bee.Plugin.First.ViewModels;

public partial class FirstViewModel : PageViewModelBase
{
    private readonly IPlugin? _plugin;
    public FirstViewModel(IOptions<AppSettings> appSettings, ILocalizer localizer, IEnumerable<IPlugin> plugins)
    {
        _plugin = plugins.FirstOrDefault(x => x.PluginName == "First");
    }

    [RelayCommand]
    private void ChangeText()
    {
       var result =  _plugin?.Execute<MethodParameter, PluginResult>("method1", new MethodParameter { Name = "Hello" });
       if(result?.OK == true)
       {

       }
    }
}
