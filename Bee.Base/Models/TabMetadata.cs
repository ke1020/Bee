namespace Bee.Base.Models;

/// <summary>
/// 标签元数据
/// </summary>
/// <param name="localKey">本地化 KEY</param>
/// <param name="viewModelType">对应的视图模型类型</param>
/// <param name="viewType">对应的视图类型</param>
public class TabMetadata(string localKey, Type viewType, Type viewModelType)
{
    public string LocalKey { get; } = localKey;
    public Type ViewModelType { get; } = viewModelType;
    public Type ViewType { get; } = viewType;
}