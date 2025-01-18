using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Bee.Base.Controls;

/// <summary>
/// 进度条控件
/// </summary>
public class Progress1 : TemplatedControl
{
    /// <summary>
    /// 依赖属性
    /// </summary>
    public static readonly StyledProperty<double> RadiusProperty =
        AvaloniaProperty.Register<Progress1, double>(nameof(Radius), 2.5)
        ;

    public double Radius
    {
        get => GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<Progress1, TimeSpan>(nameof(Duration), TimeSpan.FromSeconds(2))
        ;

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty); // 0:0:2
        set => SetValue(DurationProperty, value);
    }

    public static readonly StyledProperty<uint> ScrollWidthProperty =
        AvaloniaProperty.Register<Progress1, uint>(nameof(ScrollWidth))
        ;

    public uint ScrollWidth
    {
        get => GetValue(ScrollWidthProperty);
        set => SetValue(ScrollWidthProperty, value);
    }

    public static readonly StyledProperty<int> ScrollLeftProperty =
        AvaloniaProperty.Register<Progress1, int>(nameof(ScrollLeft))
        ;

    public int ScrollLeft
    {
        get => GetValue(ScrollLeftProperty);
        set => SetValue(ScrollLeftProperty, value);
    }

    public Progress1()
    {
        ScrollWidth = 100;
        //var slot = this.FindControl<RectangleGeometry>("");
        // Avalonia.Media.RectangleGeometry
        //Slot. Rect="0,0,500,5"

    }
}