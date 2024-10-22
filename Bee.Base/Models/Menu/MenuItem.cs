
namespace Bee.Base.Models.Menu;

/// <summary>
/// 菜单项实体
/// </summary>
public class MenuItem
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>
    /// 图标 Path 值
    /// </summary>
    public string Icon { get; set; } = string.Empty;
    /// <summary>
    /// 菜单是否激活状态
    /// </summary>
    public bool? IsActive { get; set; }
    /// <summary>
    /// 本地化键
    /// </summary>
    public string LocaleKey { get; set; } = string.Empty;
    /// <summary>
    /// 分组的本地化键
    /// </summary>
    public string GroupLocaleKey { get; set; } = string.Empty;
    /// <summary>
    /// 子菜单
    /// </summary>
    public ICollection<MenuItem> Items { get; set; } = [];
    /// <summary>
    /// 菜单分组
    /// </summary>
    public string? Group { get; set; }
    /// <summary>
    /// 命令类型
    /// </summary>
    public string? CommandType { get; set; }
    /// <summary>
    /// 命令参数
    /// </summary>
    public string? CommandParameter { get; set; }
}
