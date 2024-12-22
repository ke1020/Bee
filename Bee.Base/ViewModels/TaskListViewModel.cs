using System.Collections.ObjectModel;
using System.Diagnostics;

using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;

using Bee.Base.Abstractions.Tasks;
using Bee.Base.Models;
using Bee.Base.Models.Tasks;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Ke.Bee.Localization.Localizer.Abstractions;

using Microsoft.Extensions.Options;

namespace Bee.Base.ViewModels;

/// <summary>
/// 任务列表视图模型
/// </summary>
public sealed partial class TaskListViewModel<T> : ObservableObject, ITaskListViewModel<T> where T : TaskArgumentBase, new()
{
    /// <summary>
    /// 可用的输入文件类型 （参数面板中可选的文件类型）
    /// </summary>
    private IEnumerable<string> _inputExtensions = ["jpg", "png", "bmp", "webp", "gif", "zip", "7z", "rar", "tar", "tar.gz"];
    /// <summary>
    /// 可用的输入文件类型 （参数面板中可选的文件类型）
    /// </summary>
    public IEnumerable<string> InputExtensions
    {
        get => _inputExtensions;
        private set => SetProperty(ref _inputExtensions, value);
    }

    /// <summary>
    /// 任务状态文本
    /// </summary>
    private string? _taskStatusText;
    /// <summary>
    /// 任务状态文本
    /// </summary>
    public string? TaskStatusText
    {
        get => _taskStatusText;
        private set => SetProperty(ref _taskStatusText, value);
    }

    /// <summary>
    /// 任务列表
    /// </summary>
    private ObservableCollection<TaskItem> _taskItems = [];
    /// <summary>
    /// 任务列表
    /// </summary>
    public ObservableCollection<TaskItem> TaskItems
    {
        get => _taskItems;
        private set => SetProperty(ref _taskItems, value);
    }

    /// <summary>
    /// 任务参数对象
    /// </summary>
    private T? _taskArguments;
    /// <summary>
    /// 任务参数对象
    /// </summary>
    public T? TaskArguments
    {
        get => _taskArguments;
        private set => SetProperty(ref _taskArguments, value);
    }

    private string? _viewComment;
    /// <summary>
    /// 页面功能说明
    /// </summary>
    public string ViewComment
    {
        get => _viewComment ?? string.Empty;
        private set => SetProperty(ref _viewComment, value);
    }

    /// <summary>
    /// 本地化资源
    /// </summary>
    private readonly ILocalizer _l;
    /// <summary>
    /// 任务处理接口
    /// </summary>
    private readonly ITaskHandler<T> _taskHandler;
    /// <summary>
    /// 任务封面图处理接口
    /// </summary>
    private readonly ITaskCoverHandler? _taskCoverHandler;
    /// <summary>
    /// 应用配置
    /// </summary>
    private readonly AppSettings _appSettings;
    /// <summary>
    /// 取消任务的取消令牌
    /// </summary>
    private CancellationTokenSource? _cancellationTokenSource;
    /// <summary>
    /// 当前正在处理的任务
    /// </summary>
    private Task? _currentTask;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="appSettings">全局设置对象</param>
    /// <param name="localizer">本地化</param>
    /// <param name="taskHandler">任务处理器</param>
    /// <param name="taskCoverHandler">封面图片获取器</param>
    public TaskListViewModel(IOptions<AppSettings> appSettings,
        ILocalizer localizer,
        ITaskHandler<T> taskHandler,
        ITaskCoverHandler? taskCoverHandler = null)
    {
        _l = localizer;
        _appSettings = appSettings.Value;
        _taskHandler = taskHandler;
        _taskCoverHandler = taskCoverHandler;
        // _taskArguments = new T();
    }

    /// <summary>
    /// 设置任务准备状态
    /// </summary>
    /// <param name="tasksTotalCount">任务总数</param>
    private void SetPendingStatus(int tasksTotalCount)
    {
        SetTaskStatusText(TaskStatusEnum.Pending, tasksTotalCount);
    }

    /// <summary>
    /// 设置运行中状态
    /// </summary>
    /// <param name="completedCount">已完成</param>
    /// <param name="tasksTotalCount"></param>
    private void SetRunningStatus(int completedCount, int tasksTotalCount)
    {
        SetTaskStatusText(TaskStatusEnum.Running, completedCount, tasksTotalCount);
    }

    /// <summary>
    /// 设置任务完成状态
    /// </summary>
    private void SetCompletedStatus()
    {
        SetTaskStatusText(TaskStatusEnum.Completed);
    }

    /// <summary>
    /// 设置任务状态
    /// </summary>
    /// <param name="taskStatusEnum"></param>
    /// <param name="argments"></param>
    /// <exception cref="InvalidTaskStatusException"></exception>
    private void SetTaskStatusText(TaskStatusEnum taskStatusEnum, params object[] argments)
    {
        TaskStatusText = taskStatusEnum switch
        {
            TaskStatusEnum.Pending => string.Format(_l["Task.TaskPendingStatusText"], argments),
            TaskStatusEnum.Running => string.Format(_l["Task.TaskRunningStatusText"], argments),
            TaskStatusEnum.Completed => _l["Task.TaskCompletedStatusText"],
            _ => throw new InvalidTaskStatusException(nameof(taskStatusEnum))
        };
    }

    /// <summary>
    /// 从任务列表移除任务
    /// </summary>
    /// <param name="item"></param>
    public void RemoveTaskItem(TaskItem item)
    {
        TaskItems.Remove(item);
        SetPendingStatus(TaskItems.Count);
    }

    /// <summary>
    /// 初始化任务参数对象
    /// </summary>
    public void InitialArguments(string outputPath)
    {
        TaskArguments = new T
        {
            OutputDirectory = Path.Combine(_appSettings.OutputPath, outputPath)
        };
    }

    /// <summary>
    /// 设置输入扩展
    /// </summary>
    /// <param name="inputExtensions"></param>
    public void SetInputExtensions(IEnumerable<string> inputExtensions)
    {
        InputExtensions = inputExtensions;
    }

    /// <summary>
    /// 设置页面功能说明
    /// </summary>
    /// <param name="viewComment"></param>
    public void SetViewComment(string viewComment)
    {
        ViewComment = viewComment;
    }

    /// <summary>
    /// 拖放文件事件
    /// </summary>
    /// <param name="storageItems"></param>
    /// <returns></returns>
    public async Task OnDragOver(IEnumerable<IStorageItem>? storageItems)
    {
        if (storageItems == null)
        {
            return;
        }

        // 存放选择的文件路径
        var paths = new List<string>();
        foreach (var item in storageItems)
        {
            using (item)
            {
                paths.Add(item.Path.LocalPath);
            }
        }

        // 为了避免操作阻塞 UI，在后台执行耗时操作
        await Task.Run(async () =>
        {
            TaskItems.Merge([.. await GetTasksAsync(paths, InputExtensions.Select(x => x.StartsWith('.') ? x : $".{x}"))]);
            SetPendingStatus(TaskItems.Count);
        });

        // 如果在后台线程中执行了操作之后需要更新 UI，请确保通过 Dispatcher.UIThread.InvokeAsync 在 UI 线程中执行更新操作
        // await Dispatcher.UIThread.InvokeAsync(() =>
        // {

        // });
    }

    /// <summary>
    /// 执行按钮事件
    /// </summary>
    /// <returns></returns>
    [RelayCommand]
    private async Task Execute()
    {
        // 任务列表总数
        var taskListCount = TaskItems.Count;
        if (taskListCount == 0)
        {
            return;
        }

        // 如果已经有任务在运行，则先取消它
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel();
            // 等待现有任务完成
            await _currentTask;
        }

        // 重置令牌
        _cancellationTokenSource = new CancellationTokenSource();

        // 为了避免操作阻塞 UI，在后台执行耗时操作
        _currentTask = Task.Run(async () =>
        {
            // 创建 ParallelOptions 并设置 CancellationToken
            var parallelOptions = new ParallelOptions
            {
                CancellationToken = _cancellationTokenSource.Token,
                MaxDegreeOfParallelism = TaskArguments?.MaxDegreeOfParallelism ?? 1 // 设置最大并行度
            };

            try
            {
                // 开始并行处理...
                await Parallel.ForEachAsync(TaskItems, parallelOptions, async (taskItem, token) =>
                    {
                        // 检查是否应该取消
                        token.ThrowIfCancellationRequested();

                        // 任务是已完成状态
                        if (taskItem.IsCompleted)
                        {
                            return;
                        }

                        // await Task.Delay(300, token); // 模拟异步工作

                        await _taskHandler.ExecuteAsync(taskItem, TaskArguments, (percent) =>
                        {
                            taskItem.Percent = percent; // 设置任务进度
                            taskItem.IsCompleted = percent == 100; // 设置完成状态
                        }, token);

                        // 设置已完成数量
                        SetRunningStatus(TaskItems.Count(x => x.IsCompleted), taskListCount);
                    })
                    ;

                // 所有项处理完毕
                SetCompletedStatus();
            }
            catch (OperationCanceledException)
            {
                // 处理任务取消
                Console.WriteLine("任务已取消");
            }
            catch (Exception ex)
            {
                // 处理其他异常
                Console.WriteLine($"任务执行失败: {ex.Message}");
            }
            finally
            {
                // 释放资源
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
            }
        });
    }

    /// <summary>
    /// 清除任务按钮事件
    /// </summary>
    [RelayCommand]
    private void ClearTaskItems()
    {
        // 主动释放资源
        if (TaskItems != null)
        {
            foreach (var item in TaskItems)
            {
                item.Source?.Dispose();
            }
            TaskItems = [];
        }

        // 重置任务状态为初始状态
        SetPendingStatus(0);
    }

    /// <summary>
    /// 打开输出目录按钮事件
    /// </summary>
    [RelayCommand]
    private void OpenOutputDirectory()
    {
        if (!Directory.Exists(TaskArguments?.OutputDirectory))
        {
            return;
        }

        try
        {
            // 使用 Process.Start 方法打开文件夹
            Process.Start(new ProcessStartInfo
            {
                FileName = TaskArguments.OutputDirectory,
                UseShellExecute = true // 必须设置为 true，否则某些情况下可能无法正常工作
            });
        }
        catch (Exception ex)
        {

        }
    }

    /// <summary>
    /// 停止任务
    /// </summary>
    [RelayCommand]
    private void Stop()
    {
        if (_currentTask != null && !_currentTask.IsCompleted)
        {
            _cancellationTokenSource?.Cancel(); // 发出取消请求
        }
    }

    /// <summary>
    /// 获取任务对象集合
    /// </summary>
    /// <param name="inputPaths">输入路径集合</param>
    /// <param name="inputExtensions">输入扩展名集合</param>
    /// <returns></returns>
    private async Task<List<TaskItem>> GetTasksAsync(IList<string> inputPaths, IEnumerable<string> inputExtensions)
    {
        if (inputPaths == null)
        {
            return [];
        }

        var fileSources = GetFiles([.. inputPaths], inputExtensions);
        var files = new List<TaskItem>();
        if (fileSources != null)
        {
            foreach (var file in fileSources)
            {
                Stream stream;
                // 如果提供了封面处理器，使用封面处理器获取封面图片
                if (_taskCoverHandler != null)
                {
                    stream = await _taskCoverHandler.GetCoverAsync(file) ?? AssetLoader.Open(new Uri(_appSettings.DefaultTaskImageUri));
                }
                else
                {
                    // 没有提供则从配置文件获取封面图片
                    stream = AssetLoader.Open(new Uri(_appSettings.DefaultTaskImageUri));
                }

                using (stream)
                {
                    files.Add(new TaskItem
                    {
                        // 任务封面图片
                        Source = new Bitmap(stream),
                        // 任务名
                        Name = Path.GetFileName(file),
                        // 文件地址
                        FileName = file
                    });
                }
            }
        }
        return files;
    }

    /// <summary>
    /// 从输入的文件路径集合中，获取指定扩展名的文件
    /// </summary>
    /// <param name="inputPaths">输入路径数组</param>
    /// <returns>文件地址集合</returns>
    private List<string> GetFiles(string[] inputPaths, IEnumerable<string> inputExtensions)
    {
        var files = new List<string>();
        foreach (var path in inputPaths)
        {
            // 如果是路径
            if (Directory.Exists(path))
            {
                // 查找目录及子目录下所有指定 Extensions 扩展名的文件
                files.AddRange(Directory.GetFiles(path, "*.*", SearchOption.AllDirectories)
                    .Where(file => inputExtensions?.Any(ext => file.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) == true))
                    ;
            }
            else if (File.Exists(path))
            {
                // 如果是查找的扩展类型
                if (inputExtensions?.Any(ext => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase)) == true)
                {
                    files.Add(path);
                }
            }
        }
        return files;
    }
}