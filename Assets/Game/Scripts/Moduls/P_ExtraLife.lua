local GameSoundType = Ballance2.Sys.Services.GameSoundType
local Physics = UnityEngine.Physics
local Vector3 = UnityEngine.Vector3

---@class P_ExtraLife : ModulBase 
---@field P_Extra_Life_Particle_Fizz GameObject
---@field P_Extra_Life_Particle_Blob GameObject
---@field P_Extra_Life_Sphere GameObject
---@field P_Extra_Life_Shadow GameObject
---@field P_Extra_Life_Animator Animator
---@field P_Extra_Life_Tigger PhysicsPhantom
P_ExtraLife = ModulBase:extend()

function P_ExtraLife:new()
end

function P_ExtraLife:Start()
  ---@param phantom PhysicsPhantom
  ---@param otherBody PhysicsBody
  self.P_Extra_Life_Tigger.onOverlappingCollidableAdd = function (phantom, otherBody)
    if not self._Actived and otherBody.gameObject.tag == 'Ball' then
      self._Actived = true;
      self.P_Extra_Life_Sphere:SetActive(false)
      self.P_Extra_Life_Shadow:SetActive(false)
      self.P_Extra_Life_Particle_Fizz:SetActive(true)
      self.P_Extra_Life_Particle_Blob:SetActive(true)
      Game.SoundManager:PlayFastVoice('core.sounds:Extra_Life_Blob.wav', GameSoundType.Normal)
      GamePlay.GamePlayManager:AddLife()
    end
  end
  --触发射线，检查当前下方是不是路面，如果是，则显示 Shadow 
  ---@type boolean
  local ok, 
  ---@type RaycastHit
  hitinfo = Physics.Raycast(self.transform.position, Vector3(0, -1, 0), Slua.out, 5) 
  if ok and hitinfo.collider ~= nil then
    local parentName = hitinfo.collider.gameObject.tag
    if parentName == 'Phys_Floors' or parentName == 'Phys_FloorWoods' then
      self._OnFloor = true
    else
      self._OnFloor = false
    end
  else
    self._OnFloor = false
  end
end
function P_ExtraLife:Active()
  if self._OnFloor then
    self.P_Extra_Life_Shadow:SetActive(true)
    --如果在路面上，还要播放上下的动画
    self.P_Extra_Life_Animator:Play('P_ExtraLife_Updown_Animation', 1)
  end
  
  self.P_Extra_Life_Animator.speed = 1
  self.P_Extra_Life_Animator:Play('P_ExtraLife_Animation', 0)
  self.P_Extra_Life_Sphere:SetActive(true)
end
function P_ExtraLife:Deactive()
  self.P_Extra_Life_Sphere:SetActive(false)
  self.P_Extra_Life_Shadow:SetActive(false)
  self.P_Extra_Life_Particle_Fizz:SetActive(false)
  self.P_Extra_Life_Particle_Blob:SetActive(false)
  self.P_Extra_Life_Animator.speed = 0
end
function P_ExtraLife:Reset()
  self._Actived = false
  self.P_Extra_Life_Particle_Fizz:SetActive(false)
  self.P_Extra_Life_Particle_Blob:SetActive(false)
  self.P_Extra_Life_Animator:Stop()
end

function CreateClass_P_ExtraLife()
  return P_ExtraLife()
end