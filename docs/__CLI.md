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
puml-gen D:\0Open\Bee\Bee\Services D:\0Open\Bee\uml -dir -ignore Private,Protected -createAssociation -allInOne -excludePaths ServiceCollectionExtensions.cs,HarmonyOSFontCollection.cs
```

## docker 运行 plantuml-server 服务

```bash
sudo docker run -d -p 8080:8080 plantuml/plantuml-server:jetty
```