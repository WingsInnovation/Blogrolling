# Blogrolling

数据库模型在这里。

## 命令行工具

需要安装 `dotnet-ef` 。

### 新建迁移

```shell
dotnet ef migrations add <迁移名> -- "<MySQL 链接字符串>"
```

### 更新数据库

```shell
dotnet ef database update -- "<MySQL 链接字符串>"
```
