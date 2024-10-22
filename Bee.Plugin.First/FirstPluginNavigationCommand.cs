using Bee.Base.Abstractions.Navigation;
using Bee.Base.Models.Navigation;
using Bee.Plugin.First.ViewModels;

namespace Bee.Plugin.First;

public class FirstPluginNavigationCommand : INavigationCommand
{
    public string Key => "First";

    private readonly FirstViewModel _vm;

    public FirstPluginNavigationCommand(FirstViewModel firstViewModel)
    {
        _vm = firstViewModel;
    }

    public void Execute(NavigationCommandContext context)
    {
        context.Navigator?.SetCurrentPage(_vm);
    }
}
