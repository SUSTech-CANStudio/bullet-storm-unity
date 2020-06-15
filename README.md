# particle-storm-unity

[![Codacy Badge](https://api.codacy.com/project/badge/Grade/a639fe15435f42408e9eeb2e315121c3)](https://app.codacy.com/gh/SUSTech-CANStudio/particle-storm-unity?utm_source=github.com&utm_medium=referral&utm_content=SUSTech-CANStudio/particle-storm-unity&utm_campaign=Badge_Grade_Dashboard)

更好用的粒子系统，为三维情况下的弹幕游戏设计

- 可以为每个粒子单独编写脚本
- 极低的性能开销
- 方便的全局和局部粒子控制方法
- 可将编写好的粒子风暴存为文件（待实现）

## 安装

直接将[Assets](./Assets)文件夹下所有内容复制到您的Unity项目Assets文件夹下

## 使用

- 基本概念
  - 粒子`Particle`：粒子风暴中，所有被发射的物体统称`Particle`
  - 粒子预设`ParticlePrefeb`：允许您使用图形界面设置`Particle`
  - 风暴`Storm`：按照预定义的方式有序发射各种粒子的过程称为`Storm`
  - 风暴发生器`StormGenerator`：可以在发生器所处的位置生成`Storm`
  - 发射参数列表`EmitList`：表示一次`Storm`的发射行为中所有粒子的初始状态

- 初次使用，请尝试[使用示例](./Assets/ParticleStorm/Samples)
- 详细介绍查看[帮助文档](./Assets/ParticleStorm/README.md)