
using Bee.Base.Models.Navigation;

namespace Bee.Base.Abstractions.Navigation;

/// <summary>
/// 导航命令接口
/// </summary>
public interface INavigationCommand
{
    /// <summary>
    /// 页面唯一标识
    /// </summary>
    string Key { get; }
    /// <summary>
    /// 执行导航
    /// </summary>
    /// <param name="context">执行导航命令时传递的上下文信息</param>
    void Execute(NavigationCommandContext context);
}
