-- --------------------------------------------------------
-- 服务器版本:                        PostgreSQL 16.4 (Debian 16.4-1.pgdg120+1) on x86_64-pc-linux-gnu, compiled by gcc (Debian 12.2.0-14) 12.2.0, 64-bit
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
