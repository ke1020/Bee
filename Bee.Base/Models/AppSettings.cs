
namespace Bee.Base.Models;

public class AppSettings
{
    /// <summary>
    /// 全局输出目录
    /// </summary>
    public string OutputPath { get; set; } = string.Empty;
    /// <summary>
    /// 全局插件目录
    /// </summary>
    public string PluginPath { get; set; } = string.Empty;
    /// <summary>
    /// 任务列表项默认封面图
    /// </summary>
    public string DefaultTaskImageUri { get; set; } = string.Empty;
}
