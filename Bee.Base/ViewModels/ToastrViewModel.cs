
using System.Collections.ObjectModel;

using Avalonia.Threading;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.ViewModels;

public partial class ToastrItem : ObservableObject, IDisposable
{
    [ObservableProperty]
    private bool _isVisible = true;
    public string Message { get; }

    private DispatcherTimer? _timer;

    public event EventHandler? RequestRemove;

    public ToastrItem(string message)
    {
        IsVisible = true;
        Message = message;

        // 使用 DispatcherTimer 确保回调在主线程上执行
        _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(2000), DispatcherPriority.Normal, async (s, e) =>
        {
            IsVisible = false;
            await Task.Delay(500); // 确保在主线程上等待
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

public sealed partial class ToastrViewModel : ObservableObject
{
    private ObservableCollection<ToastrItem> _items = [];

    public ObservableCollection<ToastrItem> Items
    {
        get => _items;
        private set => SetProperty(ref _items, value);
    }
    private readonly HashSet<string> _shownMessages = []; // 用于防止重复消息

    public void ShowToastr(string message)
    {
        // 检查是否已经显示过该消息
        if (_shownMessages.Contains(message))
        {
            return;
        }

        _shownMessages.Add(message);

        var item = new ToastrItem(message);
        item.RequestRemove += (sender, e) =>
        {
            var toItem = sender as ToastrItem;
            if (toItem != null && Items.Contains(toItem))
            {
                Items.Remove(toItem);
                _shownMessages.Remove(toItem.Message); // 移除已显示的消息
            }
        };

        Items.Add(item);
    }
}