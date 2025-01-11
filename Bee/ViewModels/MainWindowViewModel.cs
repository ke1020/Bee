using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;
using Bee.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Ke.Bee.Localization.Localizer.Abstractions;
using Bee.Base.Abstractions.Navigation;
using Bee.Base.ViewModels;
using Bee.Base.Models.Menu;
using Bee.Base.ViewModels.Menus;

namespace Bee.ViewModels;

public partial class MainWindowViewModel : PageViewModelBase
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
    /// 侧栏二级菜单
    /// </summary>
    [ObservableProperty]
    private IDictionary<string, ObservableCollection<MenuItemViewModel>>? _sidebarMenus;
    /// <summary>
    /// 存储筛选前的二级子菜单数据
    /// </summary>
    private IDictionary<string, ObservableCollection<MenuItemViewModel>>? _originalSidebarMenus;
    /// <summary>
    /// 本地化资源
    /// </summary>
    private readonly ILocalizer _l;
    /// <summary>
    /// 菜单数据
    /// </summary>
    private readonly List<MenuItem> _menuItems;
    /// <summary>
    /// 防抖器
    /// </summary>
    private readonly Debounce _debounce;
    /// <summary>
    /// 视图导航器
    /// </summary>
    private readonly IViewNavigator _viewNavigator;
    /// <summary>
    /// 当前页面视图模型
    /// </summary>
    [ObservableProperty]
    private PageViewModelBase? _currentPage;
    public ToastrViewModel Toastr { get; }

    /// <summary>
    /// MenuItem 到 MenuItemViewModel 的转换器
    /// </summary>
    private Func<MenuItem, MenuItemViewModel> MenuItemToViewModel => x => new MenuItemViewModel(_l[x.LocaleKey])
    {
        Key = x.Key,
        IsActive = x.IsActive == true,
        Icon = string.IsNullOrWhiteSpace(x.Icon) ? null : StreamGeometry.Parse(x.Icon),
        CommandType = x.CommandType,
        CommandParameter = x.CommandParameter,
        Items = x.Items.Select(MenuItemToViewModel).ToList(),
        MenuClickCommand = GetRelayCommand(x.CommandType),
        Group = _l[x.GroupLocaleKey] // 将本地化后的值作为分组名称
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
                if (menuItem == null)
                {
                    return;
                }

                SetMenuActive(menuItem, () => ToolbarMenus?.FirstOrDefault(x => x.IsActive));

                // 激活工具栏菜单时载入二级菜单数据
                LoadSidebarMenus(menuItem);
            }),

            // 导航到视图中继命令
            MenuClickCommandType.Navigate => new RelayCommand<MenuItemViewModel>((MenuItemViewModel? menuItem) =>
            {
                SetMenuActive(menuItem, () => SidebarMenus?.Values.SelectMany(x => x)?.FirstOrDefault(x => x.IsActive));

                if (menuItem?.CommandParameter is null)
                {
                    return;
                }

                // 导航到视图
                _viewNavigator.NavigateTo(menuItem.CommandParameter);
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

                // 重载视图
                _viewNavigator.ReloadCurrentPage();
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

                // 重载视图，触发本地化更新
                _viewNavigator.ReloadCurrentPage();

                LoadToolbarMenus(true);
            }),

            // 返回 null
            _ => null
        };
    }

    public MainWindowViewModel(MenuConfigurationContext menuContext,
        ILocalizer localizer,
        IViewNavigator viewNavigator,
        ToastrViewModel toastr)
    {
        _l = localizer;
        _menuItems = menuContext.Menus;
        _debounce = new Debounce(500);
        _viewNavigator = viewNavigator;
        _viewNavigator.PropertyChanged += OnNavigatorPropertyChanged;
        LoadToolbarMenus();
        Toastr = toastr;
    }

    private void OnNavigatorPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(_viewNavigator.CurrentPage))
        {
            CurrentPage = _viewNavigator.CurrentPage;
        }
    }

    /// <summary>
    /// 载入工具栏菜单
    /// </summary>
    /// <param name="isReload">是否重载菜单（true: 保持当前激活菜单）</param>
    private void LoadToolbarMenus(bool isReload = false)
    {
        var toolbarMenus = _menuItems.Where(x => x.Group == "Toolbar").Select(MenuItemToViewModel);
        ToolbarMenus = [.. toolbarMenus];
        var settingMenus = _menuItems.Where(x => x.Group == "Settings").Select(MenuItemToViewModel);
        SettingMenus = [.. settingMenus];

        if (isReload)
        {
            var currentActiveToolbarMenu = ToolbarMenus?.FirstOrDefault(x => x.IsActive);
            var currentActiveSidebarMenu = SidebarMenus?.Values.SelectMany(x => x).FirstOrDefault(x => x.IsActive);
            LoadSidebarMenus(currentActiveToolbarMenu, currentActiveSidebarMenu);
        }
        else
        {
            LoadSidebarMenus(ToolbarMenus[0]);
        }
    }

    /// <summary>
    /// 载入侧栏二级菜单
    /// </summary>
    /// <param name="menuItem">当前激活菜单项</param>
    private void LoadSidebarMenus(MenuItemViewModel? menuItem, MenuItemViewModel? activeSidebarMenu = null)
    {
        // 找到激活菜单的子菜单
        var items = _menuItems.FirstOrDefault(x => x.Key == menuItem?.Key)?.Items;
        // 将子菜单分组 (分组键就是组名，分组值就是分组之后的菜单集合)
        _originalSidebarMenus = SidebarMenus = ParseGroupDictionary(items?.Select(MenuItemToViewModel));

        if (activeSidebarMenu != null)
        {
            var active = SidebarMenus?.Values.SelectMany(x => x).FirstOrDefault(x => x.Key.Equals(activeSidebarMenu.Key));
            if (active != null)
            {
                active.IsActive = true;
            }
            return;
        }

        // 激活首个菜单
        var first = SidebarMenus?.Values.FirstOrDefault()?.FirstOrDefault();
        if (first == null)
        {
            return;
        }

        first.IsActive = true;
        // 导航到视图
        if (!string.IsNullOrWhiteSpace(first.CommandParameter) &&
            Enum.TryParse(first.CommandType, true, out MenuClickCommandType commandType) &&
            commandType == MenuClickCommandType.Navigate)
        {
            _viewNavigator.NavigateTo(first.CommandParameter);
        }
    }

    /// <summary>
    /// 搜索查找菜单
    /// </summary>
    /// <param name="keywords"></param>
    public void OnSerachMenu(string? keywords)
    {
        // 防抖是指在一定时间范围内，只有最后一次操作才会被执行。如果在这段时间内再次触发操作，则重新计时。
        // KeyUp 事件会在每个按键弹起时执行，为了避免频繁触发事件。所以进行防抖处理
        _debounce.Trigger(() =>
        {
            // 筛选菜单。在国际化场景中，使用 InvariantCultureIgnoreCase，以确保跨文化的比较行为一致
            var filterdMenus = _originalSidebarMenus?.Values
                .SelectMany(x => x)
                .Where(x => x.Text.Contains(keywords ?? string.Empty, StringComparison.InvariantCultureIgnoreCase))
                ;

            SidebarMenus = ParseGroupDictionary(filterdMenus);
        });
    }

    /// <summary>
    /// 将菜单分组然后转换为字典类型
    /// </summary>
    /// <param name="menuItems"></param>
    /// <returns></returns>
    private IDictionary<string, ObservableCollection<MenuItemViewModel>>? ParseGroupDictionary(IEnumerable<MenuItemViewModel>? menuItems)
    {
        return menuItems?.GroupBy(x => x.Group).ToDictionary(
                x => x.Key ?? "UNGROUPED",
                x => new ObservableCollection<MenuItemViewModel>(x))
                ;
    }

    /// <summary>
    /// 设置激活菜单
    /// </summary>
    /// <param name="menuItem">要激活项</param>
    /// <param name="callBeforeActivedItem">之前激活项</param>
    private void SetMenuActive(MenuItemViewModel? menuItem, Func<MenuItemViewModel?> callBeforeActivedItem)
    {
        // 已经选中
        if (menuItem is null || menuItem.IsActive == true)
        {
            return;
        }

        // 清除之前选中项
        var beforeActived = callBeforeActivedItem();
        if (beforeActived != null)
        {
            beforeActived.IsActive = false;
        }

        menuItem.IsActive = true;
    }
}
