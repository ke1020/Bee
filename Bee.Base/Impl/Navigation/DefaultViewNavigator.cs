using System.ComponentModel;
using Bee.Base.Abstractions.Navigation;
using Bee.Base.Helpers;
using Bee.Base.Models.Navigation;
using Bee.Base.ViewModels;
using Ke.Bee.Localization.Localizer.Abstractions;

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
    /// <summary>
    /// 本地化资源对象
    /// </summary>
    private readonly ILocalizer _l;

    public DefaultViewNavigator(ILocalizer localizer, IEnumerable<INavigationCommand> commands)
    {
        _l = localizer;
        _commands = commands;
    }

    /// <summary>
    /// 导航到指定视图页面
    /// </summary>
    /// <param name="key"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void NavigateTo(string key)
    {
        ArgumentExceptionHelper.ThrowIfNullOrEmpty(key);

        var command = _commands.FirstOrDefault(x => x.Key == key);

        ArgumentExceptionHelper.ThrowIfNavigationCommandNull(command, key);

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
}
