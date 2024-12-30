using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Bee.ViewModels;

namespace Bee.Views;

public partial class MainWindow : Window
{
    public double OrignalWidth { get; private set; }
    public double OrignalHeight { get; private set; }

    // public int X { get; private set; }
    // public int Y { get; private set; }

    public MainWindow()
    {
        InitializeComponent();

        // 记录窗口原始尺寸
        OrignalWidth = Width;
        OrignalHeight = Height;

        this.GetObservable(WindowStateProperty).Subscribe(new WindowStateObserver(this));

        /*
        // 监听窗口位置变化
        PositionChanged += (sender, e) =>
        {
            if (WindowState == WindowState.Normal)
            {
                X = e.Point.X;
                Y = e.Point.Y;
            }
        };
        */
    }

    protected override void OnResized(WindowResizedEventArgs e)
    {
        // 用户调整窗口大小之后，记录新的窗口尺寸
        if (e.Reason == WindowResizeReason.User && WindowState == WindowState.Normal)
        {
            OrignalWidth = e.ClientSize.Width;
            OrignalHeight = e.ClientSize.Height;
        }

        base.OnResized(e);
    }

    /// <summary>
    /// 搜索菜单输入框键盘抬起事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SerachMenuKeyUp(object sender, KeyEventArgs e)
    {
        ((MainWindowViewModel)DataContext!).OnSerachMenu(((TextBox)sender).Text);
    }
}

/// <summary>
/// 窗口状态观察者
/// </summary>
/// <param name="window"></param>
internal class WindowStateObserver(MainWindow window) : IObserver<WindowState>
{
    private readonly MainWindow _window = window;

    public void OnCompleted() { }

    public void OnError(Exception error) { }

    public void OnNext(WindowState value)
    {
        if (value == WindowState.Maximized)
        {
            _window.Width = _window.OrignalWidth;
            _window.Height = _window.OrignalHeight;
        }
    }
}