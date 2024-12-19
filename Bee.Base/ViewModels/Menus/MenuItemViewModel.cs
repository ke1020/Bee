using System.Windows.Input;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.ViewModels.Menus;

/// <summary>
/// 菜单项视图模型
/// </summary>
public partial class MenuItemViewModel : ObservableObject
{
    /// <summary>
    /// 唯一标识
    /// </summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>
    /// 按钮文本
    /// </summary>
    public string Text { get; set; } = string.Empty;
    /// <summary>
    /// 菜单分组
    /// </summary>
    public string Group { get; set; } = string.Empty;
    /// <summary>
    /// 图标
    /// </summary>
    public StreamGeometry? Icon { get; set; }
    /// <summary>
    /// 子菜单
    /// </summary>
    public ICollection<MenuItemViewModel>? Items { get; set; }
    /// <summary>
    /// 菜单是否选中
    /// </summary>
    [ObservableProperty]
    private bool _isActive;
    /// <summary>
    /// 菜单点击命令
    /// </summary>
    public ICommand? MenuClickCommand { get; set; }
    /// <summary>
    /// 命令参数
    /// </summary>
    public string? CommandParameter { get; set; }

    public MenuItemViewModel(string text)
    {
        Text = text;
    }

    public void AddItem(MenuItemViewModel item)
    {
        Items ??= [];
        Items?.Add(item);
    }
}
