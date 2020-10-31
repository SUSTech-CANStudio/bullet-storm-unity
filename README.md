# BulletStorm

[![Codacy Badge](https://app.codacy.com/project/badge/Grade/7837d955c0a24890b8e54ecda0768576)](https://www.codacy.com/gh/SUSTech-CANStudio/bullet-storm-unity/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=SUSTech-CANStudio/bullet-storm-unity&amp;utm_campaign=Badge_Grade)[![openupm](https://img.shields.io/npm/v/com.canstudio.bullet-storm?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.canstudio.bullet-storm/)

**点击查看[中文介绍](README-zh-CN.md)**

BulletStorm is an editor tool for *danmaku* (or say barrage) design in 3D STG games.

It provides full graphic user interface for your workflow, from emission to runtime control. You can easily design danmaku in your game without code knowledge base.

BulletStorm is a high level danmaku manage system, it doesn't care about how the danmaku is implemented. Every class (particle system, game object, or anything else) that implements `BulletSystem.IBulletSystem` can be a danmaku implementation. However, BulletStorm provides support for Unity build-in particle system and GameObject, so you don't need to implement them by yourself.

## Installation

BulletStorm is easy to install, you can use any of following methods to install it.

### OpenUPM (Recommended)

1. If you are new to OpenUPM, install [openupm-cli](https://github.com/openupm/openupm-cli#installation) first.

2. Go to your Unity project root folder (you can find an `Assets` folder under it), run this command:

   ```shell
   openupm add com.canstudio.bullet-storm
   ```

3. Open your Unity editor, BulletStorm should be installed successfully.

### UPM

1. If you haven't installed Git, download and install it here: [download Git](https://git-scm.com/downloads)

2. Open your Unity editor, open `Window -> Package Manager` in the toolbar.

3. In Package Manager, click `+ -> add package from git URL` in the top left corner.

4. Add following packages:

   `"com.dbrizov.naughtyattributes": "https://github.com/dbrizov/NaughtyAttributes.git#upm"`

   `"com.github.siccity.xnode": "https://github.com/siccity/xNode.git"`

   `"com.canstudio.bullet-storm": "https://github.com/SUSTech-CANStudio/bullet-storm-unity.git#upm"`

### Use this as template

If you are going to create a new project, you can simply use this repository as template.

1. Download source code in master branch with your favorite method.
2. Open it as Unity project.

## Overview

### Shape Editor

![image-shape-test](docs/img/image-shape-test.png)

### Auto Emitters

![image-20201031151015657](docs/img/image-20201031151015657.png)

![image-20201031151244333](docs/img/image-20201031151244333.png)

### Build-in Particle System Support

![image-20201031151629973](docs/img/image-20201031151629973.png)
