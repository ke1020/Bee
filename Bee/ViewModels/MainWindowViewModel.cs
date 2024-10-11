using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Bee.Models.Menu;
using Bee.ViewModels.Menu;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ke.Bee.Localization.Localizer.Abstractions;
using Microsoft.Extensions.Options;

namespace Bee.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    /// <summary>
    /// 工具栏按钮集合
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel>? _toolbarMenus;
    /// <summary>
    /// 设置菜单集合
    /// </summary>
    [ObservableProperty]
    private ObservableCollection<MenuItemViewModel>? _settingMenus;
    /// <summary>
    /// 本地化资源
    /// </summary>
    private readonly ILocalizer _l;
    /// <summary>
    /// 菜单数据
    /// </summary>
    private readonly MenuItem[] _menuItems;

    /// <summary>
    /// MenuItem 到 MenuItemViewModel 的转换器
    /// </summary>
    private Func<MenuItem, MenuItemViewModel> _menuItemToViewModel => x => new MenuItemViewModel(_l[x.LocaleKey])
    {
        Key = x.Key,
        IsActive = x.IsActive == true,
        Icon = string.IsNullOrWhiteSpace(x.Icon) ? null : StreamGeometry.Parse(x.Icon),
        CommandParameter = x.CommandParameter,
        Items = x.Items.Select(_menuItemToViewModel).ToList(),
        MenuClickCommand = GetRelayCommand(x.CommandType)
    };

    /// <summary>
    /// 根据命令类型返回中继命令
    /// </summary>
    /// <param name="commandType"></param>
    /// <returns></returns>
    private IRelayCommand? GetRelayCommand(string? commandType)
    {
        if (!Enum.TryParse<MenuClickCommandType>(commandType, out var cmdType))
        {
            return null;
        }

        return cmdType switch
        {
            // 激活菜单命令返回的中继命令
            MenuClickCommandType.Active => new RelayCommand<MenuItemViewModel>((MenuItemViewModel? menuItem) =>
            {
                // 已经选中
                if (menuItem is null || menuItem.IsActive == true)
                {
                    return;
                }

                // 清除之前选中项
                var beforeActived = ToolbarMenus?.FirstOrDefault(x => x.IsActive);
                if (beforeActived != null)
                {
                    beforeActived.IsActive = false;
                }

                menuItem.IsActive = true;
            }),

            // 主题切换返回的中继命令
            MenuClickCommandType.SwitchTheme => new RelayCommand<MenuItemViewModel>((MenuItemViewModel? menuItem) =>
            {
                if (Application.Current is null)
                {
                    return;
                }

                ThemeVariant? tv = menuItem?.CommandParameter switch
                {
                    nameof(ThemeVariant.Default) => ThemeVariant.Default,
                    nameof(ThemeVariant.Dark) => ThemeVariant.Dark,
                    _ => ThemeVariant.Default
                };

                // 可以通过 Application.Current 设置或修改 RequestedThemeVariant 属性
                Application.Current.RequestedThemeVariant = tv;
            }),

            // 打开链接返回的中继命令
            MenuClickCommandType.Link => new RelayCommand<MenuItemViewModel>((MenuItemViewModel? menuItem) =>
            {
                var url = menuItem?.CommandParameter;
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }),

            // 切换本地化语言
            MenuClickCommandType.SwitchLanguage => new RelayCommand<MenuItemViewModel>((MenuItemViewModel? menuItem) =>
            {
                _l.CurrentCulture = _l.AvailableCultures.FirstOrDefault(f => f.IetfLanguageTag == menuItem?.CommandParameter) ??
                    Thread.CurrentThread.CurrentCulture
                    ;

                Reload();
            }),

            // 返回 null
            _ => null
        };
    }

    public MainWindowViewModel(IOptions<MenuItem[]> menuItems, ILocalizer localizer)
    {
        _l = localizer;
        _menuItems = menuItems.Value;
        Reload();
    }

    private void Reload()
    {
        var toolbarMenus = _menuItems.Where(x => x.Group == "Toolbar").Select(_menuItemToViewModel);
        ToolbarMenus = new ObservableCollection<MenuItemViewModel>(toolbarMenus);
        var settingMenus = _menuItems.Where(x => x.Group == "Settings").Select(_menuItemToViewModel);
        SettingMenus = new ObservableCollection<MenuItemViewModel>(settingMenus);
    }
}
