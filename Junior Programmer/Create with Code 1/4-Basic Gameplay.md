# Introduction

本节讲Non-Player and Player Ability，又叫basic Gameplay



# Player Positioning

你将从创建一个新的项目开始你的第二个原型，并应用基础的玩家移动。首先你会选择你要使用的角色，想使用的动物，以及想要喂给动物的事务。你将令玩家拥有基本的从一端移动到另一端的能力，正如你在第一个原型中所做的那样，不同的是，你会使用if语句来保证玩家始终在范围内。



## Create Project

1. 创建新项目，命名为Prototype2
2. import package导入项目包
3. 打开Prototype2场景



## 添加玩家，动物和食物

1. 打开Course Library文件夹，拖入1个Human, 3个动物和1个食物
2. 将角色重命名为“Player”
3. 重新排布动物和食物的位置



## PlayerController

1. 新建脚本PlayerController，加到角色上
2. 读取Horizontal输入，设置水平方向移动

```c#
public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed;
    
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);
    }
}
```



## 让玩家一直在范围内

1. 获取玩家位置的x值：`transform.position.x`
2. 让这个值保证在-10和10之间。

```c#
if(transform.position.x < -10)
        {
            transform.position = new Vector3(-10, transform.position.y, transform.position.z);
        }    
        if(transform.position.x > 10)
        {
            transform.position = new Vector3(10, transform.position.y, transform.position.z);
        }
```



## 整理代码

```c#
public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float speed;
    public float xRange;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.x < -xRange)
        {
            transform.position = new Vector3(-xRange, transform.position.y, transform.position.z);
        }    
        if(transform.position.x > xRange)
        {
            transform.position = new Vector3(xRange, transform.position.y, transform.position.z);
        }
        horizontalInput = Input.GetAxis("Horizontal");
        transform.Translate(Vector3.right * speed * Time.deltaTime * horizontalInput);
    }
}
```



# Food Flight

在这一节吗，你将让玩家能够发射投射物。首先你会编写一个新脚本，使投射物向前移动。接着，你会将这个投射物保存为预制体。玩家将按空格键发射这个预制体。最后，你将为场景添加边界，从而删除离开屏幕范围的物体。

目标：玩家将能够按空格发射投射物预制体，这些预制体会在离开游戏边界后销毁。动物也会在离开边界后销毁。



## 让食物向前飞

1. 为食物添加新的脚本，命名为MoveForward

2. ```c#
   transform.Translate(Vector3.forward * Time.deltaTime* speed);
   ```



## 将投射物变为预制体

1. 新建Prefabs文件夹，将层级中的食物拖进去，创建为Original Prefab。

2. 在PlayerController脚本中，声明新的`public GameObject projectilePrefab;`

3. 从**Prefabs文件夹**拖动预制体到Player的Inspector中，建立引用关系。

   > 这里一定要传入预制体，而不是场景中那个实例，那个实例后面要删掉的



## 按下按键发射预制体

使用Input.GetKeyDown()获取输入，使用Instantiate()创建预制体的Clone。

Instantiate()方法用和编辑器中复制同样的办法创建对象的一个拷贝，对象可以是**GameObject或者Component**，可以指定position和rotation，对象的子对象同样会被拷贝。需要注意的是，对于子对象的子（嵌套子），Unity会限制拷贝，从而防止堆栈溢出。

```c#
// 在运行时创建预制体实例
public GameObject prefab;
Instantiate(prefab, new Vector3(0,0,0), rotation);
```

更多有关运行时创建预制体参考[Instantiating Prefabs at run time](https://docs.unity.cn/2021.3/Documentation/Manual/InstantiatingPrefabs.html)



## 将动物变成预制体

1. 将动物旋转180
2. 在层级选中所有的动物，挂上MoveForward脚本
3. 适当调整不同动物的速度
4. 将动物变成Original Prefab



## 销毁离开屏幕的投射物与动物

使用Destory()方法销毁GameObject。

```c#
public class DestroyOutBounds : MonoBehaviour
{
    private float topBounds = 35.0f;
    private float bottomBounds = -15.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.z > topBounds)
        {
            Destroy(gameObject);
        }
        else if(transform.position.z < bottomBounds)
        {
            Destroy(gameObject); 
        }
    }
}
```



## 多个投射物

```c#
// 可以通过for循环，调整Instantiate()方法中的position和rotation，从而投射多个投射物
for(int i = 0; i <= 2; ++i)
{
	for(int j = -2; j <= 2; ++j)
         Instantiate(projectilePrefab, transform.position + new Vector3(0,0,i), Quaternion.Euler(0, 10 * j, 0));
}
```



# Random Animal Stampede

尽管动物预制体已经可以走过屏幕，在底部被销毁，这些预制体仍然需要我们手动拖入场景中。这个教程我们会让动物自动生成在屏幕顶端的随机位置。我们需要创建一个新对象和新脚本来管理整个生成过程。

**目标**：当玩家按下S键的时候，屏幕顶端会随机生成一种动物在随机位置，从上走到下。

使用Array（数组）存储每个动物，Random.range()获得随机数。



## 创建生成管理器（spawn manager）

1. 在层级中，创建空，命名为SpawnManager
2. 创建脚本，命名为SpawnManager
3. 声明GameObject数组：`public GameObject[] animalPrefabs`
4. 在层级中，修改Size为3，将三种动物存进去



## 按下S召唤动物

1. 在Update()方法中，GetKeyDown(KeyCode.S)，Instantiate()召唤一个固定Index的动物

   ```c#
   int animalIndex = Random.Range(0, animalPrefabs.Length);
   ```



## 随机位置召唤

1. 用Random.Range()作为召唤位置的x值。
2. 创建public变量方便修改range。

```c#
public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    public int spawnRangeX = 15; // Random.Range()的传参可以是int也可以是float
    public float spawnPosZ = 20.0f;
    
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.S))
        {
            int animalIndex = Random.Range(0, animalPrefabs.Length);
            Vector3 spawnTransform = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
            Instantiate(animalPrefabs[animalIndex], spawnTransform, animalPrefabs[animalIndex].transform.rotation);
        }
    }
}

```



## 修改相机视角（Perspective和Isometric）

意思是修改投影（projection）

层级中选中Main Camera，Inspector > Projection > Orthographic



# 碰撞判定

我们的游戏就快完成了，但在结束之前，还需要添加一些很重要的部分。首先，我们需要一个计时器定时生成动物，而不是按下S键。接着，我们要为预制体添加碰撞，使得它们能够被投射物破坏。最后，我们会在动物走过玩家的位置的时候显示Game Over界面。

**目标**：动物会固定间隔召唤，并走向屏幕底部。如果动物走过玩家所在的位置，就会触发GameOver信息。如果玩家打中了动物，动物就会销毁。



## 创建新方法来生成动物

将Update()中生成动物的代码整合为一个方法

```c#
void SpawnAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnTransform = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(animalPrefabs[animalIndex], spawnTransform, animalPrefabs[animalIndex].transform.rotation);
    }
```



## 定时生成动物

使用InvokeRepeating()方法作为计时器。

```c#
void Start()
    {
        InvokeRepeating("SpawnAnimal", delayTime, repeatRate);
    }
```



## 添加trigger和Collider组件

1. 在动物预制体中，添加Box Collider
2. 勾选Box Collider中的Is Trigger属性
3. 对所有的动物和投射物都添加Box Collider
4. 为投射物添加RigidBody组件，并停用use gravity



## 在碰撞时销毁对象

1. 创建DetectCollisions脚本，应用到投射物上

2. 使用Monobehaviour的OnTriggerEnter()方法，Destory投射物和与它碰撞的对象（在这个原型中，只有动物会和投射物碰撞）

为了避免多个投射物碰撞相消，加一个条件判断：

```c#
public GameObject target;
if (other.gameObject.name != target.name)
        {
            Destroy(gameObject);
            Destroy(other.gameObject); 
        }   
```



## 触发“Game Over”信息

1. 使用Debug.Log()
2. 在DestoryOutBounds脚本中，添加Log



# 挑战2 - 接球游戏

使用数组和随机数知识，完成本节挑战，让球从天空随机位置下落，并派你的狗在球落地前接住它。你需要调整合适的变量值、编写正确的if语句、有效检测碰撞、以及随机生成对象来完成这项挑战。

**目标**：

- 在随机的x位置生成三种小球中的一种
- 当用户按下空格的时候，生成一条狗跑出去接住球
- 如果狗碰到了球，小球就销毁
- 如果球碰到了地面，输出“Game Over”消息
- 狗和球在离开场景的时候 销毁。



## 总览

1. 打开Prototype2项目
2. 下载Challenge 2 Starter files，导入项目中
3. 打开Instructions文件夹，查看目标结果的视频，作为完成挑战的指引



## 狗在屏幕上方生成

Spawn Manager的问题，关联的Prefab都是Dog。改成Ball1,2,3就好。



## 玩家生成的是绿球而不是狗

Player Control组件关联的预制体有问题，改成Dog。



## 当小球碰到狗附近的位置的时候，就会销毁

意思是销毁过早，估计是碰撞的问题。

检查碰撞，三种小球都是用的Sphere Collider，没有问题

而狗使用的Box Collider范围没调整好。

![image-20230224083526564](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230224083526564.png)



## 离开屏幕区域的物体不会销毁

DestoryOutBounds的问题。检查了下发现狗是朝X轴负方向跑的，而判断用的Limit设的 x > 30

球是朝y轴负方向下落的，而判断条件用的z值



## 只生成了一种小球

应该是Spawn Manager的问题。检查发现Instantiate()没加Random



## 生成小球的间隔都一样

要求改成3秒到5秒。

将InvokeRepeating中的repeatRate改为Random.Range(3,6)



## 玩家可以连点生成狗狗

让玩家职能在一端时间后才可以再次生成

使用Time.time获取当前时间curTime，与上一次生成的时间lastTime进行比较，如果小于minSpawnInterval，那就不能生成。



# 实验：使用原始形状创建新项目

你将创建和设置即将转变成你自己的项目的原始项目。在这个阶段，你会使用原始形状（例如球、方块和平面等）作为对象的占位符，从免去寻找图形的困扰，更快添加功能。为了能够确定哪个对象对应哪个占位符，你会给每个对象添加一个颜色材质。

**目的**：所有的对象都在场景中被分配原始形状的占位符，并且相机被放置在符合项目类型的正确位置。



## 创建新的Unity 项目，重命名场景

1. 创建项目，命名为Personal Project，使用3D 模板
2. 将SampleScene重命名为My Game



## 创建背景平面

1. 在层级中，右键 > 3D Object > Plane添加一个平面
2. Reset为默认
3. 将Scale调整为(5, 1, 5)



## 创建原始形状玩家，附带新材质

1. 在层级中，右键 > 3D Object > Sphere，重命名为Player
2. 在资产文件夹，右键 > Create > Folder，命名为Materials
3. 在Materialsh中创建新的Material，命名为Blue
4. 将新Material的Albedo调整为蓝色，拖动到Player上



## 根据项目类型调整相机位置

我们的项目是个Top down类型项目，且会有enemy从上面移动下来，所以相机视角应该是Top down的。

![image-20230224174223736](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230224174223736.png)



## 敌人，障碍物，投射物，材质

添加其他必要物体的占位符。

对于我们的项目，我们需要几种敌人、一种投射物，以及PowerUp和Heal收集物。

![image-20230224205118217](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230224205118217.png)



## 导出Unity包来备份

1. 保存场景
2. 在Project窗口，右键Asset文件夹 > Export Package，点击Export
3. 创建BackUp文件夹存储备份，保存为V0.1

> 这其实也相当于本地版本管理了。





# 分享你的项目

在这个教程中，你可以回顾并进一步应用你所学的知识，然后将你的作品分享给其他创作者

教程中有四个额外功能，难度分别为简单、中等、困难和专家。你可以选择其中几种。



## 总览

看了下最终效果图，动物会在上左右三个方向随机生成，玩家可以四个方向移动，动物具有血条。



## 简单：让玩家能够垂直移动

再添加一个读取Vectical的getAxis()

```c#
verticalInput = Input.GetAxis("Vertical");
transform.Translate(Vector3.forward * speed * Time.deltaTime * verticalInput);
```

确定能够移动的上下界，为了方便起见，将相机位置改为(0,25,0)，DestoryOutBound的边界改为+-15.0f，SpawnManager的PosZ改为15.0f

调整代码，用两个if分别控制左右边界和上下边界（取绝对值，看blog有人提到，在保证不出界的情况下，使用三目的效率要比Math.abs()快，因为边界检查不可能溢出，所以直接用三目运算符）。

```c#
if((transform.position.x > 0 ? transform.position.x : -transform.position.x) > xRange)
{
    transform.position = new Vector3(transform.position.x > 0 ? xRange : -xRange, transform.position.y, transform.position.z);
}
if((transform.position.z > 0 ? transform.position.z : -transform.position.z) > zRange)
{
    transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z > 0 ? zRange : -zRange);
}
```



## 中等：好斗的动物

让动物在屏幕两侧也能生成，如果被动物撞到了，则显示”GameOver"

分两步，首先调整动物的碰撞，创建新的脚本“TriggerPlayerCollision”

在Start()中使用`GameObject.Find(string name)`获取Player对象

为Player添加RigidBody和Box Collider

> 始终牢记，使用OnTriggerEnter()检测碰撞的前提是，两个物体都有Collider，至少一方选中isTrigger，以及至少一方应用了RigidBody（应用RigidBody和应用is Trigger的可以不是同一个）

```c#
public class TriggerPlayerCollision : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Act when collision is happening
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == player)
        {
            Debug.Log("Game Over! - Animal Collision Recall.");
        }
    }
}
```



修改SpawnManager，让动物在左右也可以生成：

分别写左右生成的函数，然后InvokeRepeating()。

问题，如何让动物旋转？Instanitate()第三个参数是Quaternion。可以使用`Quaternion.Euler(Vector3 euler)`将欧拉角转换为四元数

```c#
public class SpawnManager : MonoBehaviour
{
    public GameObject[] animalPrefabs;
    private float spawnRangeX = 15.0f;
    public float spawnRangeZ = 10.0f;
    private float spawnPosZ = 15.0f;
    public float spawnPosX = 20.0f;
    public float delayTime = 2;
    public float repeatRate = 1.5f;
    
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnAnimal", delayTime, repeatRate);
        InvokeRepeating("SpawnAnimalLeft", delayTime, repeatRate);
        InvokeRepeating("SpawnAnimalRight", delayTime, repeatRate);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Spawn an animal when this method is called
    void SpawnAnimal()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnTransform = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, spawnPosZ);
        Instantiate(animalPrefabs[animalIndex], spawnTransform, animalPrefabs[animalIndex].transform.rotation);
    }
    void SpawnAnimalLeft()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnTransform = new Vector3(-spawnPosX, 0, Random.Range(-spawnRangeZ, spawnRangeZ));
        Instantiate(animalPrefabs[animalIndex], spawnTransform, Quaternion.Euler(0, 90, 0));
    }
    void SpawnAnimalRight()
    {
        int animalIndex = Random.Range(0, animalPrefabs.Length);
        Vector3 spawnTransform = new Vector3(spawnPosX, 0, Random.Range(-spawnRangeZ, spawnRangeZ));
        Instantiate(animalPrefabs[animalIndex], spawnTransform, Quaternion.Euler(0, -90, 0));
    }
}
```



修改DestoryOutBound，让代码也可以判断左右边界

```c++
public class DestroyOutBounds : MonoBehaviour
{
    private float boundsZ = 15.0f;
    private float boundsX = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Destory gameObject when out of bounds
        if((transform.position.z > 0 ? transform.position.z : -transform.position.z) > boundsZ)
        {
            Destroy(gameObject);
        }
        if ((transform.position.x > 0 ? transform.position.x : -transform.position.x) > boundsX)
        {
            Destroy(gameObject); 
        }
    }
}
```



## 困难：游戏用户界面

为玩家设置生命值和计分板，每当打到动物的时候，就加分，每当被碰到的时候，就减分。

创建GameManager脚本和空对象，将脚本加到空对象上

问题：如何获取这个组件？

可以直接在需要引用的脚本中，`private GameManager gameManager`声明GameManager类型的变量。然后用GameObject.Find找到包含该脚本的对象（例如GameManager）：`gameManager = GameObject.Find("GameManager").GetComponent<GameManager>()`；



在GameManager中，添加public方法AddLives(int value)

```c#
public void AddLives(int value)
    {
        if (lives > minLives)
        {
            lives += value;
            Debug.Log("Lives = " + lives);
        }
        else // lives <= minLives
        {
            Debug.Log("Game Over!");
        }
    }
```

在TriggerPlayerCollision中，引用GameManager的GameManager组件，在发生碰撞的时候调用AddLives(-1);

![image-20230301191506495](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301191506495.png)



同理，在GameManager中编写AddScore(int value)方法，在TriggerCollision中调用AddScore(1);

![image-20230301191647328](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301191647328.png)



## 专家：为动物添加饥饿值

在每个动物头顶添加一个“饥饿槽”，每当喂给动物食物的时候，就会填充一部分饥饿槽。喂饱每个动物需要的食物数量不同，当动物饥饿槽满的时候，动物才会消失。

编写一个新的脚本应用在动物上，然后在TriggerCollision中调用脚本方法。

问题：要怎么显示血条？



### 创建血条UI

1. 在层级中，右键 > UI > Slider，重命名Canvas为SliderCanvas

2. 选中SliderCanvas，在Inspector中，找到Canvas组件，修改Render Mode为World Space

   > 这里注意，修改为World Space后，要设置下面的Camera为场景中的Main Camera

   ![image-20230301200058065](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301200058065.png)

3. 调整大小

   > 调整Canvas大小必须在设置Render Mode之后，因为Canvas大小和Render Mode有关联

   ![image-20230301200217896](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301200217896-1677672138544-1.png)

4. 接下来调整Slider，展开Slider的子对象

   ![image-20230301200341724](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301200341724.png)

5. Handle不需要可以直接删掉，选中Background，调整Color和Image组件（背景颜色应该为暗色）

6. 在Fill Area对象中，调整RectTransform组件，将Right修改为5

   ![image-20230301200623580](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301200623580.png)

7. 在Fill对象中，调整填充颜色（例如绿色）

8. 调整Slider组件，应用Whole Numbers属性

   > 这个属性会限制slider使用int

9. 将血条保存为预制体

10. 将血条预制体应用在动物上

11. 新建脚本AnimalHunger

12. 在脚本中，添加一行`using UnityEngine.UI`，调用UI库

13. 声明变量

    ![image-20230301202042460](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301202042460.png)

    > 这里声明GameManager是为了调用AddScore()

14. 在Start()中初始化变量

    ![image-20230301202110082](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301202110082.png)

15. 编写FeedAnimal(int amount)方法，更新饱食度。如果饱食度满了就销毁动物

16. 在TriggerCollision中，使用`other.GetComponent<AnimalHunger>().FeedAnimal(1)`更新动物饱食度。



## 附注：CompareTag(string tag)

1. 在Inspector中可以设置对象和预制体的tag

   ![image-20230301205447722](D:\CS\Unity\Junior Programmer\Create with Code 1\4-Basic Gameplay.assets\image-20230301205447722.png)

2. 在代码中，可以通过CompareTag()来判断是否为对应类型的对象
