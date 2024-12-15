using Microsoft.Extensions.DependencyInjection;

namespace Bee.Base;

/// <summary>
/// 服务定位器
/// </summary>
public class ServiceLocator
{
    private static IServiceProvider? _current;

    public static IServiceProvider Current
    {
        get => _current!;
        set
        {
            if (_current != null)
            {
                throw new InvalidOperationException("The service locator has already been set.");
            }
            _current = value;
        }
    }

    public static T GetRequiredService<T>() where T : class
    {
        return (T)Current.GetRequiredService(typeof(T));
    }
}