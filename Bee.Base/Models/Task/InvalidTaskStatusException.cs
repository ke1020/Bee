
namespace Bee.Base.Models.Task;

public class InvalidTaskStatusException : ApplicationException
{
    public InvalidTaskStatusException() : base("错误的任务状态")
    {
    }

    public InvalidTaskStatusException(string message)
        : base(message) { }

    public InvalidTaskStatusException(string message, Exception innerException)
        : base(message, innerException) { }
}
