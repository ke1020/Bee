# 标记扩展（Markup Extensions）

`MarkupExtension` 允许在 `XAML` 中以方便且可重用的语法，基于代码对目标属性的设定逻辑进行定制。使用花括号 `{}` 来区分普通文本的使用。

Avalonia 提供了以下功能：

| 标记扩展类型      | 描述                                       |
| ----------------- | ------------------------------------------ |
| StaticResource    | 一个已存在的键控资源，在发生变化时不会更新 |
| DynamicResource   | 延迟加载的键控资源，会在发生变化时更新     |
| Binding           | 基于默认的绑定偏好：编译或反射             |
| CompiledBinding   | 基于编译后的绑定                           |
| ReflectionBinding | 基于反射绑定                               |
| TemplateBinding   | 基于仅在 ControlTemplate 内使用的简化绑定  |
| OnPlatform        | 在指定平台上时条件性使用                   |
| OnFormFactor      | 在指定形态因素上时条件性使用               |

## 编译器内在函数（Compiler Intrinsics）

这些技术上不属于 `MarkupExtension` 而是 XAML 编译器的一部分，但是 XAML 语法相同。

| 内在函数   | 分配给属性               |
| ---------- | ------------------------ |
| `x:True`   | `true` 文本字面量        |
| `x:False`  | `false` 文本字面量       |
| `x:Null`   | `null` 文本字面量        |
| `x:Static` | 静态成员值               |
| `x:Type`   | `System.Type` 文本字面量 |

`x:True` 和 `x:False` 字面量在目标绑定属性为 `object` 时有用例，并且你需要提供布尔值。在缺乏类型信息的情况下，提供 `"True"` 仍然是 `string` 类型。

示例：

```xml
<Button Command="{Binding SetStateCommand}" CommandParameter="{x:True}"/>
```

## 创建 `MarkupExtension`

从 `MarkupExtension` 派生或者添加以下通过鸭子类型支持的签名之一：

```csharp
T ProvideValue();
T ProvideValue(IServiceProvider provider);
object ProvideValue();
object ProvideValue(IServiceProvider provider);
```

当使用强类型而不是 `object` 时，在 XAML 构造参数、属性或 `ProvideValue` 方法返回值与声明类型不匹配时，会收到编译错误。当返回 `object` 时，实际返回类型必须与目标属性类型匹配，否则会在运行时抛出 `InvalidCastException` 异常。

### 接收字面量参数

需要参数时，可以使用构造函数来接收每个参数。

对于可选或无序参数，则应使用属性。允许混合使用多个构造函数，包括无参构造函数。

```csharp
public class MultiplyLiteral
{
    private readonly double _first;
    private readonly double _second;

    public double? Third { get; set; }

    public MultiplyLiteral(double first, double second)
    {
        _first = first;
        _second = second;
    }

    public double ProvideValue(IServiceProvider provider)
    {
        return First * Second * Third ?? 1;
    }
}
```

```xml
<TextBlock Text="This has FontSize=40" FontSize="{namespace:MultiplyLiteral 10, 8, Third=0.5}" />
```

### 从绑定接收参数（Receiving Parameters From Bindings）

一个常见的场景是希望转换来自绑定的数据并更新目标属性。当所有参数都来自绑定时，创建一个带有 `IMultiValueConverter` 的 `MultiBinding` 是比较直接的方法。在下面的例子中，`MultiplyBinding` 需要两个绑定参数。如果需要混合字面量和绑定参数，创建一个 `IMultiValueConverter` 可以允许将字面量作为构造函数或 `init` 参数传递。`BindingBase` 允许使用 `CompiledBinding` 和 `ReflectionBinding`，但不允许使用字面量。

```cs
public class MultiplyBinding
{
    private readonly BindingBase _first;
    private readonly BindingBase _second;

    public MultiplyBinding(BindingBase first, BindingBase second)
    {
        _first = first;
        _second = second;
    }

    public object ProvideValue()
    {
        var mb = new MultiBinding()
        {
            Bindings = new[] { _first, _second },
            Converter = new FuncMultiValueConverter<double, double>(doubles => doubles.Aggregate(1d, (x, y) => x * y))
        };

        return mb;
    }
}
```

```xml
<TextBlock
       FontSize="{local:MultiplyBinding {Binding Multiplier}, {Binding Multiplicand}}"
       Text="MarkupExtension with Bindings!"/>
```

> **信息**
>
> 另一种方法是返回 `IObservable<T>.ToBinding()`。

### 返回参数（Returning Parameters）

为了使 `MarkupExtension` 与多种目标属性类型兼容，可以返回 `object` 并单独处理每种支持的类型。

```cs
public object ProvideValue(IServiceProvider provider)
{
    var target = (IProvideValueTarget)provider.GetService(typeof(IProvideValueTarget))!;
    var targetProperty = target.TargetProperty as AvaloniaProperty;
    var targetType = targetProperty?.PropertyType;

    double result = First * Second * (Third ?? 1);

    if (targetType == typeof(double))
        return result;
    else if (targetType == typeof(float))
        return (float)result;
    else if (targetType == typeof(int))
        return (int)result;
    else
        throw new NotSupportedException();
}
```

构造函数也可以使用 `object` 方法接收参数类型，但编译时错误同样会在运行时转变为异常。

### 标记扩展属性特性（MarkupExtension Property Attributes）

- `[ConstructorArgument]` - 关联属性可能由构造函数参数初始化，并且如果使用构造函数则应该忽略该属性用于 XAML 序列化。
- `[MarkupExtensionOption]`, `[MarkupExtensionDefaultOption]` - 结合 `ShouldProvideOption` 使用，检查 `OnPlatform` 和 `OnFormFactor` 的源码以获取示例。
