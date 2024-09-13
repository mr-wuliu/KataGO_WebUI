# WuliuGO

## 介绍
WuliuGO 是一个物流管理系统，提供用户认证和管理功能。

## 运行项目

### 先决条件
- .NET 6.0 SDK
- PostgreSQL 数据库

### 配置数据库
1. 在 `appsettings.json` 中配置数据库连接字符串。
2. 运行数据库迁移命令：
   ```bash
   dotnet ef database update
   ```

### 运行应用
1. 通过以下命令运行应用：
   ```bash
   dotnet run
   ```
2. 打开浏览器并访问 `https://localhost:5001/swagger` 查看 API 文档。

## API 端点
- `POST /api/user/login` - 用户登录
- `POST /api/user/add` - 添加用户
- `GET /api/user/{id}` - 根据 ID 获取用户
- `GET /api/user/getAll` - 获取所有用户

# 搭建数据库

```bash
docker pull postgres:latest
docker run --name pg-database -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 postgres
```

执行

```sql
CREATE DATABASE mydatabase;
\c mydatabase;
CREATE TABLE players (
    id SERIAL PRIMARY KEY,
    name VARCHAR(100),
    rank INT
);
```

