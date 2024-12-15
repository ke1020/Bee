namespace Bee.Base.Models;

public class SelectableItem
{
    public string Label { get; set; } = string.Empty;
    public bool IsSelected { get; set; }
}

/// <summary>
/// 可绑定到 CheckBox 控件的模型
/// </summary>
public class SelectableItem<T> : SelectableItem
{
    public T? Value { get; set; }

}