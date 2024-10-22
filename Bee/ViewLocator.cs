using System;
using System.Text.RegularExpressions;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Bee.Base.ViewModels;
using Bee.ViewModels;

namespace Bee;

public class ViewLocator : IDataTemplate
{
    public Control? Build(object? data)
    {
        if (data is null)
            return null;

        var dataType = data.GetType();
        var name = dataType.FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        //var type = Type.GetType(name);
        // 完全限定名
        var type = Type.GetType($"{name}, {GetAssemblyName(dataType)}");
        if (type != null)
        {
            var control = (Control)Activator.CreateInstance(type)!;
            control.DataContext = data;
            return control;
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase || data is PageViewModelBase;
    }

    /// <summary>
    /// 获取程序集名称
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private string GetAssemblyName(Type type)
    {
        // 类型的完整名称
        string? input = type.AssemblyQualifiedName;
        if (input is null)
        {
            return string.Empty;
        }

        // 正则表达式匹配两个逗号之间的内容
        var match = Regex.Match(input, ",(.*?),");
        return match.Success ? match.Groups[1].Value.Trim() : string.Empty;
    }
}
