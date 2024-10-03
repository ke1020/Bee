using System;
using System.Collections;
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
    /// <summary>
    /// 设置菜单集合
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel> _settingMenus;

    private Func<bool, Func<MenuItem, MenuItemViewModel>> _menuItemToViewModel => (bool hasCommand) => x => new MenuItemViewModel(x.LocaleKey)
    {
        Key = x.Key,
        IsActive = x.IsActive == true,
        Icon = string.IsNullOrWhiteSpace(x.Icon) ? null : StreamGeometry.Parse(x.Icon),
        MenuClickCommand = hasCommand ? new RelayCommand<string>((string? key) =>
        {
            var item = ToolbarMenus?.FirstOrDefault(i => i.Key == key);
            if (item is not null)
            {
                if (item.IsActive == true)
                {
                    return;
                }

                // 取消之前选中项
                var actived = ToolbarMenus?.FirstOrDefault(i => i.IsActive == true);
                if (actived is not null)
                {
                    actived.IsActive = false;
                }

                item.IsActive = true;
            }
        }) : null
    };

    public MainWindowViewModel(IOptions<MenuItem[]> menuItems)
    {
        ToolbarMenus = new ObservableCollection<MenuItemViewModel>(
            menuItems.Value.Where(x => x.GroupLocaleKey == "Toolbar").Select(_menuItemToViewModel(true))
        );
        SettingMenus = new ObservableCollection<MenuItemViewModel>(
            menuItems.Value.Where(x => x.GroupLocaleKey == "Settings").Select(_menuItemToViewModel(false))
        );
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
