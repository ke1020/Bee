
namespace Bee.Base.Models;

/// <summary>
/// 应用异常基类
/// </summary>
public class ApplicationException : Exception
{
    public ApplicationException() { }

    public ApplicationException(string message)
        : base(message) { }

    public ApplicationException(string message, Exception innerException)
        : base(message, innerException) { }
}
