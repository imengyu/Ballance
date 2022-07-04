local GameErrorChecker = Ballance2.Services.Debug.GameErrorChecker
local GameError = Ballance2.Services.Debug.GameError

---手机键盘管理器
---管理不同种类的键盘，你可以注册自己的键盘
---@class KeypadUIManager
KeypadUIManager = {}

local TAG = 'KeypadUIManager'
local keypadData = {}

---注册键盘
---@param name string 名称
---@param prefrab GameObject 键盘对象预制体
---@param image Sprite 这个键盘的菜单图片，用于菜单显示
---@return boolean u 返回true注册成功，返回false失败，可能是已经注册过同名键盘
function KeypadUIManager.AddKeypad(name, prefrab, image)
  if keypadData[name] ~= nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.AlreadyRegistered, TAG, 'Keypad '..name..' already registered!')
    return false
  end

  keypadData[name] = {
    prefrab = prefrab,
    image = image
  }
  return true
end 
---获取已注册键盘，如果没有找到则返回nil
---@param name string 名称
function KeypadUIManager.GetKeypad(name)
  return keypadData[name]
end 
---获取所有已注册键盘
function KeypadUIManager.GetAllKeypad()
  return keypadData
end 
---取消键盘
---@param name string 名称
---@return boolean u 返回true注册成功，返回false失败
function KeypadUIManager.DeletetKeypad(name)  
  if keypadData[name] == nil then
    GameErrorChecker.SetLastErrorAndLog(GameError.NotRegister, TAG, 'Keypad '..name..' not register!')
    return false
  end
  keypadData[name] = nil
  return true
end 