
using System.Runtime.CompilerServices;
using Bee.Base.Abstractions.Navigation;
using Bee.Base.Models.Navigation;

namespace Bee.Base.Helpers;

/// <summary>
/// 参数异常助手类
/// </summary>
[Obsolete("不再使用")]
public class ArgumentExceptionHelper
{
    public static void ThrowIfNullOrEmpty(string? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(paramName);
    }

    public static void ThrowIfOutOfRange(bool argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument)
            throw new ArgumentOutOfRangeException(paramName);
    }

    public static void ThrowIfNavigationCommandNull(INavigationCommand? command, [CallerArgumentExpression(nameof(command))] string? paramName = null)
    {
        if (command is null)
            throw new NavigationCommandNotFoundException(paramName!);
    }
}
