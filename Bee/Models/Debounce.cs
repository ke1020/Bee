using System;
using System.Threading;

namespace Bee.Models;

/// <summary>
/// 防抖器
/// </summary>
public class Debounce
{
    private readonly int _delay;
    private Timer? _timer;

    public Debounce(int delay)
    {
        _delay = delay;
    }

    public void Trigger(Action action)
    {
        // 清除之前的定时器
        _timer?.Dispose();
        _timer = null;

        // 创建新的定时器
        _timer = new Timer(_ => action(), null, _delay, Timeout.Infinite);
    }
}