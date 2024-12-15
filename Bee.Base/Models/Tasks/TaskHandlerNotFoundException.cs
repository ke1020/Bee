
namespace Bee.Base.Models.Tasks;

public class TaskHandlerNotFoundException : ApplicationException
{
    public TaskHandlerNotFoundException() : base("任务处理器不存在")
    {
    }

    public TaskHandlerNotFoundException(string message)
        : base(message) { }

    public TaskHandlerNotFoundException(string message, Exception innerException)
        : base(message, innerException) { }
}
