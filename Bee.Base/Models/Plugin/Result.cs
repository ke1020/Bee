
namespace Bee.Base.Models.Plugin;

/// <summary>
/// 调用插件方法返回的结果对象
/// </summary>
public class Result(bool ok, string? message = null)
{
    public bool OK { get; set; } = ok;

    public string? Message { get; set; } = message;

    public static Result Success(string? message = null)
    {
        return new Result(true, message);
    }

    public static Result Fail(string? message = null)
    {
        return new Result(false, message);
    }
}

/// <summary>
/// 调用插件方法返回的结果对象
/// </summary>
/// <typeparam name="T">对象 Data 属性的数据类型</typeparam>
public class Result<T> : Result
{
    /// <summary>
    /// 执行插件方法返回的数据
    /// </summary>
    public T? Data { get; set; }

    public Result(bool ok) : base(ok)
    {
    }

    public Result(bool ok, T data) : base(ok)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string? message = null)
    {
        return new Result<T>(true, data) { Message = message };
    }
}
