# 相关 CLI 命令

## 安装 Avalonia 模板

```bash
dotnet new install Avalonia.Templates
```

## 卸载 Avalonia 模板

```bash
dotnet new uninstall Avalonia.Templates
```

## 列出已安装的模板

```bash
dotnet new list
```

## 创建项目

```bash
dotnet new avalonia.mvvm -o Bee
```

## 向 GitHub 与 Gitee 推送数据

```bash
# git branch -M main
git push -u github main

# gitee
git push -u gitee main
```

## 推送 Nuget 包

```bash
dotnet nuget push .\bin\Release\Ke.Bee.Localization.0.1.1.nupkg --source https://api.nuget.org/v3/index.json --api-key {key}
```

## 生成 UML 图命令

```bash
puml-gen C:\Users\ke\dev\proj\avalonia\Bee\Bee\Services D:\0Open\Bee\uml -dir -ignore Private,Protected -createAssociation -allInOne -excludePaths ServiceCollectionExtensions.cs,HarmonyOSFontCollection.cs
```

## 快速创建插件所需目录

```bash
# powershell 中执行
cmd /c "mkdir Configs i18n Models Navigation Views ViewModels Tasks docs"
```