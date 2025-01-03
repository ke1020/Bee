namespace Bee.Base.Models;

/// <summary>
/// 简单键值强类型对象
/// </summary>
/// <param name="name"></param>
/// <param name="value"></param>
public class NameValue(string name, string value)
{
    public string Name { get; set; } = name;
    public string Value { get; set; } = value;
}