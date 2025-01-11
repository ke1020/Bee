using Bee.Base.Abstractions.Navigation;
using Bee.ViewModels;

namespace Bee.Navigation.Commands;

/// <summary>
/// 导航命令
/// </summary>
/// <param name="vm"></param>
public class DonateNavigationCommand(DonateViewModel vm) : 
    NavigationCommandBase<DonateViewModel>("Donate", vm)
{
}