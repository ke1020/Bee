
namespace Bee.Base.Models.Navigation;

/// <summary>
/// 导航命令不存在异常
/// </summary>
public class NavigationCommandNotFoundException : ApplicationException
{
    public NavigationCommandNotFoundException() : base("Navigation command not found")
    {
    }

    public NavigationCommandNotFoundException(string message)
        : base(message) { }

    public NavigationCommandNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
