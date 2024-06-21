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

程序会从 `%UserHome%/.config/blogrolling.cfg` 和 `./blogrolling.cfg` 搜索配置文件。  
是一个键值对形式的文件，也可以从环境变量中读取。

| 键名            | 是否必须 | 默认值   | 说明               |
|---------------|------|-------|------------------|
| MYSQL_HOST    | 是    | N/A   | 数据库的地址。          |
| MYSQL_PORT    | 是    | N/A   | 数据库的端口号。         |
| MYSQL_USER    | 是    | N/A   | 数据库的用户名。         |
| MYSQL_PASS    | 是    | N/A   | 数据库的密码。          |
| MYSQL_NAME    | 是    | N/A   | 数据库的名称。          |
| DEBUG         | 否    | false | 开启 Debug 模式。     |
| FETCH_TIMEOUT | 否    | 5     | 访问网络地址时的超时时间（秒）。 |

## 构建

```shell
dotnet restore
dotnet publish -c Release
```
