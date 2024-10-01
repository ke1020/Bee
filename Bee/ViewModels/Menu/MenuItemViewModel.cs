using System.Collections.Generic;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.ViewModels.Menu;

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
    /// 图标
    /// </summary>
    public StreamGeometry? Icon { get; set; }
    /// <summary>
    /// 子菜单
    /// </summary>
    public ICollection<MenuItemViewModel>? Items { get; private set; }
    /// <summary>
    /// 菜单是否选中
    /// </summary>
    [ObservableProperty]
    private bool _isActive;

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
