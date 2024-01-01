# Blogrolling.Data.RSS

Blogrolling 的 RSS 内容获取器。

## 使用

### 添加源

```shell
Blogrolling.Data.RSS add <link>
```

### 删除源

```shell
Blogrolling.Data.RSS remove <link or name>
```

### 更新源

```shell
Blogrolling.Data.RSS refresh <link or name> [--force]
```

## 配置文件

配置文件目录位于 `UserHome/.config/blogrolling` 。  
其中每一个选项使用单独的文件保存，文件内容即选项值。  

| 选项名              | 是否必须 | 默认值 | 说明                |
|------------------|------|-----|-------------------|
| ConnectionString | 是    | N/A | 数据库的连接字符串。        |
| Debug            | 否    | N/A | 存在此文件即是 Debug 模式。 |

[//]: # (| Timeout          | 否    | 5   | 访问网络地址时的超时时间（秒）。  |)

## 构建

```shell
dotnet build
```
