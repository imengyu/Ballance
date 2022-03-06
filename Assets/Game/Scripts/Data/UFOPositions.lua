local Vector3 = UnityEngine.Vector3

---@class UFOPositionItem 
---@field pos Vector3
---@field flyTime number
---@field waitTime number
---@field catchBall boolean
---@field startBall boolean
UFOPositionItem = {}

---UFO 动画的位置参数
UFOPositions = {
  { pos = Vector3(-500, -30, -50), flyTime = 0.01, waitTime = 3 },
  { pos = Vector3(-30, 7, -20), flyTime = 1.8, waitTime = 2 },
  { pos = Vector3(-30, 7, 20), flyTime = 1.8, waitTime = 2 },
  { pos = Vector3(8,0,3), flyTime = 1.8, waitTime = 3 },
  { pos = Vector3(0,6,0), flyTime = 1.8, waitTime = 3 },
  { pos = Vector3(0,6,0), flyTime = 0.5, waitTime = 1.0, startBall = true },
  { pos = Vector3(0,0,0), flyTime = 0.3, waitTime = 1.2, catchBall = true },
  { pos = Vector3(0,0,0), flyTime = 0.1, waitTime = 0.02 },
  { pos = Vector3(0,5,0), flyTime = 0.6, waitTime = 1 },
  { pos = Vector3(-25,15,0), flyTime = 1.1, waitTime = 2 },
  { pos = Vector3(-20,7,20), flyTime = 1, waitTime = 1.5 },
  { pos = Vector3(-25,10,-50), flyTime = 1, waitTime = 1.5 },
  { pos = Vector3(50,-10,-50), flyTime = 2.5, waitTime = 4 },
  { pos = Vector3(-500,30,250), flyTime = 1.1, waitTime = 1.1 },
  { pos = Vector3(-200,0,0), flyTime = 0.9, waitTime = 1.8 },
}

return UFOPositions