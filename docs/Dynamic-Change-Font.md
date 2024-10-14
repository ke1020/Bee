# 动态修改字体

1. 将字体设置放入资源字典

```xml

<Application.Resources>
    <ResourceDictionary>
        <FontFamily x:Key="DefaultFontFamily">HarmonyOS Sans SC</FontFamily>
    </ResourceDictionary>
</Application.Resources>

```

2. 在样式文件中或控件上使用 `DynamicResource`

```xml

<!-- window 窗体 -->
<Style Selector="Window">
    <Setter Property="FontFamily" Value="{DynamicResource DefaultFontFamily}" />
</Style>

<!-- 菜单项 -->
<Style Selector="MenuItem">
    <Setter Property="FontFamily" Value="{DynamicResource DefaultFontFamily}" />
</Style>

```

3. 然后在后台代码中可以动态更改这个资源字典中的字体：

```cs
// 动态修改字体实现
var res = Application.Current?.Resources;
if (res != null)
{
    res["DefaultFontFamily"] = menuItem?.CommandParameter switch
    {
        "en-US" => new FontFamily("Inter"), // 英文使用 Inter 字体
        _ => new FontFamily("HarmonyOS Sans SC") // 其它语言使用鸿蒙 Sans SC 字体
    };
}
```
