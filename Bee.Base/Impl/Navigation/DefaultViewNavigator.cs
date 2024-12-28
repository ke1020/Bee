using System.ComponentModel;

using Bee.Base.Abstractions.Navigation;
using Bee.Base.Models.Navigation;
using Bee.Base.ViewModels;

namespace Bee.Base.Impl.Navigation;

/// <summary>
/// 默认视图导航器
/// </summary>
public class DefaultViewNavigator : IViewNavigator
{
    /// <summary>
    /// 当前页视图模型
    /// </summary>
    public PageViewModelBase? CurrentPage { get; private set; }
    /// <summary>
    /// 属性变化通知事件
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
    /// <summary>
    /// 导航命令集合
    /// </summary>
    private readonly IEnumerable<INavigationCommand> _commands;

    private readonly IServiceProvider _serviceProvider;

    public DefaultViewNavigator(IEnumerable<INavigationCommand> commands, IServiceProvider serviceProvider)
    {
        _commands = commands;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 导航到指定视图页面
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void NavigateTo(string key)
    {
        ArgumentNullException.ThrowIfNull(key);

        var command = _commands.FirstOrDefault(x => x.Key == key);

        ArgumentNullException.ThrowIfNull(command);
        command?.Execute(new NavigationCommandContext(this));
    }

    /// <summary>
    /// 设置当前视图模型
    /// </summary>
    /// <param name="page"></param>
    public void SetCurrentPage(PageViewModelBase page)
    {

        CurrentPage = page;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentPage)));
    }

    /// <summary>
    /// 重载视图
    /// </summary>
    public void ReloadCurrentPage()
    {
        if (CurrentPage == null)
        {
            return;
        }

        // 使用 DI 容器创建实例 (因为视图模型都是以瞬态模式注入，所以获取的实例都是新实例)
        if (_serviceProvider.GetService(CurrentPage.GetType()) is PageViewModelBase newPage)
        {
            SetCurrentPage(newPage);
        }
    }
}
