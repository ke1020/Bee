
using Bee.Base.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Base.Models.Task;

/// <summary>
/// 任务类页面视图模型基类
/// </summary>
public partial class TaskPageViewModelBase : PageViewModelBase
{
    /// <summary>
    /// 本地化资源
    /// </summary>
    protected ILocalizer L { get; }
    /// <summary>
    /// 任务状态文本
    /// </summary>
    [ObservableProperty]
    private string? _taskStatusText;

    public TaskPageViewModelBase(ILocalizer localizer)
    {
        L = localizer;
    }

    /// <summary>
    /// 设置任务准备状态
    /// </summary>
    /// <param name="tasksTotalCount">任务总数</param>
    protected void SetPendingStatus(int tasksTotalCount)
    {
        SetTaskStatusText(TaskStatusEnum.Pending, tasksTotalCount);
    }

    /// <summary>
    /// 设置运行中状态
    /// </summary>
    /// <param name="currentTaskIndex"></param>
    /// <param name="tasksTotalCount"></param>
    protected void SetRunningStatus(int currentTaskIndex, int tasksTotalCount)
    {
        SetTaskStatusText(TaskStatusEnum.Running, currentTaskIndex, tasksTotalCount);
    }

    /// <summary>
    /// 设置任务完成状态
    /// </summary>
    protected void SetCompletedStatus()
    {
        SetTaskStatusText(TaskStatusEnum.Completed);
    }

    private void SetTaskStatusText(TaskStatusEnum taskStatusEnum, params object[] argments)
    {
        TaskStatusText = taskStatusEnum switch
        {
            TaskStatusEnum.Pending => string.Format(L["Task.TaskPendingStatusText"], argments),
            TaskStatusEnum.Running => string.Format(L["Task.TaskRunningStatusText"], argments),
            TaskStatusEnum.Completed => L["Task.TaskCompletedStatusText"],
            _ => throw new InvalidTaskStatusException(nameof(taskStatusEnum))
        };
    }
}
