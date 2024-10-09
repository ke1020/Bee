
namespace Bee.Models.Menu;

/// <summary>
/// 菜单按钮点击命令类型
/// </summary>
public enum MenuClickCommandType
{
    /// <summary>
    /// 无效
    /// </summary>
    None = 0,
    /// <summary>
    /// 激活菜单命令
    /// </summary>
    Active,
    /// <summary>
    /// 切换主题命令
    /// </summary>
    SwitchTheme,
    /// <summary>
    /// 超链接
    /// </summary>
    Link,
    /// <summary>
    /// 导航命令
    /// </summary>
    Navigate
}
