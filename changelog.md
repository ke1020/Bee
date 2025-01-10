# 变更日志

本项目的所有重要变更都将记录在此文件中。

## [] - 2025-01-10

### 修改

- 日志记录器修改为依赖 `Serilog.Extensions.Logging` 库
- 移除 `Bee.Base/SerilogLogger.cs` 类文件

## [] - 2025-01-07

### 修改

- 任务列表控件视图模型 `Execute` 方法中处理逻辑，移至 `TaskHandlerBase` 类中 (可以重写来自定义任务列表的处理逻辑)

## [] - 2025-01-04

### 修改

- 任务列表控件中的项，增加参数配置框

## [] - 2025-01-03

### 修改

- 修正 `ILocalizationResourceContributor` 名称拼写错误的问题
- 升级本地化包至 `0.1.4` 版本

## [] - 2025-01-01

### 修改

- 引入 `LanguageExt.Core` 库，使用库的 `Fin` 类型取代 `Result` 类型

## [] - 2024-12-30

### 新增

- 新增 `Toastr` 控件

### 删除

- 删除 `ViewModelBase` 类

## [] - 2024-12-29

### 修改

- `PluginResult` 类重命名为 `Result`
- `ITaskHandler` 接口的 `ExecuteAsync` 方法返回值类型修改为 `Result`

## [] - 2024-12-26

### 新增

- `Serilog` 日志接收器
- `ITaskHandler` 接口中增加 `CreateTasksFromInputPathsAsync` 方法
- 为 `ITaskHandler` 接口提供一个默认实现 `TaskHandlerBase` 抽象基类

### 修改

- `MainWindow` 修复窗口不能恢复原始尺寸的问题
- `Avalonia` 框架版本升级到 `11.2.3` 版本
- `ITaskCoverHandler` 接口修改为 `ICoverHandler`，并提供一个默认实现（作为整个应用的通用封面处理器）

### 移除

- 删除主程序中文档转换器相关的类、视图、视图模型及菜单等 (已在文档转换器插件实现)

## [] - 2024-12-22

### 修改

- `ITaskHandler` 接口 `ExecuteAsync` 方法增加 `CancellationToken` 参数
- `ServiceCollectionExtensions.cs` 注册插件方法中
  - 增加反射类型载入异常 `ReflectionTypeLoadException` 捕获
  - `AddPlugins` 方法中修改为只注册插件根目录下的 `dll` 文件
- 参数面板中的 `Tabs` 等视图模型抽象到 `WorkspaceViewModel` 类中

## [0.0.1.1] - 2024-12-19

### 新增功能

### 修改

- `Bee` 主程序 `MenuItemViewModel.cs` 移动至 `Bee.Base` 程序集的 `ViewModels` 目录下

### 修复

### 废弃

### 移除

### 安全
