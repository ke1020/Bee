
using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.Models.Controls;

/// <summary>
/// 消息提示项
/// </summary>
public partial class ToastrItem : ObservableObject, IDisposable
{
    [ObservableProperty]
    private bool _isVisible = true;
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Message { get; }
    /// <summary>
    /// 消息类型
    /// </summary>
    public ToastrType ToastrType { get; }
    private DispatcherTimer? _timer;

    public event EventHandler? RequestRemove;

    public ToastrItem(string message, ToastrType toastrType = ToastrType.Danger, int duration = 3000)
    {
        IsVisible = true;
        Message = message;
        ToastrType = toastrType;

        // 使用 DispatcherTimer 确保回调在主线程上执行
        _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(duration), DispatcherPriority.Normal, (s, e) =>
        {
            IsVisible = false;
            RequestRemove?.Invoke(this, EventArgs.Empty);
            Dispose(); // 自动调用 Dispose 释放资源
        });

        _timer.Start();
    }

    public void Dispose()
    {
        _timer?.Stop();
        _timer = null;
    }
}