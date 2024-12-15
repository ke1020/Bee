using Bee.Base.Models.Navigation;
using Bee.Base.ViewModels;

namespace Bee.Base.Abstractions.Navigation;

/// <summary>
/// 导航命令抽象基类
/// </summary>
/// <typeparam name="T">视图模型</typeparam>
/// <param name="key"></param>
/// <param name="vm"></param>
public class NavigationCommandBase<T>(string key, T vm) : INavigationCommand where T : PageViewModelBase
{
    /// <summary>
    /// 导航命令键（要与菜单中的 CommandParameter 一致）
    /// </summary>
    public string Key => key;

    public virtual void Execute(NavigationCommandContext context)
    {
        context.Navigator?.SetCurrentPage(vm); 
    }
}
