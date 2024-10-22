
namespace Bee.Base.Models.Menu;

/// <summary>
/// 菜单配置上下文对象
/// </summary>
public class MenuConfigurationContext
{
    public List<MenuItem> Menus { get; set; }

    public MenuConfigurationContext(List<MenuItem>? menuItems = null)
    {
        Menus = menuItems ?? [];
    }
}
