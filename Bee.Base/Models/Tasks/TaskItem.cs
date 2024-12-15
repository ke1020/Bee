using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Bee.Base.Models.Tasks;

/// <summary>
/// 任务项
/// </summary>
public partial class TaskItem : ObservableObject, IDisposable
{
    /// <summary>
    /// 任务名称
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// 百分比
    /// </summary>
    [ObservableProperty]
    private double _percent = 0;
    [ObservableProperty]
    private bool _isCompleted = false;
    /// <summary>
    /// 位图资源
    /// </summary>
    public Bitmap? Source { get; set; }
    /// <summary>
    /// 文件地址
    /// </summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// 释放占用资源
    /// </summary>
    public void Dispose()
    {
        Source?.Dispose();
    }
}
