# 变更日志

本项目的所有重要变更都将记录在此文件中。

## [] - 2024-12-26

### 新增

- 文档转换功能插件
- `Serilog` 日志接收器

### 修改

- `MainWindow` 修复窗口不能恢复原始尺寸的问题
- `Avalonia` 框架版本升级到 `11.2.3` 版本

### 移除

- 删除主程序中文档转换器相关的类、视图、视图模型及菜单等 (已在文档转换器插件实现)

## [] - 2024-12-22

### 修改

- `ITaskHandler` 接口 `ExecuteAsync` 方法增加 `CancellationToken` 参数
- `ServiceCollectionExtensions.cs` 注册插件方法中
  - 增加反射类型载入异常 `ReflectionTypeLoadException` 捕获
  - 修改为只注册插件根目录下的 `dll` 文件
- 参数面板中的 `Tabs` 等视图模型抽象到 `WorkspaceViewModel` 类中

## [0.0.1.1] - 2024-12-19

### 新增功能

### 修改

- `Bee` 主程序 `MenuItemViewModel.cs` 移动至 `Bee.Base` 程序集的 `ViewModels` 目录下

### 修复

### 废弃

### 移除

### 安全
