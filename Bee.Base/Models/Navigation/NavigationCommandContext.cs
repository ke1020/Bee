

using Bee.Base.Abstractions.Navigation;

namespace Bee.Base.Models.Navigation;

/// <summary>
/// 视图导航命令上下文对象
/// </summary>
public class NavigationCommandContext
{
    /// <summary>
    /// 视图导航器对象
    /// </summary>
    public IViewNavigator? Navigator { get; set; }

    public NavigationCommandContext(IViewNavigator? navigator)
    {
        Navigator = navigator;
    }
}
