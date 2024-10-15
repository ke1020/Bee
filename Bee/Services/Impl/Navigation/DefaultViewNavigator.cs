using System;
using System.Collections.Generic;
using System.ComponentModel;
using Bee.Models.Navigation;
using Bee.Services.Abstractions.Navigation;
using Bee.Services.Impl.Navigation.Commands;
using Bee.ViewModels;
using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Services.Impl.Navigation;

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
    /// 命令字典 （视图页面与导航命令对象之间的映射）
    /// </summary>
    private readonly IReadOnlyDictionary<ViewPages, INavigationCommand> _navigationCommands;
    /// <summary>
    /// 本地化资源对象
    /// </summary>
    private readonly ILocalizer _l;

    public DefaultViewNavigator(ILocalizer localizer)
    {
        _l = localizer;

        _navigationCommands = new Dictionary<ViewPages, INavigationCommand>
        {
            { ViewPages.PosterGenerator, new PosterGeneratorNavigationCommand()  },
            { ViewPages.DocumentConverter, new DocumentConverterNavigationCommand() }
        };
    }

    /// <summary>
    /// 导航到指定视图页面
    /// </summary>
    /// <param name="page"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void NavigateTo(ViewPages page)
    {
        if (!_navigationCommands.TryGetValue(page, out var command))
        {
            throw new ArgumentOutOfRangeException(nameof(page), page, _l["Errors.Invalid.ViewPage"]);
        }

        command.Execute(new NavigationCommandContext(this));
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
