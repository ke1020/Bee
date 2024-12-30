
using Bee.Base.Models.Plugin;

namespace Bee.Base.Abstractions.Plugin;

/// <summary>
/// 执行插件方法的处理接口
/// </summary>
/// <typeparam name="T">方法请求参数数据类型</typeparam>
public interface IPluginMethodHandler<T>
{
    /// <summary>
    /// 执行方法，并返回结果对象
    /// </summary>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result> ExecuteAsync(T? parameters);
}

/// <summary>
/// 执行插件方法的处理接口
/// </summary>
/// <typeparam name="T">方法请求参数数据类型</typeparam>
/// <typeparam name="D">方法返回对象的 Data 属性数据类型</typeparam>
public interface IPluginMethodHandler<T, D>
{
    /// <summary>
    /// 执行方法，并返回带 R 类型数据的结果对象
    /// </summary>
    /// <typeparam name="D"></typeparam>
    /// <param name="parameters"></param>
    /// <returns></returns>
    Task<Result<D>> ExecuteAsync(T? parameters);
}
