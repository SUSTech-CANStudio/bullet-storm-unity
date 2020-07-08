# Particle Storm

*粒子风暴* 中文帮助手册

## 目录

[toc]

## 命名空间

- 可能会用到的命名空间
  - [`ParticleStorm`](./)：经常需要用到的基本类型放在这个命名空间中
  - [`ParticleStorm.Script`](./Script)：为粒子创建脚本需要用到的类
  - [`ParticleStorm.Util`](./Util)：设置和一些实用工具

## API文档

### `ParticleStorm` 命名空间

#### `Particle` 类

粒子可以用于发射。粒子可以添加各种模块，包括脚本。

##### 属性

- `name`：粒子的名字
  - get
  - set

##### 方法

- `Particle()`：创建一个空的粒子
- `Particle(string name)`：创建一个空的粒子
  - `name`：粒子名字
- `Particle(ParticlePrefeb prefeb)`：根据预设创建一个粒子，并默认以预设名字作为粒子名字
  - `prefeb`：粒子的预设
- `Particle(string name, ParticlePrefeb prefeb)`：根据预设创建一个粒子，但使用自定义的名字
  - `name`：粒子名字
  - `prefeb`：粒子的预设
- `Find(string name)`：根据名字获取粒子
  - 静态方法
  - `name`：粒子名字
  - 返回`Particle`：获取到的粒子
- `SetPrefeb(ParticlePrefeb prefeb, bool destroy = true)`：设置粒子的预设
  - `prefeb`：粒子的预设
  - `destroy`：是否销毁之前的预设对象。默认为true，如果之前的预设对象还将被其它粒子使用，请改为false
- `Emit(EmitParams emitParams, int num)`：发射该粒子
  - `emitParams`：粒子的初始参数
  - `num`：发射的数量

#### `ParticlePrefeb` 类

粒子预设

提供图形界面，在项目资源管理器中单击右键→创建→`ParticlePrefeb`

#### `EmitList` 类

发射参数`EmitParams`的列表，用于创建和调整粒子发射参数

##### 属性

- `List<EmitParams> list`：发射参数的列表
  - get
- `EmitParams this[int index]`：列表项
  - get
  - set
- `int Count`：参数数量
  - get

##### 方法

- `EmitList(int num)`：创建指定长度的发射参数列表
  - `num`：发射粒子的数量
- `Cone(int num, float radius, float theta, float speed)`：圆锥生成器
  - 静态方法
  - `radius`：生成点与原点距离（圆锥母线长）
  - `theta`：圆锥半顶角（角度制）
  - `speed`：粒子初始速度
  - 返回`EmitList`：生成的参数列表
- `ConeFilter(float radius, float theta, float speed, OverlayMode mode = OverlayMode.COVER)`：圆锥过滤器
  - `radius`：生成点与原点距离（圆锥母线长）
  - `theta`：圆锥半顶角（角度制）
  - `speed`：粒子初始速度
  - `mode`：叠加方式
  - 返回`EmitList`：自身
- `SizeFilter(float size, OverlayMode mode = OverlayMode.COVER)`：尺寸过滤器
  - `size`：粒子大小
  - `mode`：叠加方式
  - 返回`EmitList`：自身
- `SizeFilter(float fromSize, float toSize, OverlayMode mode = OverlayMode.COVER)`：渐变尺寸过滤器
  - `fromSize`：起始大小
  - `toSize`：末尾大小
  - `mode`：叠加方式
  - 返回`EmitList`：自身
- `RandomSizeFilter(float min, float max, OverlayMode mode = OverlayMode.COVER)`：随机尺寸过滤器
  - `min`：最小尺寸
  - `max`：最大尺寸
  - `mode`：叠加方式
  - 返回`EmitList`：自身
- 重载运算符：（所有运算结果均为浅拷贝）
  - 与自身类型：+
  - 与`int`类型：*


#### `Storm` 类

风暴类，描述粒子行为

##### 属性

- `string name`：风暴的名字
  - get
  - set
- `bool sorted`：风暴行为是否已经排序完毕
  - get

##### 方法

- `Storm()`：创建一个空的`Storm`对象

- `Storm(string name)`：创建一个空的`Storm`对象
  - `name`：风暴的名字
- `Find(string name)`：根据名字获取一个已有的风暴
  - 静态方法
  - `name`：要获取的风暴名字
  - 返回`Storm`：获取到的风暴对象
- `AddBehavior(float startTime, EmitList emitList, Particle particle, float gap = 0)`：为风暴添加一个行为
  - `startTime`：行为开始时间，相对于风暴开始时间计算，单位秒
  - `emitList`：发射参数列表
  - `particle`：被发射的粒子
  - `gap`：单个粒子发射间隔，可选，默认0为同时发射所有粒子
  - 返回`Storm`：该风暴本身
- `Sort()`：排序风暴中的行为，为风暴的生成做好准备，即使不调用这个方法，风暴也会在生成前自动排序。如果希望不受到排序消耗时间影响，在调用`StormGenerator:Generate()`的瞬间开始生成风暴，您可以提前手动调用这个方法。

#### `StormGenerator` 类

风暴生成器，作为组件添加到`GameObject`上

- `Generate(Storm storm)`：生成风暴
  - `storm`：要生成的风暴

### `ParticleStorm.Script` 命名空间

#### `ParticleScript` 静态类

用于将方法注册为粒子脚本

##### 定义

- `delegate void Script(ParticleStatus particle)`：脚本代理
  - `particle`：储存粒子参数，用于脚本操作的对象

##### 方法

- `AddUpdateScript(Script script)`：注册一个每帧调用的脚本
  - `script`：用作脚本的函数
- `AddTrigger(Script script)`：注册一个作为触发的脚本
  - `script`：用作脚本的函数

#### `ParticleStatus` 类

表示粒子状态，用于脚本操作

##### 属性

- `Vector3 position`：粒子的坐标
  - get
  - set
- `Vector3 velocity`：粒子的速度
  - get
  - set
- `Vector3 rotation`：粒子的旋转
  - get
  - set
- `Vector3 angularVelocity`：粒子的旋转速度
  - get
  - set
- `float remainingLifetime`：粒子剩余存在时间
  - get
  - set
- `float startLifetime`：粒子初始存在时间
  - get
  - set
- `Color32 color`：粒子颜色
  - get
- `Color32 startColor`：粒子初始颜色
  - get
  - set
- `float size`：粒子尺寸
  - get
- `float startSize`：粒子初始尺寸
  - get
  - set
- `float meshIndex`：粒子使用模型序号
  - get
  - set

### `ParticleStorm.Util` 命名空间

#### `OverlayMode` 枚举类

`EmitList`中过滤器的叠加模式

- COVER：覆盖
- ADD：数值相加
- MINUS：数值相减
- MULTIPLY：乘算
- DIVIDE：除算
- AVERAGE：数值平均

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

#### `Settings` 静态类

全局设置

##### 属性

- `particleTag`：粒子对应游戏物体的默认Tag，必须是游戏中存在的Tag
- `useRightHandedCoordinateSystem`：是否使用右手系，若使用右手系，极坐标的y轴和z轴将会交换