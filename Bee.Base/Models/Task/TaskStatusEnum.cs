
namespace Bee.Base.Models.Task;

/// <summary>
/// 任务状态枚举对象
/// </summary>
public enum TaskStatusEnum
{
    None = 0,
    /// <summary>
    /// 准备
    /// </summary>
    Pending,
    /// <summary>
    /// 执行中
    /// </summary>
    Running,
    /// <summary>
    /// 完成
    /// </summary>
    Completed,
}
