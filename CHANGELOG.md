## [1.4.1](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.4.0...v1.4.1) (2020-12-09)


### Bug Fixes

* add experimental module AroundAxisModule to repair some function loss in deflection module ([7521702](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/75217029099e6e55a3b0a16eb0ada0215bff7257))

# [1.4.0](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.3.0...v1.4.0) (2020-12-08)


### Features

* add some new modules to bullet systems ([792e09b](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/792e09b596bdd2763669dd62a66b8102e7e158b9))
* tracing module can edit rate with curve ([851fb25](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/851fb25259c539030c154d8913eb2cf57eccd842))

# [1.3.0](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.2.3...v1.3.0) (2020-11-28)


### Bug Fixes

* [#25](https://github.com/SUSTech-CANStudio/bullet-storm-unity/issues/25) ([cec3cfd](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/cec3cfd994724c8d58ae5d9e8e927d5ddc64c9ef))
* allow find target after emitter and bullet system initialized ([5e82ca0](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/5e82ca022b74086257325f2cafe6051bd402025a))
* bullets don't update in Unity 2019.4.13f1c1 ([ada421f](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/ada421f3ffae3a0df3f54ddfa0442200f2dbfdc5))


### Features

* add onEmit event in auto emitters ([a4a944a](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/a4a944af93b99cb27d6e1442fd9339ab7793ffd9))
* allow repeat emissions in auto shape emitter ([5be60bb](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/5be60bb04454dda86f248159f6fdf9d100ec6a8d))
* show total time in auto shape emitter ([80a281e](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/80a281e84a709d14b6f6ee3e69d5e1a61912a50a))

## [1.2.3](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.2.2...v1.2.3) (2020-11-08)


### Bug Fixes

* no error message for shape not set in AutoShapeEmitter ([6145c20](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/6145c20b4adf87f8282c569872621f31c117a5f8))

## [1.2.2](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.2.1...v1.2.2) (2020-11-04)


### Bug Fixes

* [#22](https://github.com/SUSTech-CANStudio/bullet-storm-unity/issues/22) ([f8b0537](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/f8b053783c6b80ed8f1116c4e149a4a23625e42c))
* null reference exception when destroy emitter during emission ([48c2d62](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/48c2d62a71155b618da97206c04f0727fff1b70d))

## [1.2.1](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.2.0...v1.2.1) (2020-10-31)


### Bug Fixes

* extral interval in shape emitter one by one mode ([cfc0203](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/cfc02039c113877d73b39b8d47d1105bb4da494b))

# [1.2.0](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.1.1...v1.2.0) (2020-10-30)


### Bug Fixes

* line generator doesn't work correctly ([7919554](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/7919554bd0b5be8f59e07b7ea6dab18e68334799))


### Features

* add math nodes ([0306879](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/030687994aa5f76580e891aa383f98bf649a1fa0))
* enable refer shape asset in node ([c9211f1](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/c9211f1f48643f4a3d12b6c8ffb37cfc3cbd38a9))
* node graph for shape asset ([cc2d17f](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/cc2d17f4e41e8ad3e9f6e00b46a3270b1d9aec9f))

## [1.1.1](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.1.0...v1.1.1) (2020-10-23)


### Bug Fixes

* scoped static method doesn't supported in Unity 2019 ([89f86f7](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/89f86f73305d7640b540b7a4d48bae6d264ef87a))

# [1.1.0](https://github.com/SUSTech-CANStudio/bullet-storm-unity/compare/v1.0.0...v1.1.0) (2020-10-23)


### Bug Fixes

* null reference error when emitter created but not started ([29c3410](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/29c34106bb68fe6f9be6ff8d2ed276f99d1f6ce8))
* null reference error when turn on aim offset module in play mode ([3d38f46](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/3d38f460c07c8b824502b06411946a5f8a7f0339))


### Features

* add auto shape emitter ([4b9f271](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/4b9f27197ad0ac458cdbce56e8b6ff6b65056f4a))
* add process bar on emitters ([41648a9](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/41648a939556e31befed6cdb120b1b8e406a42bf))
* complete auto bullet emitter for basic bullet emissions ([b729c07](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/b729c0720bb438890e493b642242da654c036a88))

# 1.0.0 (2020-10-05)


### Bug Fixes

* github workflow ([fba9249](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/fba92498c7a1bb57b1b8089b60cfe5b3a0336d95))
* Samples => Samples~ ([771a32c](https://github.com/SUSTech-CANStudio/bullet-storm-unity/commit/771a32c6cf9c896d5509c57af51a001d6a4542fe))
