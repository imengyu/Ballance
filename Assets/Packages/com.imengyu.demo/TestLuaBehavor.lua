--这里导入了Unity的一些定义
local Vector3 = UnityEngine.Vector3
local Time = UnityEngine.Time
--这里导入了Ballance2框架的一些定义
local Log = Ballance2.Log

---测试Lua类
---这个类为你展示了如何使用 GameLuaObjectHostClass 和 Lua 类
---@class TestLuaBehavor : GameLuaObjectHostClass
---@field MyVar Camera 添加了一个变量，类型是 Camera
TestLuaBehavor = ClassicObject:extend()

--暴露方法给 GameLuaObjectHost 创建当前类
function CreateClass:TestLuaBehavor()
  return TestLuaBehavor()
end

--New 函数
function TestLuaBehavor:new()
  --new 中是当前类创建的时候调用
  --你可以在这里初始化一些变量
  self.speed = 3
end

---start 函数，与 MonoBehavior Start 一样
function TestLuaBehavor:Start()
  Log.D('Test', "Start!")

  --使用 self.gameObject 可以访问当前游戏对象
  Log.D('Test', self.gameObject)

  --使用 self.transform 可以访问当前变换对象
  Log.D('Test', self.transform)
end

---Update，与 MonoBehavior Update 一样
--但是注意，lua 中不建议在update中执行大量操作，会产生大量跨语言调用和GC，非常影响性能。
--Update 的调用频率可以在 GameLuaObjectHostClass 中设置 UpdateDelta 来限制，UpdateDelta设置多少就表示每隔几帧调用一次。
function TestLuaBehavor:Update()
  --调用API旋转当前物体
  self.transform:RotateAround(Vector3.up, self.speed * Time.deltaTime)
end