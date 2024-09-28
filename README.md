# WuliuGO

## 介绍
WuliuGO 围棋网页项目。

## 运行项目

### 先决条件
- .NET 8.0 SDK
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

```bash6
docker pull postgres:latest
docker run --name pg-database -e POSTGRES_PASSWORD=mysecretpassword -d -p 5432:5432 postgres
```

执行

```sql
-- --------------------------------------------------------
-- 服务器版本:                        PostgreSQL 16.4 (Debian 16.4-1.pgdg120+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
-- HeidiSQL 版本:                  12.6.0.6765
-- --------------------------------------------------------

CREATE TABLE IF NOT EXISTS "KatagoQueries" (
	"id" SERIAL NOT NULL,
	"query_id" VARCHAR(255) NULL DEFAULT NULL,
	"is_during_search" BOOLEAN NOT NULL,
	"move_infos" JSONB NULL DEFAULT NULL,
	"root_info" JSONB NULL DEFAULT NULL,
	"turn_number" INTEGER NOT NULL,
	PRIMARY KEY ("id")
);
CREATE TABLE IF NOT EXISTS "Users" (
	"id" SERIAL NOT NULL,
	"name" VARCHAR(100) NULL DEFAULT NULL,
	"rank" INTEGER NULL DEFAULT NULL,
	PRIMARY KEY ("id")
);

```

