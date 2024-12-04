
namespace Bee.Base.Models.Plugin;

public class PluginNotFoundException : ApplicationException
{
    public PluginNotFoundException() : base("插件不存在")
    {
    }

    public PluginNotFoundException(string message)
        : base(message) { }

    public PluginNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
