在 C# 中实现一个插件架构，支持动态加载和卸载 DLL 文件的功能，通常会使用到 `Assembly` 类和 `AppDomain` 或 `AssemblyLoadContext` 来实现动态的加载和卸载。以下是一个简单的实现方法，涵盖了动态加载、执行插件方法、以及卸载 DLL 的基本步骤。

### 1. 插件接口定义

首先，我们定义一个插件接口 `IPlugin`，这是所有插件必须实现的标准接口。这样插件可以是动态加载的 DLL 文件中的一个类。

```csharp
public interface IPlugin
{
    void Execute();
}
```

### 2. 插件实现

然后，你可以创建一个插件 DLL，例如 `MyPlugin.dll`，实现这个接口。

```csharp
public class MyPlugin : IPlugin
{
    public void Execute()
    {
        Console.WriteLine("MyPlugin is executing.");
    }
}
```

### 3. 主应用程序：动态加载插件

在主程序中，我们可以动态加载 DLL 并执行插件方法。使用 `Assembly.LoadFrom` 或 `Assembly.LoadFile` 方法来加载插件 DLL 文件。使用反射来调用插件中的方法。

```csharp
using System;
using System.IO;
using System.Reflection;

public class PluginHost
{
    private static IPlugin loadedPlugin = null;

    public static void LoadPlugin(string pluginPath)
    {
        // 检查插件 DLL 是否存在
        if (!File.Exists(pluginPath))
        {
            Console.WriteLine("Plugin not found.");
            return;
        }

        // 加载插件程序集
        Assembly pluginAssembly = Assembly.LoadFrom(pluginPath);

        // 查找实现了 IPlugin 接口的类型
        Type pluginType = null;
        foreach (Type type in pluginAssembly.GetTypes())
        {
            if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            {
                pluginType = type;
                break;
            }
        }

        if (pluginType != null)
        {
            // 创建插件实例
            loadedPlugin = (IPlugin)Activator.CreateInstance(pluginType);
            Console.WriteLine("Plugin loaded successfully.");
        }
        else
        {
            Console.WriteLine("No valid plugin found.");
        }
    }

    public static void ExecutePlugin()
    {
        loadedPlugin?.Execute();
    }

    public static void UnloadPlugin()
    {
        loadedPlugin = null;
        Console.WriteLine("Plugin unloaded.");
    }
}
```

### 4. 插件的卸载

在 .NET Framework 中，程序集一旦加载到应用程序域中，是不能被卸载的。即使我们将插件从 `IPlugin` 接口引用中移除，程序集本身依然会占用内存。因此，卸载插件的关键是在新的应用程序域中加载程序集，或使用 `AssemblyLoadContext`（在 .NET Core 和 .NET 5 及以上版本中支持）。

#### 使用 `AssemblyLoadContext`（.NET Core/5+）

如果你使用的是 .NET Core 或 .NET 5/6/7 等版本，你可以通过 `AssemblyLoadContext` 来实现卸载功能。

```csharp
using System;
using System.Reflection;
using System.Runtime.Loader;

public class PluginHostWithContext
{
    private static AssemblyLoadContext pluginLoadContext = null;

    public static void LoadPlugin(string pluginPath)
    {
        if (!File.Exists(pluginPath))
        {
            Console.WriteLine("Plugin not found.");
            return;
        }

        // 创建一个新的 AssemblyLoadContext，便于卸载
        pluginLoadContext = new AssemblyLoadContext("PluginContext", true);

        // 加载插件程序集
        Assembly pluginAssembly = pluginLoadContext.LoadFromAssemblyPath(pluginPath);

        // 查找实现 IPlugin 接口的类型
        Type pluginType = null;
        foreach (Type type in pluginAssembly.GetTypes())
        {
            if (typeof(IPlugin).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            {
                pluginType = type;
                break;
            }
        }

        if (pluginType != null)
        {
            // 创建插件实例
            IPlugin plugin = (IPlugin)Activator.CreateInstance(pluginType);
            Console.WriteLine("Plugin loaded successfully.");
            plugin.Execute();
        }
        else
        {
            Console.WriteLine("No valid plugin found.");
        }
    }

    public static void UnloadPlugin()
    {
        if (pluginLoadContext != null)
        {
            pluginLoadContext.Unload();
            Console.WriteLine("Plugin unloaded.");
        }
    }
}
```

在这个示例中，`AssemblyLoadContext` 是一个允许我们卸载程序集的机制，通过 `pluginLoadContext.Unload()` 来卸载插件程序集。这个方法只有在 .NET Core 和更高版本中才有效。

### 5. 使用示例

```csharp
public class Program
{
    public static void Main()
    {
        // 插件路径
        string pluginPath = @"C:\path\to\your\plugin\MyPlugin.dll";

        // 加载插件
        PluginHostWithContext.LoadPlugin(pluginPath);

        // 执行插件
        PluginHostWithContext.ExecutePlugin();

        // 卸载插件
        PluginHostWithContext.UnloadPlugin();
    }
}
```

### 总结

- **插件接口**：我们通过一个统一的接口 (`IPlugin`) 来确保所有插件具有相同的功能。
- **动态加载**：使用 `Assembly.LoadFrom` 或 `Assembly.LoadFile` 加载插件 DLL 文件。
- **卸载插件**：对于 .NET Core 或 .NET 5/6/7，可以使用 `AssemblyLoadContext` 来加载和卸载插件。对于 .NET Framework，卸载插件较为复杂，通常需要将插件程序集加载到独立的应用程序域中。

这种方式可以实现一个简单而灵活的插件架构，适合动态加载和卸载插件。
