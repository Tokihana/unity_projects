Unity提供了两种方法来处理交互或者说碰撞时的对象行为，分别是Collision和Trigger



## Collision

OnCollisionEnter(Collision other)接收一个Collision类型的参数，该参数返回例如接触点、或者速度等信息。这类事件发生在两个或多个Collider之间交互的时候，因此，想要该方法起作用，检测事件的对象必须有Rigidbody组件。

OnCollisionStay(Collision other)在两个或更多有Rigidbody和Collider组件的物体接触的每一帧都被调用。

OnCollisionExit(Collision other)在两个带有Rigidbody和Collider组建的物体停止接触的时候被调用一次



## Trigger

OnTriggerEnter(Collider other)接收Collider类型的参数，这个参数返回被检测到的Collider的信息。collision发生的位置不是关注点。通常来说，OnTriggerEnter()方法用于返回进入Collider空间的GameObject的信息（即`other.gameObject`）

OnTriggerStay(Collider other)在某个Collider接触到Trigger空间的每一帧都被调用。

OnTriggerExit(Collider other)在某个Collider或者Rigidbody停止接触Trigger空间的时候被调用。



## 总结

OnCollisionEnter()通常用来检查或者修改物理行为。

而OnTriggerEnter()用来检测Trigger area，在Trigger area中会忽略物理属性。

选择何种方法取决于需求、以及需要检测和交互的对象。

![img](D:\CS\Unity\什么时候用OnCollisionEnter，什么时候用OnTriggerEnter？.assets\1nlWqmmvTmDFK4Nwb7VaWPw.gif)



> 简单地说，如果你需要处理物理效果，就用OnColliderEnter，如果不想出现物理效果，就用OnTriggerEnter()

