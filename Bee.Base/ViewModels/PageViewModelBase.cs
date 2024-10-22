
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.ViewModels;

/// <summary>
/// 视图页面模型基类
/// </summary>
public class PageViewModelBase : ObservableObject
{
    /// <summary>
    /// 页面标题
    /// </summary>
    public string? Title { get; set; }
}
