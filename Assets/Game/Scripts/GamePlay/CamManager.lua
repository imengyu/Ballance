---@type GameLuaObjectHostClass
---@class CamManager
CamManager = {
  CameraOverlook = nil, ---@type GameObject
  CameraRotateHost = nil, ---@type GameObject
  SkyBox = nil, ---@type Skybox
}

function CreateClass_CamManager()
  function CamManager:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function CamManager:Start(thisGameObject)

  end
  function CamManager:OnDestroy()

  end
  ---设置主摄像机天空盒材质
  ---@param mat Material
  function CamManager:SetSkyBox(mat)
    self.SkyBox.material = mat
  end

  return CamManager:new(nil)
end