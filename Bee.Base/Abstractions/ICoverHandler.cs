
namespace Bee.Base.Abstractions;

/// <summary>
/// 封面处理器
/// </summary>
public interface ICoverHandler
{
    /// <summary>
    /// 获取封面图像流
    /// </summary>
    /// <param name="imageSource">源图地址</param>
    /// <param name="width">宽度</param>
    /// <param name="height">宽度</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns></returns>
    Task<Stream> GetCoverAsync(string imageSource, uint? width, uint? height, CancellationToken cancellationToken = default);
}