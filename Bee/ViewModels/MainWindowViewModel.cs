using System.Collections.ObjectModel;
using System.Linq;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Bee.Models;
using Bee.ViewModels.Menu;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Options;

namespace Bee.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
#pragma warning disable CA1822 // Mark members as static
    public string Greeting => "Welcome to Avalonia!";
#pragma warning restore CA1822 // Mark members as static

    /// <summary>
    /// 工具栏按钮集合
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _toolbarMenus;

    public MainWindowViewModel(IOptions<MenuItem[]> menuItems)
    {
        var menuList = menuItems.Value.Select(x => new MenuItemViewModel(x.LocaleKey)
        {
            Key = x.Key,
            IsActive = x.IsActive == true,
            Icon = string.IsNullOrWhiteSpace(x.Icon) ? null : StreamGeometry.Parse(x.Icon),
        });
        ToolbarMenus = new ObservableCollection<MenuItemViewModel>(menuList);
    }

    /// <summary>
    /// 改变主题方法
    /// </summary>
    [RelayCommand]
    private void ChangeTheme()
    {
        if (Application.Current is null)
        {
            return;
        }

        // 可以通过 Application.Current 设置或修改 RequestedThemeVariant 属性
        Application.Current.RequestedThemeVariant = Application.Current.RequestedThemeVariant == ThemeVariant.Dark
            ? ThemeVariant.Default : ThemeVariant.Dark;
    }

    /*
    private RelayCommand? changeThemeCommand;

    public IRelayCommand ChangeThemeCommand => changeThemeCommand ??= new RelayCommand(ChangeTheme);
    */
}
