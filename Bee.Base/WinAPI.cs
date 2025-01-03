
using System.Runtime.InteropServices;

// 定义 EXECUTION_STATE 枚举
[Flags]
public enum EXECUTION_STATE : uint
{
    ES_AWAYMODE_REQUIRED = 0x00000040,
    ES_CONTINUOUS = 0x80000000,
    ES_DISPLAY_REQUIRED = 0x00000002,
    ES_SYSTEM_REQUIRED = 0x00000001
    // ES_USER_PRESENT = 0x00000004, // 不推荐使用
}

public static class WinAPI
{
    // 导入 SetThreadExecutionState 函数
    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
}