# NovalAi3自动跑图工具

本工具实现了以下功能

- 自动批量生成
- 随机的画师组合抽取
- 随机的分辨率
- wildcard功能
- 生成时保存prompt（用于生成训练集）

~~因为刚开始的时候没有考虑很多，加上工作繁忙没有时间规划，所以代码写的相当乱~~

[使用教程](https://cyanautumn.github.io/NovalAi3AutoMaticDoc/)

![alt text](image.png)

## 架构说明

- 引入 `GenerationController` 与 `DirectorToolController`，分别负责生图与导演工具流程，WinForms 只做事件绑定。
- `PresetConfigRepository`/`SystemConfigRepository` 统一管理配置文件读写，消除了 UI 与磁盘路径的紧耦合。
- Tag/Prompt 模块通过 `IPromptContext` 获取依赖，支持后续服务化或单元测试。

## 后续建议

- 将导演工具加入取消/进度反馈，进一步完善控制器事件。
- 继续拆分 UI 与业务逻辑，例如将随机提示词、Wildcard 管理迁移到独立服务。
