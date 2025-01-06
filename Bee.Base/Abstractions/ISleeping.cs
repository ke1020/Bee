namespace Bee.Base.Abstractions;

/// <summary>
/// 系统休眠功能管理接口
/// </summary>
public interface ISleeping
{
    /// <summary>
    /// 开始阻止进入休眠状态
    /// </summary>
    void Prevent();
    /// <summary>
    /// 恢复阻止进入休眠状态
    /// </summary>
    void Restore();
}