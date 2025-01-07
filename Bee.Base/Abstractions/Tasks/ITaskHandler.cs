using System.Collections.ObjectModel;

using Bee.Base.Models.Tasks;

using LanguageExt;

namespace Bee.Base.Abstractions.Tasks;

public delegate void MyDelegate(string message);

/// <summary>
/// 任务处理接口
/// </summary>
public interface ITaskHandler<in T> where T : TaskArgumentBase
{
    /// <summary>
    /// 任务列表处理逻辑
    /// </summary>
    /// <param name="taskItems">任务列表对象</param>
    /// <param name="arguments">任务参数</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <param name="onItemExecuted">单个任务处理完成后执行的回调方法</param>
    /// <returns></returns>
    Task ExecuteAsync(ObservableCollection<TaskItem> taskItems,
        T arguments,
        CancellationToken cancellationToken = default,
        Action<Fin<Unit>>? onItemExecuted = null)
        ;
    /// <summary>
    /// 执行任务，并返回执行结果
    /// </summary>
    /// <typeparam name="R">返回结果中数据的类型</typeparam>
    /// <param name="taskItem">任务项</param>
    /// <param name="arguments">执行任务时传递的参数</param>
    /// <param name="progressCallback">进度回调函数，用于通知任务进度。</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>返回一个执行结果对象</returns>
    Task<Fin<Unit>> ExecuteAsync(TaskItem taskItem, T arguments, Action<double> progressCallback, CancellationToken cancellationToken = default);
    /// <summary>
    /// 从输入路径创建任务列表
    /// </summary>
    /// <param name="inputPaths">输入路径集合</param>
    /// <param name="inputExtensions">允许的输入文件扩展</param>
    /// <returns></returns>
    Task<List<TaskItem>> CreateTasksFromInputPathsAsync(List<string> inputPaths, IEnumerable<string>? inputExtensions = null, T? arguments = null);
}