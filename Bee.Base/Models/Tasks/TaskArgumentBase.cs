using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.Models.Tasks;

/// <summary>
/// 任务参数基类
/// </summary>
public class TaskArgumentBase : ObservableObject
{
    /// <summary>
    /// 输出目录
    /// </summary>
    public string OutputDirectory { get; set; } = string.Empty;
    /// <summary>
    /// 并发请求数
    /// </summary>
    public int MaxDegreeOfParallelism { get; set; } = 1;
}