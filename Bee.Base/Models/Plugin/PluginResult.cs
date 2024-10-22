
namespace Bee.Base.Models.Plugin;

/// <summary>
/// 插件结果对象
/// </summary>
public class PluginResult(bool ok)
{
    public bool? OK { get; set; } = ok;

    public static PluginResult SetOk(bool ok)
    {
        return new PluginResult(ok);
    }
}
