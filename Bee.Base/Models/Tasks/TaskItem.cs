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
    /// <summary>
    /// 任务完成标志
    /// </summary>
    [ObservableProperty]
    private bool _isCompleted = false;
    /// <summary>
    /// 位图资源
    /// </summary>
    public Bitmap? Source { get; set; }
    /// <summary>
    /// 文件地址
    /// </summary>
    [Obsolete("请使用 Input 属性")]
    public string FileName { get; set; } = string.Empty;
    /// <summary>
    /// 输入地址 (根据 InputType 类型确认，可能为文件或目录地址类型)
    /// </summary>
    public string Input { get; set; } = string.Empty;
    /// <summary>
    /// 当前编辑状态
    /// </summary>
    [ObservableProperty]
    private bool _isEdit = false;
    /// <summary>
    /// 自定义参数
    /// </summary>
    public string? CustomArguments { get; set; }

    /// <summary>
    /// 释放占用资源
    /// </summary>
    public void Dispose()
    {
        Source?.Dispose();
    }
}
