# Blogrolling.Data.RSS

Blogrolling 的 RSS 内容获取器。

## 使用

### 添加源

```shell
dotnet Blogrolling.Data.RSS.dll -- add <link>
```

### 删除源

```shell
dotnet Blogrolling.Data.RSS.dll -- remove <link or name>
```

### 更新源

```shell
dotnet Blogrolling.Data.RSS.dll -- refresh [link or name] [--force]
```

## 配置文件

配置文件位于 `%UserHome%/.config/blogrolling.cfg` 。  
是一个键值对形式的文件，也可以从环境变量中读取。

| 键名                     | 是否必须 | 默认值   | 说明               |
|------------------------|------|-------|------------------|
| BLOGROLLING_MYSQL_HOST | 是    | N/A   | 数据库的地址。          |
| BLOGROLLING_MYSQL_PORT | 是    | N/A   | 数据库的端口号。         |
| BLOGROLLING_MYSQL_USER | 是    | N/A   | 数据库的用户名。         |
| BLOGROLLING_MYSQL_PASS | 是    | N/A   | 数据库的密码。          |
| BLOGROLLING_MYSQL_NAME | 是    | N/A   | 数据库的名称。          |
| BLOGROLLING_DEBUG      | 否    | false | 开启 Debug 模式。     |
| BLOGROLLING_TIMEOUT    | 否    | 5     | 访问网络地址时的超时时间（秒）。 |

## 构建

```shell
dotnet restore
dotnet publish -c Release
```
