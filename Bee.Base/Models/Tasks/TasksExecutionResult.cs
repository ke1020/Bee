namespace Bee.Base.Models.Tasks;

/// <summary>
/// 任务执行结果对象
/// </summary>
public class TasksExecutionResult
{
    /// <summary>
    /// 任务列表是否全部成功
    /// </summary>
    public bool TotalSucceeded { get; set; }
    /// <summary>
    /// 错误任务集合
    /// </summary>
    public ICollection<TaskItemStatus> Errors { get; set; } = [];
}