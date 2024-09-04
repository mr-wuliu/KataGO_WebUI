# 项目设计



```
|_ Controllers
|_ GameLogic
|_ Helpers
|_ KatagoAI
|_ Repositories
|_ Services
|_Program.cs
```



## 围棋设计



入口GoGame

一个对局只维护一个棋盘. 用于打劫判断的状态均为临时状态

一个GoGame维护多个用户对局, 使用uid区分, 对局存储于内存中.

GoGame 完全独立, 只有GoGame类对外暴露方法

