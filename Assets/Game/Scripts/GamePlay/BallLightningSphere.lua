--[[

模块说明：球出生以及变换时的闪电效果控制
作者： imengyu

]]--

local GameSoundType = Ballance2.Services.GameSoundType
local Vector3 = UnityEngine.Vector3
local Yield = UnityEngine.Yield
local Time = UnityEngine.Time
local Color = UnityEngine.Color
local WaitForSeconds = UnityEngine.WaitForSeconds

---闪电球动画控制脚本
---@class BallLightningSphere : GameLuaObjectHostClass
BallLightningSphere = {
    -- public:
    Ball_Light = nil, ---@type Light
    Ball_Smoke = nil, ---@type GameObject
    Ball_LightningSphereInnernA = nil, ---@type GameObject
    Ball_LightningSphereInnernB = nil, ---@type GameObject
    BallLightningSphereTexture1 = nil, ---@type Texture
    BallLightningSphereTexture2 = nil, ---@type Texture
    BallLightningSphereTexture3 = nil, ---@type Texture
    BallLightningBallBigCurve = nil, ---@type AnimationCurve
    BallLightningCurveEnd = nil, ---@type AnimationCurve
    BallLightningCurve = nil, ---@type AnimationCurve
    BallLightBallBigSec = 1.5,
    BallLightSec = 1.5,
    BallLightEndSec = 1.5,

    --private:
    lighing = false,
    lighingLight = false,
    lighingLightEnd = false,
    lighingLightBigTick = 0,
    lighingLightTick = 0,
    lighingLightEndTick = 0, 
    lighingLightColorAlpha = 0.6, 
    lighingBig = false, 
    lighingControlTick = 0,
    ballLightningSphereMaterialA = nil, ---@type Material
    ballLightningSphereMaterialB = nil, ---@type Material
    ballLightingRoateSpeed1 = 400,
    ballLightingRoateSpeed2 = 400,
    ballLightningSphereTextureCurrent = 1,
    ballLightningMusic = nil, ---@type AudioSource
}

---@return BallLightningSphere
function CreateClass:BallLightningSphere()

  function BallLightningSphere:new(o)
    o = o or {}
    setmetatable(o, self)
    self.__index = self
    return o
  end

  ---@param self table
  ---@param thisGameObject GameObject
  function BallLightningSphere:Start(thisGameObject)
    self.ballLightningMusic = Game.SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect,
        Game.SoundManager:LoadAudioResource('core.sounds:Misc_Lightning.wav'), false, true, 'Misc_Lightning')
    local meshRenderer = self.Ball_LightningSphereInnernA.gameObject:GetComponent(UnityEngine.MeshRenderer) ---@type MeshRenderer
    self.ballLightningSphereMaterialA = meshRenderer.material
    meshRenderer = self.Ball_LightningSphereInnernB.gameObject:GetComponent(UnityEngine.MeshRenderer) ---@type MeshRenderer
    self.ballLightningSphereMaterialB = meshRenderer.material
  end
  function BallLightningSphere:OnDestroy()
    self.ballLightningMusic = nil
  end

  function BallLightningSphere:Update()
    local Ball_LightningSphereInnernA = self.Ball_LightningSphereInnernA
    local Ball_LightningSphereInnernB = self.Ball_LightningSphereInnernB
    --闪电球
    if (self.lighing) then
        if (Ball_LightningSphereInnernA.transform.localEulerAngles.z > 360) then Ball_LightningSphereInnernA.transform.localEulerAngles = Vector3(0, 0, 0) end
        if (Ball_LightningSphereInnernB.transform.localEulerAngles.z < -360) then Ball_LightningSphereInnernB.transform.localEulerAngles = Vector3(0, 0, 0) end
        Ball_LightningSphereInnernA.transform.localEulerAngles = Vector3(0, Ball_LightningSphereInnernA.transform.localEulerAngles.y + self.ballLightingRoateSpeed1 * Time.deltaTime, 0)
        Ball_LightningSphereInnernB.transform.localEulerAngles = Vector3(0, Ball_LightningSphereInnernB.transform.localEulerAngles.y - self.ballLightingRoateSpeed2 * Time.deltaTime, 0)

        --更换闪电球贴图
        if (self.lighingControlTick < 0.1) then self.lighingControlTick = self.lighingControlTick + Time.deltaTime
        else
            if (self.ballLightningSphereTextureCurrent >= 3) then self.ballLightningSphereTextureCurrent = 1
            else self.ballLightningSphereTextureCurrent = self.ballLightningSphereTextureCurrent + 1 end
            
            --按数字更换贴图
            if (self.ballLightningSphereTextureCurrent == 1) then
                self.ballLightningSphereMaterialA.mainTexture = self.BallLightningSphereTexture1
                self.ballLightningSphereMaterialB.mainTexture = self.BallLightningSphereTexture1
            elseif (self.ballLightningSphereTextureCurrent == 2) then
                self.ballLightningSphereMaterialA.mainTexture = self.BallLightningSphereTexture2
                self.ballLightningSphereMaterialB.mainTexture = self.BallLightningSphereTexture2
            elseif (self.ballLightningSphereTextureCurrent == 3) then
                self.ballLightningSphereMaterialA.mainTexture = self.BallLightningSphereTexture3
                self.ballLightningSphereMaterialB.mainTexture = self.BallLightningSphereTexture3
            end

            self.lighingControlTick = 0
        end
    end
    --闪电球 放大
    if (self.lighingBig) then
        if(self.lighingLightBigTick < self.BallLightBallBigSec) then
            self.lighingLightBigTick = self.lighingLightBigTick + Time.deltaTime

            local v = self.BallLightningBallBigCurve:Evaluate(self.lighingLightBigTick / self.BallLightBallBigSec)
            Ball_LightningSphereInnernA.transform.localScale = Vector3(v, v, v)
            Ball_LightningSphereInnernB.transform.localScale = Vector3(v, v, v)
        else
            Ball_LightningSphereInnernA.transform.localScale = Vector3(0.9, 0.9, 0.9)
            Ball_LightningSphereInnernB.transform.localScale = Vector3(0.9, 0.9, 0.9)
            self.lighingBig = false
        end
    end
    --闪电light
    if (self.lighingLight) then
        self.lighingLightTick = self.lighingLightTick + Time.deltaTime

        self.Ball_Light.color = Color(self.Ball_Light.color.r, self.Ball_Light.color.g,
            self.BallLightningCurve:Evaluate(self.lighingLightTick / self.BallLightSec), self.lighingLightColorAlpha)

        if (self.lighingLightTick > self.BallLightSec) then
            self.lighingLightEndTick = 0
            self.lighingLightEnd = true
            self.lighingLight = false
        end
    end
    --闪电light结尾的一闪
    if (self.lighingLightEnd) then        
        self.lighingLightEndTick = self.lighingLightEndTick + Time.deltaTime
        
        local v = self.BallLightningCurveEnd:Evaluate(self.lighingLightEndTick / self.BallLightEndSec)
        self.Ball_Light.color = Color(v, v, v, self.lighingLightColorAlpha)
        if (self.lighingLightEndTick > self.BallLightEndSec) then
            self.Ball_Light.gameObject:SetActive(false)
            self.lighingLightEnd = false
        end
    end
  end

  function BallLightningSphere:IsLighting() return self.lighing end

  ---播放球 闪电动画
  ---@param position Vector3 位置
  ---@param smallToBig boolean 是否由小变大
  ---@param callback function 完成回调
  ---@param lightAnim boolean 是否播放相对应的 Light 灯光
  function BallLightningSphere:PlayLighting(position, smallToBig, lightAnim, callback)
    local Ball_LightningSphereInnernA = self.Ball_LightningSphereInnernA
    local Ball_LightningSphereInnernB = self.Ball_LightningSphereInnernB

    if self.lighing then
      return
    end

    --播放闪电声音
    self.lighing = true
    self.lighingLight = false
    self.lighingLightEnd = false
    if (self.ballLightningMusic ~= nil) then
        self.ballLightningMusic:Play()
    end
    
    self.Ball_Light.transform.position = position
    --显示球
    Ball_LightningSphereInnernA.gameObject:SetActive(true)
    Ball_LightningSphereInnernA.transform.position = position
    Ball_LightningSphereInnernB.gameObject:SetActive(true)
    Ball_LightningSphereInnernB.transform.position = position

    if (smallToBig) then
        Ball_LightningSphereInnernA.transform.localScale = Vector3(0.01, 0.01, 0.01)
        Ball_LightningSphereInnernB.transform.localScale = Vector3(0.01, 0.01, 0.01)
        self.lighingLightBigTick = 0
        self.lighingBig = true
    end

    if (lightAnim) then
        self.lighingLight = true
        self.lighingLightTick = 0
        self.Ball_Light.gameObject:SetActive(true)
    else
        self.Ball_Light.gameObject:SetActive(false)
    end

    --延时关闭
    coroutine.resume(coroutine.create(function()
        Yield(WaitForSeconds(self.BallLightSec))
        
        Ball_LightningSphereInnernA.transform.localScale = Vector3(1, 1, 1)
        Ball_LightningSphereInnernB.transform.localScale = Vector3(1, 1, 1)
        Ball_LightningSphereInnernB.gameObject:SetActive(false)
        Ball_LightningSphereInnernA.gameObject:SetActive(false)
        self.Ball_Smoke.transform.position = position
        self.Ball_Smoke:SetActive(true)
        self.lighing = false

        if (type(callback) == 'function') then
          callback()
        end
        if (lightAnim) then
          Yield(WaitForSeconds(self.BallLightEndSec))
          self.Ball_Light.gameObject:SetActive(false)
        end
    end))
  end

  return BallLightningSphere:new(nil)
end