using System.Globalization;
using Avalonia.Data.Converters;
using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Base.Converters;

/// <summary>
/// 枚举值本地化转换器
/// </summary>
public class EnumLocalizeConverter : IValueConverter
{
    private readonly ILocalizer _l;
    public EnumLocalizeConverter()
    {
        _l = ServiceLocator.GetRequiredService<ILocalizer>();
    }

    /// <summary>
    /// 枚举值转换为本地化文本
    /// </summary>
    /// <param name="value">枚举值</param>
    /// <param name="targetType"></param>
    /// <param name="prefix">对应的本地化前缀</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? prefix, CultureInfo culture)
    {
        // 拼接本地化 Key 值
        // 示例：parameter = "Bee.Plugin.ImageProcess" # 表示前缀
        // 枚举值：value = "WatermarkPosition.Center" # 表示枚举值
        // 完整的本地化 KEY 为 "Bee.Plugin.ImageProcess.WatermarkPosition.Center"
        return _l[$"{prefix}.{value}"];
    }

    public object? ConvertBack(object? value, Type targetType, object? prefix, CultureInfo culture)
    {
        return null;
    }
}