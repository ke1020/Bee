
namespace Bee.Base.Abstractions.Tasks;

/// <summary>
/// 任务封面处理器
/// </summary>
public interface ITaskCoverHandler
{
    /// <summary>
    /// 获取任务封面图像流
    /// </summary>
    /// <param name="imageSource">源图地址</param>
    /// <returns></returns>
    Task<Stream?> GetCoverAsync(string imageSource);
}