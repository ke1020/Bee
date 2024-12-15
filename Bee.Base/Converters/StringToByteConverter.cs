
using System.Globalization;
using Avalonia.Data.Converters;

namespace Bee.Base.Converters;

public class StringToByteConverter : IValueConverter
{
    /// <summary>
    /// 将数据模型中的值转换为字符串，流向 UI 元素时
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value ?? string.Empty;
    }

    /// <summary>
    /// 用户在 UI 中输入或修改了某些内容，并且这些更改需要反映到数据模型时
    /// </summary>
    /// <param name="value"></param>
    /// <param name="targetType"></param>
    /// <param name="parameter"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value != null && byte.TryParse(value.ToString(), out var v))
        {
            return v;
        }
        
        return null;
    }
}