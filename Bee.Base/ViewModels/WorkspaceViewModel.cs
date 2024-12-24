using Avalonia.Controls;

using Bee.Base.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Base.ViewModels;

/// <summary>
/// 工作区视图模型
/// </summary>
/// <param name="serviceProvider">服务提供者</param>
/// <param name="l">本地化资源对象</param>
public partial class WorkspaceViewModel(IServiceProvider serviceProvider, ILocalizer l) : PageViewModelBase
{
    
    protected IServiceProvider ServiceProvider { get; } = serviceProvider;
    protected ILocalizer L { get; } = l;
    /// <summary>
    /// 派生类中重写该属性设置 Tab 项
    /// </summary>
    protected virtual List<TabMetadata> TabList { get; } = [];
    /// <summary>
    /// 派生类中重写该属性设置默认选中项
    /// </summary>
    protected virtual int SelectedTabIndex { get; } = 0;
    /// <summary>
    /// 伸缩面板是否展开状态
    /// </summary>
    [ObservableProperty]
    private bool _isPaneOpen = false;
    /// <summary>
    /// 选中 Tab 项
    /// </summary>
    private TabItem? _selectedTab;
    /// <summary>
    /// 选中项
    /// </summary>
    public TabItem SelectedTab
    {
        get => _selectedTab ?? Tabs[SelectedTabIndex];
        set
        {
            InitialTab(value, TabList[value.TabIndex]);
            SetProperty(ref _selectedTab, value);
        }
    }
    /// <summary>
    /// 标签页
    /// </summary>
    public List<TabItem> Tabs => TabList.Select((x, i) => new TabItem { TabIndex = i, Header = L[x.LocalKey] }).ToList();

    /// <summary>
    /// 切换伸缩面板展开与收起状态的命令
    /// </summary>
    [RelayCommand]
    private void PaneToggle()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    /// <summary>
    /// 初始化标签页
    /// </summary>
    /// <param name="tabItem">标签对象</param>
    /// <param name="tabMetadata">标签元数据信息</param>
    private void InitialTab(TabItem tabItem, TabMetadata tabMetadata)
    {
        if (ServiceProvider.GetService(tabMetadata.ViewType) is not UserControl view)
        {
            return;
        }

        var vm = ServiceProvider.GetService(tabMetadata.ViewModelType);
        tabItem.DataContext = vm;
        view.DataContext = vm;
        tabItem.Content = view;
    }
}