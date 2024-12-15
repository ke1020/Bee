namespace Bee.Base.Models.Tasks;

/// <summary>
/// 任务项状态
/// </summary>
public class TaskItemStatus
{
    /// <summary>
    /// 任务索引
    /// </summary>
    public int Index { get; set; }
    /// <summary>
    /// 是否完成
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
}