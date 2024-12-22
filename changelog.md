# 变更日志

本项目的所有重要变更都将记录在此文件中。


## [0.0.1.1] - 2024-12-22
### 新增功能

### 修改
- `ITaskHandler` 接口增加 `CancellationToken` 参数
- `ServiceCollectionExtensions.cs` 注册插件方法中，增加反射类型载入异常 `ReflectionTypeLoadException` 捕获
- 参数面板抽象到 `WorkspaceViewModel` 类中

## [0.0.1.1] - 2024-12-19
### 新增功能


### 修改
- `Bee` 主程序 `MenuItemViewModel.cs` 移动至 `Bee.Base` 程序集的 `ViewModels` 目录下

### 修复
### 废弃
### 移除
### 安全