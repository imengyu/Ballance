local Vector2 = UnityEngine.Vector2
local Renderer = UnityEngine.Renderer
local Yield = UnityEngine.Yield
local WaitForSeconds = UnityEngine.WaitForSeconds
local GameSoundType = Ballance2.Sys.Services.GameSoundType

---变球器控制器
---@class TranfoAminControl : GameLuaObjectHostClass
---@field _AnimTrafo_Flashfield GameObject
---@field _AnimTrafo_Group GameObject
---@field _AnimTrafo_Ringpart1 GameObject
---@field _AnimTrafo_Ringpart2 GameObject
---@field _AnimTrafo_Ringpart3 GameObject
---@field _AnimTrafo_Ringpart4 GameObject
---@field _AnimTrafo_Animation Animator 
TranfoAminControl = ClassicObject:extend()

function TranfoAminControl:new()
  self._Misc_Trafo = nil;
  self._Flashfield = false;
  self._Flashfield_Tick = false;
end
function TranfoAminControl:Start()
  self._Misc_Trafo = Game.SoundManager:RegisterSoundPlayer(GameSoundType.BallEffect,
    Game.SoundManager:LoadAudioResource('core.sounds:Misc_Trafo.wav'), false, true, 'Misc_Trafo')
  
  --获取变球器的颜色点材质
  local renderer = self._AnimTrafo_Ringpart1:GetComponent(Renderer) ---@type Renderer
  self._AnimTrafo_RingParts_Color1 = renderer.materials[1]
  renderer = self._AnimTrafo_Ringpart2:GetComponent(Renderer) ---@type Renderer
  self._AnimTrafo_RingParts_Color2 = renderer.materials[1]
  renderer = self._AnimTrafo_Ringpart3:GetComponent(Renderer) ---@type Renderer
  self._AnimTrafo_RingParts_Color3 = renderer.materials[1]
  renderer = self._AnimTrafo_Ringpart4:GetComponent(Renderer) ---@type Renderer
  self._AnimTrafo_RingParts_Color4 = renderer.materials[1]

  --获取变球器电流材质
  renderer = self._AnimTrafo_Flashfield:GetComponent(Renderer) ---@type Renderer
  self._AnimTrafo_FlashfieldMat = renderer.material
  self._AnimTrafo_Group:SetActive(false)

  GamePlay.TranfoManager = self
end
function TranfoAminControl:Update()
  if self._Flashfield then
    self._Flashfield_Tick = not self._Flashfield_Tick
    if self._Flashfield_Tick then
      self._AnimTrafo_FlashfieldMat:SetTextureOffset("_MainTex", Vector2.zero);
    else
      self._AnimTrafo_FlashfieldMat:SetTextureOffset("_MainTex", Vector2(0.5, 0));
    end
  end
end

---开始变球动画
---@param pos Vector3
---@param color Color
---@param placeholder GameObject
---@param ballChangeCallback function
function TranfoAminControl:PlayAnim(pos, color, placeholder, ballChangeCallback)
  self._AnimTrafo_Group:SetActive(true)
  self._AnimTrafo_Group.transform.position = pos

  ---隐藏占位变球器
  if placeholder ~= nil then
    placeholder:SetActive(false)
  end

  self._AnimTrafo_Flashfield:SetActive(true)
  self._Flashfield = true;

  --设置变球器颜色
  self._AnimTrafo_RingParts_Color1.color = color
  self._AnimTrafo_RingParts_Color2.color = color
  self._AnimTrafo_RingParts_Color3.color = color
  self._AnimTrafo_RingParts_Color4.color = color

  --播放动画和声音
  self._Misc_Trafo:Play()
  self._AnimTrafo_Animation:Play('TranfoAnimation')

  --延时关闭
  coroutine.resume(coroutine.create(function()
    Yield(WaitForSeconds(1.33))

    self._AnimTrafo_Animation:Stop('TranfoAnimation');
    self._AnimTrafo_Flashfield:SetActive(false)
    self._Flashfield = false;

    if ballChangeCallback ~= nil then
      ballChangeCallback()
    end
    
    Yield(WaitForSeconds(0.33))

    --隐藏本体，显示占位变球器
    self._AnimTrafo_Group.gameObject:SetActive(false)
    if placeholder ~= nil then
      placeholder:SetActive(true)
    end
  end))
end

function CreateClass_TranfoAminControl()
  return TranfoAminControl()
end