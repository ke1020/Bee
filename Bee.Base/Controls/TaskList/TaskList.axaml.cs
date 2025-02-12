using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;

using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models.Tasks;

namespace Bee.Base.Controls;

public partial class TaskList : UserControl
{
    // 定义一个 StyledProperty
    public static readonly StyledProperty<string> ViewCommentProperty =
        AvaloniaProperty.Register<TaskList, string>(nameof(ViewComment), "Comment");

    // .NET 属性包装器
    public string ViewComment
    {
        get => GetValue(ViewCommentProperty);
        set => SetValue(ViewCommentProperty, value);
    }

    public TaskList()
    {
        InitializeComponent();

        // 添加拖放事件处理
        AddHandler(DragDrop.DropEvent, async (object? sender, DragEventArgs e) =>
        {
            await ((ITaskListViewModel<TaskArgumentBase>)DataContext!).OnDragOver(e.Data.GetFiles());
        });
    }

    /// <summary>
    /// 任务列表删除图标按下事件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RemoveItemPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not PathIcon pathIcon)
        {
            return;
        }

        if (pathIcon.DataContext is not TaskItem item)
        {
            return;
        }

        (DataContext as ITaskListViewModel<TaskArgumentBase>)?.RemoveTaskItem(item);
    }

    private void OnDrapDropAreaPressed(object? sender, PointerPressedEventArgs e)
    {
        /*
        var storageProvider = TopLevel.GetTopLevel(this)?.StorageProvider;
        if (storageProvider is null)
        {
            return;
        }

        await storageProvider.OpenFolderPickerAsync();
        var files = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            AllowMultiple = true,
            Title = "Select Files",
            // FileTypeFilter = [FilePickerFileTypes.ImageAll]
        });
        */
    }

    private void OnEditItemPressed(object? sender, PointerPressedEventArgs e)
    {
        if (sender is not PathIcon pathIcon)
        {
            return;
        }

        if (pathIcon.DataContext is not TaskItem item)
        {
            return;
        }

        // (DataContext as ITaskListViewModel<TaskArgumentBase>)?.Configure(item);

        // 直接使用扩展方法查找 StackPanel 祖先
        //var grid = pathIcon.FindAncestorOfType<Grid>().FindAncestorOfType<Grid>()!;
        //var scrollViewer = (ScrollViewer)grid.Parent!;
        //var shownTextBox = scrollViewer.FindControl<TextBox>("");

        (DataContext as ITaskListViewModel<TaskArgumentBase>)?.ConfigureTaskItem(item);
    }
}