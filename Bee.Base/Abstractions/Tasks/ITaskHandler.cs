using Bee.Base.Models.Tasks;

namespace Bee.Base.Abstractions.Tasks;

/// <summary>
/// 任务处理接口
/// </summary>
public interface ITaskHandler<in T> where T : TaskArgumentBase
{
    /// <summary>
    /// 执行任务，并返回执行结果
    /// </summary>
    /// <param name="taskItem">任务项</param>
    /// <param name="arguments">执行任务时传递的参数</param>
    /// <param name="progressCallback">进度回调函数，用于通知任务进度。</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>true:成功，false：失败</returns>
    Task<bool> ExecuteAsync(TaskItem taskItem, T? arguments, Action<double> progressCallback, CancellationToken cancellationToken = default);
}