---启动 vscode-debuggee 调试器
function StartVscodeDebuggee()
  local GamePackage = Ballance2.Package.GamePackage
  local SystemPackage = GamePackage.GetSystemPackage()
  local json = SystemPackage:RequireLuaFile('dkjson')
  local debuggee = SystemPackage:RequireLuaFile('vscode-debuggee')
  local startResult, breakerType = debuggee.start(json)
  print('debuggee start ->', startResult, breakerType)
end

---启动 mobdebug 调试器
function StartMobdebug()
  Ballance2.Package.GamePackage.GetSystemPackage():RequireLuaFile("mobdebug").start()
end

---@param name string
function InternalStart(name)
  if name == 'Mobdebug' then
    StartMobdebug()
  elseif name == 'VscodeDebuggee' then
    StartVscodeDebuggee()
  end
end
