
using Bee.Models.Navigation;

namespace Bee.Services.Abstractions.Navigation;

/// <summary>
/// 导航命令接口
/// </summary>
public interface INavigationCommand
{
    /// <summary>
    /// 执行导航
    /// </summary>
    /// <param name="context">执行导航命令时传递的上下文信息</param>
    void Execute(NavigationCommandContext context);
}
