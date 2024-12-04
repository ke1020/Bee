
namespace Bee.Base.Models.Plugin;

/// <summary>
/// 调用插件方法返回的结果对象
/// </summary>
public class PluginResult(bool ok)
{
    public bool? OK { get; set; } = ok;

    public static PluginResult SetOk(bool ok)
    {
        return new PluginResult(ok);
    }
}

/// <summary>
/// 调用插件方法返回的结果对象
/// </summary>
/// <typeparam name="T">对象 Data 属性的数据类型</typeparam>
public class PluginResult<T> : PluginResult
{
    /// <summary>
    /// 执行插件方法返回的数据
    /// </summary>
    public T? Data { get; set; }

    public PluginResult(bool ok) : base(ok)
    {
    }

    public PluginResult(bool ok, T data) : base(ok)
    {
        Data = data;
    }
}
