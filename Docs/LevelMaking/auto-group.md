# 自动归组

自动归组是另外一种关卡归组方法，不需要您写JSON配置，可以使用对象名称来自动生成归组信息。

例如，项目中自带的 leveltest 关卡就是使用了自动归组。

![图片](40.jpg)

自动归组的原理是根据对象的名字来判断物体是属于哪个组，物体名称规则如下：

* PS_LevelStart 固定，表示这是一个起始四火焰.
* PE_LevelEnd   固定，表示这是末尾飞船.
* PR_ResetPoint:[Sector] 出生点，Sector表示这是第几节。
* PC_CheckPoint:[Sector] 检查点，Sector表示这是第几节，注意检查点从2开始。
* DepthTestCubes 死亡检查区，只有它的一级子对象会被认为是死亡检查区。
* S_[Type] 路面组，Type是路面的名称。
  * 只有它的一级子对象会被认为是当前组路面。
  * 内置路面有：
    * Floors 普通路面
    * FloorWoods 木制路面
    * FloorRails 钢轨
    * FloorStopper 挡板
* P_[GroupName]:[ID]:[Sector] 机关。
  名字以 P_ 开头的对象认为是机关。
  * GroupName 是机关名称，例如 Modul_03。
  * ID 是用于区分不同物体的一个标识符。
  * Sector 表示这个机关将在第几节激活。
* O_[Name] 不参与归组的物体。
