

using System.Collections.ObjectModel;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

using Bee.Base.Models.Controls;

namespace Bee.Base.Controls;

/// <summary>
/// 扩展 Border 控件，增加两个伪类
/// </summary>
[PseudoClasses(":danger", ":success")]
public class ToastrBorder : Border
{
    public static readonly StyledProperty<ToastrType> ToastrTypeProperty =
        AvaloniaProperty.Register<Toastr, ToastrType>(nameof(ToastrType))
        ;

    public ToastrType ToastrType
    {
        get => GetValue(ToastrTypeProperty);
        set => SetValue(ToastrTypeProperty, value);
    }

    /// <summary>
    /// 加载完成后设置伪类
    /// </summary>
    /// <param name="e"></param>
    protected override void OnLoaded(RoutedEventArgs e)
    {
        base.OnLoaded(e);
        switch (ToastrType)
        {
            case ToastrType.Danger:
                PseudoClasses.Set(":danger", true);
                break;
            case ToastrType.Success:
                PseudoClasses.Set(":success", true);
                break;
        }
    }
}

/// <summary>
/// 自定义消息框控件
/// </summary>
public class Toastr : TemplatedControl
{
    /// <summary>
    /// 依赖属性 控制面板区域展开状态
    /// </summary>
    public static readonly StyledProperty<ObservableCollection<ToastrItem>> ItemsProperty =
        AvaloniaProperty.Register<Toastr, ObservableCollection<ToastrItem>>(nameof(Items))
        ;

    public ObservableCollection<ToastrItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }
}