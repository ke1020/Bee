
namespace Bee.Base.Models.Plugin;

public class PluginMethodNotFoundException: ApplicationException
{
    public PluginMethodNotFoundException() : base("插件方法不存在")
    {
    }

    public PluginMethodNotFoundException(string message)
        : base(message) { }

    public PluginMethodNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }

}
