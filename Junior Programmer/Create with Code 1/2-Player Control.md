# 摘要

在这个章节里，你将编写一个汽车的程序，让汽车从悬空着的公路的一端走向另一端，尝试绕过或者撞到路径中的障碍物。除了更好地熟悉Unity编辑区和工作流程外，你也能学到如何创建C#脚本，并进行简单的编程。在章节结束的时候，你将能够调用基本函数，声明和调整新变量，从而修改功能。



# Introduction

第一小节的任务是添加player control

player control即为玩家添加Input，并将Input转换为3D action。

任何游戏或者交互式体验都需要player control。



# Start your 3D Engines

在这一节，你将在Unity Hub中创建你的第一个游戏项目。你将选择和放置玩家要驾驶的载具，以及一个供载具避开或者撞击的障碍物。你同样会设置相机，这样玩家就可以在良好的视角看到场景。在整个流程中，你将学习如何在Unity Editor中导航，并习惯在3D场景中移动。最后，你将个性化自己喜欢的窗口布局。



## 创建课程文件夹，以及新项目

1. 创建文件夹**Create_with_Code**
2. 在Unity Hub中，使用**3D模板**，创建新的3D项目于这个文件夹，重命名为**Prototype_1**



## 导入资产并打开项目

1. 下载[Prototype 1 Starter Files](https://connect-prd-cdn.unity.com/20210923/c709e76b-3e93-4140-8675-f694b9f04399/Prototype 1 - Starter Files.zip)

2. 解压到项目中

   也可以解压后，在编辑器内，顶部菜单栏 > Asset > Import Package > Custom Package，选择解压出来的Unity Package文件

3. 删除3D模板默认的场景，打开下载的这个。

4. 查看场景内容

![image-20230215193224124](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230215193224124.png)



## 向场景中添加载具

1. 在Project > Assets > Course Library > Vehicles中，选自己喜欢的载具添加到场景中
2. 在场景中进行导航



## 向场景中添加障碍物

1. Course Library > Obstacles，选一个障碍物拖入，重命名为Obstacle
2. Reset Position，设置Postion为0, 0, 25

![image-20230215195852976](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230215195852976.png)



## 定位相机、运行游戏

1. 在层级中选中Main Camera，鼠标放在Scene内，按F定位相机
2. 运行游戏检查。

> 运行游戏的快捷键：Ctrl + P



## 将相机移动到载具后面

1. 使用Position和Rotate工具将Main Camera移动到载具后面
2. 可以按住Ctrl，这样移动的时候会按单位移动。
3. 通过右上角的坐标轴可以调整视角，方便调整位置



## 自定义窗口布局

将布局改为Tall（或者其他你习惯的布局）



# Pedal to the Metal

在这一小节，你将让场景动起来。首先你会编写简单的C#代码，修改载具的位置，让它能够前进。接着，你将为对象添加物理组件，使得它们可以相互碰撞。最后，你将学习如何在层级中复制对象，并将复制的对象放置在路面上。



## 创建和应用脚本

1. 创建一个新脚本，命名为PlayerController
2. 将这个脚本应用到载具上

> 顺带我个人不是很建议用视频这种拖入的方法，Hierarchy里面对象太多的时候容易拖歪
>
> 如果真不小心命名错了的话，不要改名字，直接删掉重新创建，改名字会有很多奇奇怪怪的问题



## 在Update()方法中添加一行注释（comment）

> 我突然意识到这个教程面向的对象甚至是没有任何编程基础的人
>
> 但他没教你怎么绑IDE来着。。。

1. 双击在IDE打开PlayerController

2. 在Update()方法中，写一行注释

   ```c#
   void Update()
   {
   // Move the vehicle forward
   }
   ```



## 让载具前进

1. 使用transform组件的Translate()方法

2. 使用三个float作为参数传入

   > 有关Unity类方法的功能及传参，可以参考IDE的代码提示（也是Document)。![image-20230216150354400](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216150354400.png)

   ```C#
   void Update()
   {
   // Move the vehicle forward
       transform.Translate(0, 0, 1);
   }
   ```

3. IDE内保存，编辑器内运行测试



## 使用Vector3

Vector3.forward相当于(0, 0, 1)

![image-20230216151318691](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216151318691.png)

用这段可读性更高的代码代替上面的

```c#
void Update()
{
// Move the vehicle forward
    transform.Translate(Vector3.forward);
}
```



## 调整载具速度

将速度调整为每秒，而不是每帧，因为每个人电脑帧率不一样。

Time.deltaTime返回每帧之间的秒数，这个值根据frames per second(FPS)的不同会变化。

![image-20230216151953086](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216151953086.png)

Console的输出是0.02

![image-20230216152711946](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216152711946.png)

通过在Vector3.forward上面乘deltaTime，可以做到每秒移动一个单位（在Unity内，相当于一米。），这个方法也是官方手册推荐的。

因为每秒一米还是太慢了，所以再调整一下。

```c#
void Update()
    {
        // Move Vehicle forward
        transform.Translate(Vector3.forward * Time.deltaTime * 20);
    }
```



## 添加刚体组件

1. 给载具和障碍物都加上刚体组件，Collider预制体自带了，不需要加。

   > 可以作为参考，障碍物和载具的Collider都是MeshCollider，绿线就是碰撞线
   >
   > ![image-20230216155320099](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216155320099.png)

2. 调整mass，Unity内的单位是千克。



## 复制并放置障碍物

1. 调整视角，复制足够多的箱子到路面上。
2. 运行并测试



# High Speed Chase

在这个教程中，你将为相机编写一个新的C#脚本，使得相机可以追踪载具，让玩家能更好地看到场景。你将使用程序中非常重要的一个概念——变量来做到这一点。

概述说实现方式是用一个变量存储载具位置，再用这个变量更新相机位置。



## 为载具添加速度变量

```c#
public float speed;
void Update()
    {
        // Move Vehicle forward
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }
```



## 为相机创建一个新脚本

1. 在相机里面添加一个新的脚本
2. 添加一个GameObject变量follow，用于引用场景中的其他GameObject
3. 将相机的postion，修改为follow的position

```c#
public GameObject follow;
void Update()
{
	transform.positon = follow.transform.position;
}
```



## 为相机位置设置偏移

如果只是赋值follow的position，相机会被摁到汽车里。

需要将最开始设置的相机的postion，作为一个偏移值加到position上（rotation不需要变化，所以不用调）

```c#
private Vector3 followOffset;
    // Start is called before the first frame update
    void Start()
    {
        followOffset = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.transform.position + followOffset;
    }
```

> 教程里硬编码了offset，这个方法不是很好，如果你需要调整相机初始位置的话，你需要同时调整相机的transform和这个硬编码的offset值，否则会出现视角瞬移之类的问题。所以我用了在Start()里面初始化的方法。



## 改用LateUpdate()更新相机位置

如果相机和载具都使用Update()更新位置的话，可能出现相机先移动，载具才移动的情况，这样会导致视角抖动。

可以使用LateUpdate()代替Update()方法，来保证相机在载具之后移动。

```c#
private Vector3 followOffset;
    // Start is called before the first frame update
    void Start()
    {
        followOffset = transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = follow.transform.position + followOffset;
    }
```



# Step into the Driver's Seat

在这个教程中，我们将起步并操控载具。我们需要检测玩家按下箭头键，然后让载具根据这个输入加速或者转向。通过使用新方法，Vector类型和变量，你将能够让载具前进和后退，左转或右转。



## 让载具向左或向右移动

1. 添加public变量turnSpeed

2. 在Update()方法中，修改transform；使用Input.GetAxis获取输入（双引号内为Edit > Project Settings > Input Manager > Axes内的Input Name）

   ```c#
   public float horizontalInput;
   void Update()
       {
           // Move Vehicle right/left
           horizontalInput = Input.GetAxis("Horizontal");
           transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
           // Move Vehicle forward
           transform.Translate(Vector3.forward * Time.deltaTime * speed);
           
       }
   ```



## 控制载具的速度

```c#
public float verticalInput;
void Update()
    {
        // Move Vehicle right/left
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * Time.deltaTime * turnSpeed * horizontalInput);
        // Move Vehicle forward
        verticalInput = Input.GetAxis("Vertical");
        transform.Translate(Vector3.forward * Time.deltaTime * speed * verticalInput);
        
    }
```



## 让载具旋转，而不是滑动

![image-20230216223653155](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230216223653155.png)

```c#
transform.Rotate(Vector3.up, Time.deltaTime * turnSpeed * horizontalInput);
```



## 整理层级和代码

1. 创建一个空对象，命名为Obstacles，存储所有的障碍物。

2. 调整好脚本值之后，将后面不需要再修改的值改为private

   > 这其实也提示了一点，可以在调试的时候设public，然后调试好了再改成private防止误动



# Plane Programming

使用你所学的技能，模拟驾驶一架飞机穿过障碍物。你将获取用户的上升和下降输入，从而控制飞机升降。你同样需要让摄像机跟随飞机视角。



## 提示

当你导入项目的时候，你会发现它有很多bug，你要做的就是修复这些bug。

- 飞机向后移动

  > 改为向前移动

- 飞机速度太快

  > 改为可调速度
  >
  > 实际上是脚本里面没乘deltaTime

- 不能控制升降

  > 看了下，虽然getVerticalInput了，但是没应用到rotate上

- 相机不能跟随飞机

  > 两个问题，一个是follow没设置对象，另一个是offset没设值

- 螺旋桨不转

  > 在飞机的自对象Propeller上新建脚本PropellerSpin
  >
  > rotate tool确定旋转轴为z轴
  >
  > Rotate(Vector3.forward * spinSpeed * Time.deltaTime);



## Project Design Document

在第一个实验课程中，你将学习创建一个成功独立项目必须的初步工作。首先，你将学习什么是独立项目、目的是什么，以及潜在的问题。接着，你将花些时间思考，找到想法并在设计手册中写出细节，包括什么完成特定功能的时间线。最后，你将用些时间绘制项目架构，帮助你可视化，并分享你的想法给他人。

你最终会完成设计手册，包括概念、时间线、最小可执行产品（Minimum Viable Product）的初步架构。



## 什么是个人项目

### 目标

1. 完成游戏：需要完成的内容，以及如何实现；
2. 知识，需要学习什么
3. 是否能够分享给其他人看。
4. 不需要创建什么很棒的，而是能够好好运行和游玩的。



## 研究设计手册示例

可以按手册条目分析其他人的游戏，研究它们是怎么创建的



# Share your work

在这个教程中，你可以提升和超越自己在本节的所学，并将你的作品分享给其他人。

下面有4个提升功能，分别为简单、中等、困难、专家。你可以选择任意数量的功能。



## 总览

简单和中等功能可以使用这节课程所学的知识完成，困难和专家级的功能则需要一些研究。

强烈建议先自己思考，如果你卡住了，底下会有解决方案。



## 简单：障碍物金字塔

创建堆叠的障碍物结构，使得撞进去的时候更有趣。

> 可以参考的形状：一竖列，一面墙、一个塔堆

用prefab来减轻工作量

![image-20230218141409166](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230218141409166.png)



## 中等：迎面驶来的载具

添加一些其他载具，自动从对面驶来，玩家也需要避开这些载具

> 1. 添加载具
>
> 2. 给载具添加RigidBody组件，设置mass
>
> 3. 给载具编写新的Script，使得载具可以前进
>
>    ```c#
>    class VehicleMoveForward : public MonoBehavoir
>    {
>        public float speed;
>        void Update()
>        {
>            transform.Translate(Vector3.forward * Time.deltaTime * speed);
>        }
>    }
>    ```
>
>    



## 困难：视角切换

允许玩家按键切换视角。

理想情况下，同一个key应当可以切换两个视角，一个是第三人称，另一个是第一人称。

> 简易状态机，获取输入后判断状态为firstPerson还是thirdPerson，分别设置状态。
>
> 需要两种offset，根据当前的offType，切换到另一种。
>
> 读取输入使用Input.GetKeyUp()返回值为bool
>
> 使用KeyUp的原因是，切换视角操作只需要执行一次，KeyDown和Key都会连续激活。

```c#
public class FollowPlayer : MonoBehaviour
{
    public GameObject follow;
    private Vector3 thirdPersonOffset = new Vector3(0, 5, -7);
    private Vector3 firstPersonOffset = new Vector3(0, 2, 1);
    public Vector3 offset;
    enum OffsetType { first, third };
    private OffsetType offType = OffsetType.third;
    
    // Start is called before the first frame update
    void Start()
    {
        offset = thirdPersonOffset;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // switch camera
        if(Input.GetKeyUp("f5")) // f5 to switch camera
        {
            if(offType == OffsetType.first) // current is first person
            {
                offset = thirdPersonOffset;
                offType = OffsetType.third; // switch to third person
            }
            else // current is third person 
            {
                offset = firstPersonOffset;
                offType = OffsetType.first; // switch to first person
            }
        }

        transform.position = follow.transform.position + offset;
    }
}
```



## 专家：本地多人游戏

首先需要将wasd和方向键分开。Name改成HorizontalP1和VerticalP1

![image-20230218154835663](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230218154835663.png)

右键 > Duplicate Array Element，复制后修改按键和名称。

![image-20230218155120519](D:\CS\Unity\Junior Programmer\Create with Code 1\2-Player Control.assets\image-20230218155120519.png)

复制一个载具，新建一个P2脚本应用到载具上。



## 提示

1. 简单挑战说要用Rigidody，但如果你是复制之前的障碍物，那就不用担心刚体的问题
2. 中等提示说使用transform，我觉得这里才是需要提示RigidBody的地方
3. 相机切换，提示说使用另一个相机，确实也是个好方法。
4. 本地多人，改Input Manager和相机视角，没啥问题。

