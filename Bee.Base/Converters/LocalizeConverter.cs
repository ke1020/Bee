using System.Globalization;

using Avalonia.Data.Converters;

using Ke.Bee.Localization.Localizer.Abstractions;

namespace Bee.Base.Converters;

/// <summary>
/// 本地化转换器
/// </summary>
public class LocalizeConverter : IValueConverter
{
    private readonly ILocalizer _l;
    public LocalizeConverter()
    {
        _l = ServiceLocator.GetRequiredService<ILocalizer>();
    }

    /// <summary>
    /// 枚举值转换为本地化文本
    /// </summary>
    /// <param name="value">对象值</param>
    /// <param name="targetType"></param>
    /// <param name="prefix">对应的本地化前缀</param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? prefix, CultureInfo culture)
    {
        return value is string v ? _l[v] : null;
    }

    public object? ConvertBack(object? value, Type targetType, object? prefix, CultureInfo culture)
    {
        return null;
    }
}