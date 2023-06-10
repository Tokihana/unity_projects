# 此前已经实现的（太久远了没法考证）

- 创建基本对象
- 实现玩家移动与Shift减速（即PlayerController）

# 2023/5/18 v0.01

- 重新规划实体类型，将enemy精简为BaseEnemy一种

- 编写Movement.cs，使用direction和speed控制移动，应用到Enemy，Heal，Powerup和Projectile上

- 创建Destory.cs，在对象离开bounding box的时候销毁

  > 后续补充：创建的时候打错字了，Destroy打成Destory，将错就错吧。

- 对敌人和玩家创建的投射物进行区分，创建Tag：Enemy Projectile和Player Projectile；从而更好地处理碰撞

- 处理玩家与敌人、掉落物、敌人子弹、敌人与玩家子弹之间的碰撞；为Enemy添加RigidBody

- 创建Projectile.cs，用于记录子弹的伤害

- 转换BaseEnemy、Player Projectile、Enemy Projectile、Heal、Powerup为Prefab

- **重要**：明确备份方法，创建Gitee备份

# 2023/5/19 v0.02

- 创建SpawnManager，实现随机生成Enemy
- 实现玩家发射子弹的逻辑，修改Movements的实现（删除了direction，因为Instantiate里面设置Rotation就好）
- 实现多角度和同角度多发子弹同时射击的逻辑
- 实现Powerup的逻辑，并在Enemy的生命值归零的时候生成Powerup
- 修改Shift减速的逻辑，实现按下Shift减速的同时子弹击中
- 尝试了创建判定点，但效果不好，暂时删除。

> gameplay目前已基本实现