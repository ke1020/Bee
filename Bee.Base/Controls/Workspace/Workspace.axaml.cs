
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Bee.Base.Controls;

public class Workspace : TemplatedControl
{
    /// <summary>
    /// 依赖属性 控制面板区域展开状态
    /// </summary>
    public static readonly StyledProperty<bool> IsPaneOpenProperty =
        AvaloniaProperty.Register<Workspace, bool>(nameof(IsPaneOpen))
        ;

    public bool IsPaneOpen
    {
        get => GetValue(IsPaneOpenProperty);
        set => SetValue(IsPaneOpenProperty, value);
    }

    /// <summary>
    /// 标题
    /// </summary>
    public static readonly StyledProperty<string> TitleProperty =
        AvaloniaProperty.Register<Workspace, string>(nameof(Title));

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 面板展开与折叠命令
    /// </summary>
    public static readonly StyledProperty<ICommand> PaneToggleCommandProperty =
        AvaloniaProperty.Register<Workspace, ICommand>(nameof(PaneToggleCommand));

    public ICommand PaneToggleCommand
    {
        get => GetValue(PaneToggleCommandProperty);
        set => SetValue(PaneToggleCommandProperty, value);
    }

    /// <summary>
    /// Defines the WorkContent property.
    /// </summary>
    public static readonly StyledProperty<object> ContentProperty =
        AvaloniaProperty.Register<Workspace, object>(nameof(Content));

    /// <summary>
    /// Gets or sets the content of the work area.
    /// </summary>
    public object Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// Defines the WorkPaneContent property.
    /// </summary>
    public static readonly StyledProperty<object> PaneProperty =
        AvaloniaProperty.Register<Workspace, object>(nameof(Pane));

    /// <summary>
    /// Gets or sets the content of the pane area.
    /// </summary>
    public object Pane
    {
        get => GetValue(PaneProperty);
        set => SetValue(PaneProperty, value);
    }
}