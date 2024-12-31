
using System.Collections.ObjectModel;

using Bee.Base.Models.Controls;

using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.ViewModels;


public sealed partial class ToastrViewModel : ObservableObject
{
    private ObservableCollection<ToastrItem> _items = [];

    public ObservableCollection<ToastrItem> Items
    {
        get => _items;
        private set => SetProperty(ref _items, value);
    }
    private readonly HashSet<string> _shownMessages = []; // 用于防止重复消息

    /// <summary>
    /// 显示成功消息
    /// </summary>
    /// <param name="message"></param>
    public void ToastrSuccess(string message)
    {
        ShowToastr(message, ToastrType.Success);
    }

    /// <summary>
    /// 显示错误消息
    /// </summary>
    /// <param name="message"></param>
    public void ToastrError(string message)
    {
        ShowToastr(message, ToastrType.Danger);
    }

    private void ShowToastr(string message, ToastrType toastrType)
    {
        // 检查是否已经显示过该消息
        if (_shownMessages.Contains(message))
        {
            return;
        }

        _shownMessages.Add(message);

        var item = new ToastrItem(message, toastrType);
        item.RequestRemove += (sender, e) =>
        {
            if (sender is ToastrItem toItem && Items.Contains(toItem))
            {
                Items.Remove(toItem);
                _shownMessages.Remove(toItem.Message); // 移除已显示的消息
            }
        };

        Items.Add(item);
    }
}