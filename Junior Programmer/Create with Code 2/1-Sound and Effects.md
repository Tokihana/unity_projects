# 摘要

在这一小节，你将编写一个快节奏无尽横向跑酷游戏，玩家需要时不时跳过障碍物，以免撞在上面。通过制作这个原型，你将学习如何添加音乐和声效，完善你项目中的体验。你同样会学习如何创建动态无限重复的背景，这对于横向游戏来说非常重要。最后，你将学习如何添加类似溅射或者爆炸之类的效果，这会使你的游戏更加有趣



# Introduction

你将学习应用动画、音效和粒子。



# Jump Force

这节棵的目标是为游戏原型设置基本的玩法。我们将从创建一个新项目并导入文件开始，接下来我们会选择一个漂亮的背景、以及一个玩家控制的角色，让这个角色在按下空格的时候跳起来。我们同样会为玩家选择一种障碍物，并创建一个Spawn Manager，用于隔一段时间在玩家面前丢出一个障碍物。

**目标**：

设置你选择的角色、背景和障碍物。玩家能够在障碍物生成在屏幕边缘、挡住道路的时候，按下空格让角色跳跃。



## 打开原型、修改背景

1. 在Unity Hub中创建Prototype3项目
2. 导入Prototype 3 Starter Files
3. 删除默认的Sample场景
4. 在层级中选中Background对象，在Sprite Renderer组件的Sprite属性中选择使用City，Nature或者Town背景图像



## 选择并设置玩家角色

1. Project > Course Library > Characters，选择一个角色并拖动到层级中，重命名为Player。调整transform。
2. 添加RigidBody和boxCollider组件
3. 创建PlayerController组件，绑定到Player上。



## 让玩家在开始的时候跳一下（RigidBody.AddForce())

1. 在PlayerController中声明`private Rigidbody playerRb`
2. 使用`GetComponent<ComponentName>()`获取RigidBody组件
3. 在Start()中，调用AddForce()，添加一个向上的力



## 微调跳跃力量和重力

首先计算下当前的跳跃高度和下落时间，当前设置的力为10.0f，mass = 1，使用Impulse方法，所以t = 1
$$
Ft = mv
\newline v = 10
$$
计算到达最大高度的时间t'，以及最大高度h
$$
gt' = v\newline
t' = 1\newline
h = \frac 1 2 g t'^2 = 5
$$
我们希望留空时间能够减少一半，即t'变为0.5，h不变，首先需要更大的重力
$$
h = \frac 12 g' t''^2
g' = 40
$$
重力需要变为4倍，为了能够跳到同样的高度，Impulse产生的初速度也需要变化，进而推出新的力大小
$$
v' = g't'' = 20\newline
F' = v' = 20
$$
综上，为了将留空时间减少一半，重力需要变为4倍，而初始力大小要变为2倍



1. 声明`private float gravityModifier`
2. 在Start()方法中，使用`Physics.gravity *= grabityModifier`修改全局重力。



## 防止玩家双跳

加一个bool检测玩家是否在地面上。

当玩家跳起时，设置IsOnGround = false。

当玩家发生碰撞时（现阶段，玩家只会和地面碰撞），设置IsOnGround = true;



## 制作一个障碍物，并让障碍物向左移动

1. Project > Asset > Course Library > Obstacle，选择一个障碍物添加到场景中
2. 给障碍物添加RigidBody和Coillider组件
3. 创建Prefabs文件夹，将这个障碍物保存为Original预制体
4. 创建MoveLeft脚本，绑定到障碍物上
5. 使用`transform.Translate(Vector3 position)`移动
6. 将MoveLeft也应用到Background上



## 创建Spawn Manager

1. 创建空对象，重命名为Spawn Manager
2. 创建SpawnManager脚本
3. 在脚本中声明GameObject，用于传入要生成的预制体；声明Vector3，设置生成障碍物的位置
4. 使用Instantiate方法生成障碍物



## 间隔一段时间生成一个障碍物

1. 使用InvokeRepeating调用SpawnObstacle。

2. 为了防止角色被障碍物撞飞，在Player的Rigidbody组件的Constrain属性中，将X轴和Z轴的position以及三轴的rotation都freeze掉。

   ![image-20230302232243300](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230302232243300.png)



# Make the World Whiz By

我们已经获得了游戏需要的核心机制：玩家可以按下空格跳过迎面而来的障碍物。尽管如此，玩家只在刚开始的几秒看上去在移动，接着背景就会消失。为了修复这个问题，我们需要让背景无缝重复，从而看起来像世界正在移动。我们同样需要在玩家碰到障碍物的时候暂停游戏，停止背景移动和障碍物生成。最后，我们需要销毁任何越过玩家位置的障碍物。

**目的**：背景要和障碍物同步无缝移动，障碍物要在离开游戏边界后销毁。通过使用脚本，背景和spawn manager将在玩家碰到障碍物的时候暂停。与障碍物碰撞会同时触发控制台消息“GameOver"。



## 创建脚本让背景重复

这个让背景重复的方法是基于背景可以被从中间划分为完全相同的两部分为基础的，例如项目中的这张背景：

![image-20230303172225780](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230303172225780.png)

可以注意到，Main Camera能够看到的位置只有前半张场景的范围

![image-20230303172310613](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230303172310613.png)

因此，可以在遍历到后半部分场景的时候，将整个场景后移到前半张场景相同的位置

1. 创建RepeatBackground脚本，应用到Background对象上。



## 重设background的位置

1. 声明`private Vector3 startPos`
2. 在Start()中初始化startPos
3. 在Update()中，使用if语句检测transform.position.x移动特定距离后，将transform.position改回startPos

这里的特定距离没法确定，下一小节就会讲如何获取背景长度，设置这个特定距离为长度的一半。



## 使用collider完善背景重复

1. 为背景添加Box Collider组件

2. ![image-20230305203152178](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230305203152178.png)

   Box Collider中的Size属性可以获取背景的长度

3. 将特定距离设置为size.x



## 添加Game Over触发

1. 在Inspector中添加Ground和Obstacle tag。

2. 在PlayerController中声明bool gameOver

3. 在OnCollisionEnter方法中，使用CompareTag判断碰撞的物体是地面还是障碍物。

   > 前面好像忘记给Obstacle加box collider了，顺手加上。



## 在game over的时候停止移动

1. 在MoveLeft脚本，声明`private PlayerController playerControllerScript`

   > 脚本间通讯，将脚本看作是一个类，声明对应类型的reference

2. 在Start()方法中，使用GameObject.Find()获取场景中的Player对象，使用`GetComponent<PlayerController>`获取Player对象的PlayerController组件、

   > 这一步的作用是初始化，为第一步的reference赋值

3. 使用PlayerController.gameOver判断是否游戏结束。

   > 脚本间通讯只能调用公有成员，如果gameOver是私有就无法访问
   >
   > 如果想保证gameOver不被访问（即不会被修改），可以在PlayerController中将gameOver声明为私有，然后编写公有函数返回gameOver



## 在game over的时候停止生成障碍物

1. 在SpawnManager脚本中，获取playerControllerScript 的reference
2. 使用if(!gameOver)控制SpawnObstacle()方法。



## 在障碍物离开边界的时候销毁它

1. 在MoveLeft脚本编写bounds判断
2. 注意，因为MoveLeft脚本也用于background移动，所以需要用CompareTag判断是否为Obstacle



# Don‘t Just Stand There - Animation Controller

我们的游戏看起来已经很接近完美了，但玩家的角色目前看起来有点没有生机。接下来，我们要为角色添加跑动、跳跃甚至死亡动画。我们同样会调整动画的速度，使得动画看起来更真实。

**目标**：通过使用animator controller中的动画，角色将有三个新动画，分别在不同的游戏状态下执行。这些状态包括跑动、跳跃和死亡。动画将能够平滑转换，并和游戏的整体速度协调。



## 研究玩家动画

1. 在层级中选中Player，双击Animator组件中的Controller属性，进入Animator视窗

![image-20230306152159946](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306152159946.png)

> Animation视窗导航：按住Alt + 左键拖动，鼠标滚轮缩放，或者按住鼠标滚轮拖动



## 让玩家跑起来

1. 在Animator视窗，双击Run_Static，检查进入该状态的条件

   ![image-20230306153212972](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153212972.png)

   > 选中箭头查看进入状态的条件。
   >
   > 进入Run_Static的条件为speed_f 大于0.5

2. 在Parameter栏，将Speed_f修改为1.0

   ![image-20230306153357836](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153357836.png)

3. 游戏开始后首先会进入Idle状态，然后切换到Walk_Static，再切换到Run_Static

   ![image-20230306153620319](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153620319.png)

   ![image-20230306153643071](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153643071.png)

   我们希望能够直接进入Run_Static，所以要将Entry直接连到Walk_Static上。

   右键Run_Static，点击Set as Layer Default State

   ![image-20230306153805326](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153805326.png)

   ![image-20230306153817339](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306153817339.png)

4. 单击Run_Static，调整speed属性，让动画速度和背景移动速度匹配。

   ![image-20230306154030840](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306154030840.png)



## 设置跳跃动画

我们希望角色在跳跃的时候播放跳跃动画，即，将动作转为Jump。

1. 首先在Animator > Layers中找一下是否有这样的动作，可以从Run转换到Jump

   ![image-20230306155134035](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306155134035.png)

2. 然后，检查转换条件

   ![image-20230306155216005](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306155216005.png)

3. 在Parameter中，寻找Jump_trig这个条件

   ![image-20230306155304785](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306155304785.png)

   从后面的图标，以及命名来推测，这个参数应该是个Trigger，需要调用SetTrigger方法

4. 在PlayerController脚本中，首先获取Player的Animator组件引用

   ```c#
   private Animator playerAnim;
   playerAnim = GetComponent<Animator>();
   ```

5. 调整控制跳跃的代码段，在跳跃的时候，调用SetTrigger(string name)

   ```
   playerAnim.SetTrigger("Jump_trig");
   ```



## 调整跳跃动画

1. 在这之前，角色的mass一直都是默认值1，现在给它调一个合适的体重，例如60

2. 调整体重之后，为了保证跳跃高度还和之前一致，需要调整AddForce

   - 一种方法是改变ForceMode，设置为VelocityChange，这样mass会使用默认值1，不管体重多少，跳跃高度不会变化
   - 另一种方法是加大force，根据ft = mv，v想要维持之前的大小，则f需要增大60倍

3. 微调跳跃的speed，使得落地的时候开始Run

   ![image-20230306161239687](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306161239687.png)

   



## 设置倒地动画

1. 查看Death动画的触发条件

   ![image-20230306171149292](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306171149292.png)

2. 在PlayerController的OnCollisionEnter()方法中，修改game over的片段

   ```
   playerAnim.SetBool("Death_b", true);
   playerAnim.SetInteger("DeathType_int", 1);
   ```



## 在game over的时候禁用跳跃

玩家在倒地后应该不能进行任何操作，在这个demo中，玩家能够进行的操作只有跳跃，所以可以在if条件中加上!gameover，从而防止下图的情况发生

![image-20230306171556390](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230306171556390.png)



# Particles and Sound Effects

我们的游戏已经看上去非常不错了，但还缺少某些东西：声效和粒子。声效和音乐能够让安静的游戏世界变得鲜活，而粒子能够让玩家的动作更加有动感和引人注目。在这一小节，我们会为玩家的跑动、跳跃和翻车添加一些炫酷的音效和粒子

**目标**：玩家跑动的时候播放音乐，卷起尘土粒子。当跳跃的时候，播放有弹性的音效，当撞到障碍物的时候，播放爆炸音效，并且在倒下的同时显示烟幕。



## 制作爆炸粒子

1. Course Library > Particles，将FX_Explosion_Smoke拖入层级中，使用play / restart / stop来预览粒子效果
2. 在particle system属性中调整粒子效果
3. 不要选中play on awake属性，这个属性会让粒子在开始时就播放
4. 将粒子拖入Player中，作为Player的子对象



## 在碰撞的时候触发粒子效果

1. 在PlayerController脚本中，声明`public PraticleSystem explosionParticle`
2. 在Inspector中，将烟雾粒子作为引用传入
3. 在判断玩家撞到障碍物的if语句中，调用`explosionParticle.Play()`，测试并调整粒子属性。



## 添加泥土飞溅效果

为了让玩家能够在快速穿越场景的时候，看起来像踩在地面上，我们需要一个泥土飞溅粒子。需要注意的是，这个效果只应该在角色跑动的时候播放（也就是说跳起来和gameover的时候要Stop()）

1. 将FX_DirtSplatter拖入层级中，作为Player的子对象
2. 声明`public ParticleSystem dirtParticle`，在Inspector中分配引用
3. 当玩家跳起来或者撞到障碍物的时候，调用`.Stop()`
4. 当玩家接触到地面的时候，调用`.Play()`



## 为camera添加音乐

1. 选中Camera对象，Add Component > Audio Source
2. 在Course Library > Sound中，选择想要的音乐
3. 适当调整音量
4. 选中Loop，使得音乐可以循环播放





## 为音频切片（Audio Clip）声明变量

1. 在PlayerController脚本中，声明`public AudioClip jumpSound`和`public AudioClip crashSound`
2. 在编辑器中，为这两个变量分配音频



## 在跳跃或碰到障碍物的时候播放音频切片

1. 为Player添加Audio Source组件
2. 在PlayerController中声明`private AudioSource playerAudio`，在Start()方法中通过GetComponent<>()初始化
3. 在跳跃的时候调用`playerAudio.PlayOneShot(jumpSound, 1.0f)`
4. 在碰撞的时候调用`playerAudio.PlayOneShot(crashSound,1.0f);`



# 挑战3：气球、炸弹和布尔

运用你的物理、屏幕滚动和特效知识，使一个气球飞跃小镇，拾取掉落物并避开爆炸物。你需要分析并解决很多问题，因为这个项目充满错误。



## 总览

1. 打开Prototype3项目
2. 点击并下载Challenge 3 starter files



## 提示

当你导入这个挑战的时候，里面应该有很多bug

这些bug会在下面列出，挑战的目标是修复这些bug。如果你没有思路，页面的最底部有一些提示。

如果你不能修复这些bug，并且想要删除挑战文件，在Project窗口中，Assets > Challenge 3，右键删除



## 玩家无法控制气球

检查下playercontroller的Update()方法，先写个Debug.Log()看能不能正确进入if语句

![image-20230307145513282](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307145513282.png)

![image-20230307145533485](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307145533485.png)

怪，沿着运行逻辑走一遍看看。

首先声明rigidbody变量

![image-20230307145803007](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307145803007.png)

然后变量初始化，嗯哼？Start()里面没有初始化

把变量声明改成public运行下看看。

![image-20230307145923416](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307145923416.png)

啊哈，确实没初始化rb，给它加上

![image-20230307150100130](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150100130.png)

测试，没问题了。



## 背景只在game over的时候移动

我猜是没加NOT，看一眼moveleft

![image-20230307150211300](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150211300.png)

确实没加。



## 没有生成对象

spawnManager的问题，估计是没传引用

![image-20230307150355474](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150355474.png)

传了。。。看一眼脚本里面

![image-20230307150434111](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150434111.png)

SpawnObjects方法为啥没被调用。。。看看InvokeRepeat

![image-20230307150457690](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150457690.png)

哈哈哈哈，打错字了可还行。

改过来，OK，能正常生成了



## 烟火特效在气球旁边产生

位置错了，调回来就好![image-20230307150643272](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150643272.png)



## 背景重复的不是很好

看一眼repeatBackground

![image-20230307150831859](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230307150831859.png)

repeatWidth设成y了，改成x就好



## 气球会飞太高

加个边界

```c#
if(transform.position.y > floatBound)
        {
            Debug.Log("current y is " + transform.position.y);
            transform.position = new Vector3(transform.position.x, floatBound, transform.position.z);
        }
```

这个加边界的方法不是很好，因为添加上升力用的AddForce这样加边界会让气球一直卡在边界上

判断应该加在AddForce那里，只有没超过一定高度才可以加浮力





## 气球会飞太低

要求气球可以在地面弹起来，并且发出效果音。

那应该得加collision。

先将Ground的tag调整为Ground，然后再OnCollisionEnter中编写CompareTag("Ground")

Ground有Mesh Collider，所以直接写代码就行。

要求气球弹起来，那么可以考虑AddFoce。

气球弹跳高度想设为2，那么
$$
h = 2 = \frac 12gt^2\newline
v = gt\newline
f = v\space(t = 1, m = 1)\newline
综上，bounceForce应该为6左右
$$


发出效果音可以用AudioClip和AudioSource



# Lab3: Player Control

在这一小节，你会编写基本的玩家移动，包括限制移动的代码。由于不同类型的项目中玩家的的移动方式不同，你不需要完全按照教程中的步骤来。你需要调查和引用其他的代码，并修复一些可能出现的bug。

**目标**：玩家可以通过按键移动，但不会移动到不该移动的位置



## 创建PlayerController，规划代码

1. 选中Player，添加Rigidbody组件

   > 如果需要trigger或者collision，那么就必须有Rigidbody组件

2. 创建Scripts文件夹，创建PlayerController脚本并打开。

3. 瞄一眼之前做的设计文档

   ![image-20230310172550267](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230310172550267.png)

4. 可以将playerControl分为movement和fire两种。方向键移动，zx攻击，shift降速。



## 根据用户输入做出基本移动

1. 声明`private float speed`
2. 如果需要使用物理，声明`private Rigidbody playerRb`
3. 使用GetAxis()和GetKey()获取输入
4. 使用Translate或者AddForce进行移动



## 限制玩家的移动

1. 如果应用了Rigidbody，且存在一些不应该和玩家碰撞的物体，记得给这些物体的Collider应用IsTrigger
2. 如果玩家的position或者rotation需要受到约束，在Rigidbody的constraints里面设置
3. 如果不想让玩家跑到范围外，用if语句限制住
4. 如果设置跳跃，同时想要禁止玩家双跳，可以用一个bool控制（和prototype3的方法类似）
5. 如果玩家需要被实际的物体挡住，可以拉cube或者plane



1. 设置边界

   ```c#
   public class PlayerController : MonoBehaviour
   {
       // player movement bounds
       public float xBounds = 10.0f;
       public float zBounds = 10.0f;
   
       // Update is called once per frame
       void Update()
       {
           // keep player's position in bounds
           if((transform.position.x > 0 ? transform.position.x : -transform.position.x) > xBounds)
           {
               transform.position = new Vector3(transform.position.x > 0 ? xBounds : -xBounds, transform.position.y, transform.position.z);
           }
           if((transform.position.z > 0 ? transform.position.z : -transform.position.z) > zBounds)
           {
               transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z > 0 ? zBounds : -zBounds);
           }
       }
   }
   ```

2. 因为不需要旋转和y轴移动，所以禁掉

   ![image-20230310182012857](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230310182012857.png)

3. 把重力也去掉

   ![image-20230310182210670](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230310182210670.png)

4. 在按下Shift的期间减速



## 整理代码和导出备份

1. 创建新的空对象来嵌套场景中的物体
2. 使用MovePlayer()方法和ConstrainPlayerPosition()封装代码
3. 添加注释
4. 测试并保存
5. 右键Assert文件夹 > Export Package，保存新的备份





# 附加功能3 - 分享你的项目

在这个教程中，你可以回顾和进一步深入你在本章所学的内容，并将你的项目分享给其他创作者。

下面有四个附加功能，难度分别为简单、中等、困难和专家，你可以任选几项功能加以实现。



## 总览

- 简单：随机生成障碍物
- 中等：双跳
- 困难：冲刺和计分
- 专家：游戏开始动画



## 简单：随机生成障碍物

生成多个种类的障碍物（可以不同高度）。

首先创建更多障碍物预制体，修改tag为Obstacle。

1. 拉一个新障碍物到场景中，添加Box Collider和Rigidbody组件
2. 添加MoveLeft脚本
3. 拖入到Prefab文件夹 > Original Prefab

为了给后面双跳做准备，创建个高一点的障碍物

![image-20230311155547865](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311155547865.png)



修改SpawnManager，用数组接收预制体引用，用Random.Range(0, .Length);随机生成。



## 中等：双跳

首先整理下代码，将跳跃封装为有PlayerJump()函数。

![image-20230311160949815](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311160949815.png)

分析包含双跳后的跳跃逻辑：

```flow
st=>start: 开始
i=>inputoutput: 按下空格
isover=>condition: Game Over?
isground=>condition: Is On Ground?
isDjump=>condition: Is Double Jump?
canJump=>inputoutput: 单跳
notJump=>inputoutput: 不可以跳
doubleJump=>inputoutput: 双跳
end=>end: 结束

st->i
i->isover
isover(yes)->notJump
isover(no)->isground
isground(yes)->canJump
isground(no)->isDjump
isDjump(yes)->notJump
isDjump(no)->doubleJump
canJump->end
notJump->end
doubleJump->end

```



因为判断逻辑变得挺复杂了，所以直接用一个函数封装

```c#
bool CheckJump()
    {
        if (!gameOver && isOnGround)
        {
            return true;
        }
        else if(!gameOver && !isOnGround && !isDoubleJump)
        {
            isDoubleJump= true;
            return true;
        }
        return false;
    }
```



测试，发现一个问题，如果在下落的过程中双跳，跳跃高度会大跌，推测是此时存在向下的速度，所以被抵消掉了。

考虑在跳跃之前先消掉y轴速度：

```c#
playerRb.velocity = new Vector3(playerRb.velocity.x, 0, playerRb.velocity.z);
```



另外还发现一个问题，因为高一点的障碍物是两个矮障碍物的预制体叠起来做成的，有可能发生两次碰撞，导致死亡动画和粒子播放两次；另一种情况是玩家死亡后会触地，再次触发奔跑粒子，所以要给所有的collision判断那里加个`!gameOver`。



## 困难：加速能力和计分板

在玩家按下某个按键的时候加速；在Debug中显示玩家的分数，如果在加速模式下，每越过一个障碍物都可以获得双倍分数。

可能需要创建GameManager?

加速功能需要调整MoveLeft和PlayerController的动画速度，以及计分板的分值。



首先实现计分板，思路是当障碍物走到玩家身后（score bound）的时候加一分。这里用一个稍微偷懒点的办法，在Destory()的时候加分



加速考虑在MoveLeft中实现

```c#
if (Input.GetKey(KeyCode.LeftShift))
        {
            SpeedUp();
            isDash = true;
        }
        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            SpeedDown();
            isDash = false;
        }
```



## 专家：游戏开始动画

让玩家从屏幕左边走入场景中，到达位置后才开始跑动。使得玩家有一点反应时间。

不清楚怎么做计时，所以直接看step by step了。



## Step By Step

- 简单难度的实现方法是一样的。
- 双跳的实现也是用了个bool，不过单独设个了双跳的力度，确实，双跳可能高度不一样。
- 加速的实现是在PlayerController和MoveLeft里分别处理的，
  在PlayerController里面加速动画，在MoveLeft里面调整速度。
  然后创建GameManager用来计分



### 加速实现

在Animator中新建Speed_Multiplayer参数，调整这个参数来控制播放速度

![image-20230311182803507](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311182803507.png)

![image-20230311182812899](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311182812899.png)

为玩家动作设置Multiplayer（设置run_static和run_jump就行）

![image-20230311182849772](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311182849772.png)

使用GetKey()，在按住按键的时候加速，并设置`doubleSpeed = true`，当没有按住按键（doubleSpeed = true）的时候，调回原样

```c#
if(Input.GetKey(KeyCode.LeftShift))
        {
            doubleSpeed = true;
            animator.SetFloat("Speed_Multiplier", 2.0f);
        }
        else if (doubleSpeed)
        {
            doubleSpeed = false;
            animator.SetFloat("Speed_Multiplier", 1.0f);
        }
```



在MoveLeft脚本中，调用PlayerController的doubleSpeed判断是否加速

```c#
if(controller.doubleSpeed)
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime * 2); // move to left
            }
            else
            {
                transform.Translate(Vector3.left * speed * Time.deltaTime); // move to left
            }
```



### 开场动画实现

1. 打开GameManager脚本，声明

   ```c#
   public Transform startingPoint; 
   public float lerpSpeed;
   ```

2. 在Start()方法中，设置playerController.gameOver = true，防止玩家乱动

3. 调用StartCoroutine(PlayIntro())开始协程

   > ```c#
   > public Coroutine StartCoroutine(IEnumerator routine);
   > ```
   >
   > coroutine，协程，可以用来设计在几帧内的动作
   >
   > 看了眼中文手册，协程就像个函数。但与函数不同的是，函数调用后会运行到完成状态然后返回，即函数中的动作都需要单帧内完成。而协程不是这样，协程能够暂停执行并将控制权返还给Unity，并在下一帧继续执行。
   >
   > 协程通常用来执行需要多帧连续更新的任务，例如逐渐淡入淡出，或者逐渐移动位置。
   >
   > 简单地说可以用来当计时器。相比于每帧都调用一次，协程可以更方便控制调用时序，减少开销。

   

4. 创建PlayIntro()协程

   ```c#
       IEnumerator PlayIntro()
       {
           Vector3 startPos = playerController.transform.position;
           Vector3 endPos = startingPoint.position;
           float journeyLength = Vector3.Distance(startPos, endPos);
           float startTime = Time.time;
   
           float distanceCovered = (Time.time - startTime) / lerpSpeed;
           float fractionOfJourney = distanceCovered / journeyLength;
   
           playerController.GetComponent<Animator>().SetFloat("Speed_Multiplayer", 0.5f); // 设置步行
   
           while (fractionOfJourney < 1)
           {
               distanceCovered = (Time.time - startTime) * lerpSpeed; // 当前已经进行了多少
               fractionOfJourney = distanceCovered / journeyLength; // 进度比例
               playerController.transform.position = Vector3.Lerp(startPos, endPos, fractionOfJourney); // Lerp()是线性变换
               yield return null; // 返回yield
           }
           playerController.GetComponent<Animator>().SetFloat("Speed_Multiplayer", 1.0f); // 设置跑步
           playerController.gameOver= false; // 开始游戏
       }
   ```

5. 创建StartPoint对象，重设position(0,0,0)，传入GameManager

   ![image-20230311233234341](D:\CS\Unity\Junior Programmer\Create with Code 2\1-Sound and Effects.assets\image-20230311233234341.png)

6. 将Player的位置设为（-5,0,0)



