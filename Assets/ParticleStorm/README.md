# Particle Storm

*粒子风暴* 中文帮助手册

## 简介

更好用的粒子系统，为三维情况下的弹幕游戏设计

- 特性
  - 可以为每个粒子单独编写脚本
  - 极低的性能开销
  - 方便的全局和局部粒子控制方法
  - 可将编写好的粒子风暴存为文件（待实现）
- 基本概念
  - 粒子`Particle`：您可以创建一类粒子，它们具有同一个脚本定义的行为模式和相同的材质
  - 风暴`Storm`：粒子的整体运动称为风暴，风暴是设置粒子行为的基本单位，一个风暴可以包含多种粒子和行为方式
  - 风暴发生器`StormGenerator`：用于在游戏引擎中形成风暴的组件，同一个风暴可以在不同的发生器被生成，一个发生器可以生成多个风暴
  - 发射参数`EmitParams`：和Unity引擎的`ParticleSystem.EmitParams`类似，`EmitParams`的列表用于表示一个发射行为需要的参数，但您不必亲自创建这个列表，有写好的过滤器`Filters`来替您创建

## 命名空间

- [`ParticleStorm`](./)：经常需要用到的基本类型放在这个命名空间中
  - [`ParticleStorm.Factories`](./Factories)：工厂类的存放位置，`Particle`必须从工厂被创建
  - [`ParticleStorm.Util`](./Util)：设置和一些实用工具
  - [`ParticleStorm.Core`](./Core)：核心代码，您一般不会用到

## API文档

### `ParticleStorm` 命名空间

#### `APParticle` 类

基于`ParticleSystem`的粒子抽象类，粒子脚本可以继承这个类

注意：没有构造器，必须从`ParticleFactory`创建实例

##### 属性

- `updateMode`：枚举类型，脚本调用方式，根据Unity的刷新方式有三种选择：`Update`，`FixedUpdate`，和`LateUpdate`
  - get，set
- `materials`：粒子材质数组
  - get，set
- `material`：粒子材质，有多个材质时指第一个
  - get，set
- `trigger`：与粒子trigger有关的设置，详见[TriggerModule](https://docs.unity3d.com/ScriptReference/ParticleSystem.TriggerModule.html)
  - get
- `main`：粒子系统的基本设置，详见[MainModule](https://docs.unity3d.com/ScriptReference/ParticleSystem.MainModule.html)
  - get

##### 方法

- `ParticleStart()`：在粒子系统初始化时被调用，可以进行初始设定
  - 抽象方法，必须被重写
- `ParticleUpdate()`：粒子更新时被调用，具体调用时间取决于`updateMode`
  - 抽象方法，必须被重写
  - `self`：代表粒子的参数，可使用脚本获取和修改属性，详见[ParticleModule](https://docs.unity3d.com/ScriptReference/ParticleSystem.Particle.html)
  - 注意：此方法中使用`MonoBehavior`相关函数无效
- `OnEnter()`：粒子进入碰撞体时调用
  - 可重写
- `OnExit()`：粒子离开碰撞体时调用
  - 可重写
- `OnInside()`：粒子在碰撞体内时调用
  - 可重写
- `OnOutside()`：粒子在碰撞体外时调用
  - 可重写

#### `AOParticle` 类

基于`GameObject`的粒子抽象类，粒子脚本可以继承这个类，效率较低

（待实现）

#### `Storm` 类

风暴类，粒子行为的基本单位

##### 属性

- `name`：风暴的名字
  - get

##### 方法

- `Storm(string name)`：构造器，创建一个空的`Storm`对象
  - `name`：风暴的名字
- `Find(string name)`：根据名字获取一个已有的风暴
  - 静态方法
  - `name`：要获取的风暴名字
  - 返回：获取到的风暴对象，若不存在，抛出异常并返回`null`
- `AddBehavior(float startTime, List<EmitParams> emitParams, IParticle particle, float gap = 0)`：为风暴添加一个行为
  - `startTime`：行为开始时间，相对于风暴开始时间计算，单位秒
  - `emitParams`：发射参数，常通过`ParticleStorm.Util.Filters`获取
  - `particle`：被发射的粒子种类
  - `gap`：单个粒子发射间隔，可选，0为同时发射

#### `StormGenerator` 类

风暴生成器，作为组件添加到`GameObject`上

- `Generate(Storm storm)`：生成风暴
  - `storm`：要生成的风暴

### `ParticleSystm.Factories` 命名空间

#### `ParticleFactory` 类

粒子工厂，所有`Particle`应该在这里创建

##### 属性

- `particleTag`：创建粒子游戏物体时使用的Tag
  - get

##### 方法

- `GetParticle(string name)`：获取一个粒子
  - `name`：要获取的粒子名字
  - 返回：得到的粒子对象，若不存在，抛出异常并返回`null`
- `NewParticle<T>(string name)`：根据类型创建一个粒子
  - `T`：粒子的类型
  - `name`：粒子名字
  - 返回：创建的粒子，若粒子已存在，抛出异常并返回`null`
- `NewParticle(GameObject prefeb)`：从`prefeb`创建一个粒子
  - `prefeb`：带有粒子组件的`GameObject`
  - 返回：创建的粒子，若创建失败，抛出异常并返回`null`
  - **注意**：由于Unity的特性，从`prefeb`创建的粒子名称会带有`(copy)`后缀，并非`prefeb`中游戏物体的名称

### `ParticleSystm.Util` 命名空间

#### `Filters` 类

静态工具类，过滤器，用于产生和修改`emitParams`

##### 枚举类型

- `OverlayMode`：叠加方式
  - `COVER`：覆盖
  - `ADD`：叠加
  - `MINUS`：消去
  - `MULTIPLY`：求积
  - `DIVIDE`：求商
  - `AVERGE`：平均

##### 方法

所有方法返回值均为`emitParams`列表。

所有生成器第一个参数都是粒子数量。所有过滤器第一个参数都是被修改的`emitParams`列表，最后一个参数都是过滤器叠加方式。这两个参数文档中将不再赘述。

- `Empty(int num)`：空生成器

- `Cone(int num, float radius, float theta, float speed)`：圆锥生成器
  - `radius`：生成点与原点距离（圆锥母线长）
  - `theta`：圆锥半顶角（角度制）
  - `speed`：粒子初始速度
- `Cone(List<EmitParams> @params, float radius, float theta, float speed, OverlayMode mode = OverlayMode.COVER)`：圆锥过滤器
  - 参数与圆锥生成器相同
- `Size(List<EmitParams> @params, float size, OverlayMode mode = OverlayMode.COVER)`：尺寸过滤器
  - `size`：粒子大小
- `Size(List<EmitParams> @params, float fromSize, float toSize, OverlayMode mode = OverlayMode.COVER)`：渐变尺寸过滤器
  - `fromSize`：起始大小
  - `toSize`：末尾大小

#### `Sphere` 类

球坐标

##### 属性

- `r`：轴长r
  - get，set
- `theta`：到北极角度θ（角度制）
  - get，set
- `phi`：到极轴角度φ（角度制）
  - get，set
- `vector3`：对应的`Vector3`向量
  - get
- `normalized`：单位向量
  - get

##### 方法

- `Sphere(float r, float theta, float phi)`：构造器
  - `r`：轴长r
  - `theta`：到北极角度θ（角度制）
  - `phi`：到极轴角度φ（角度制）
- `Sphere(Vector3 vector3)`：构造器
  - `vector3`：`Vector3`向量
- `Sphere(Sphere sphere)`：拷贝构造器
  - `sphere`：要拷贝的对象
- `ToVector3()`：
  - 返回：对应的`Vector3`向量
- `ToString()`：
  - 返回：字符串描述的球坐标

#### `Settings` 类

静态类，设置部分

##### 属性

- `particleTag`：粒子对应游戏物体的默认Tag，必须是游戏中存在的Tag
- `useRightHandedCoordinateSystem`：是否使用右手系，若使用右手系，极坐标的y轴和z轴将会交换