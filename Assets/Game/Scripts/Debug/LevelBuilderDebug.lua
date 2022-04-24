local GameManager = Ballance2.Services.GameManager

function CoreDebugLevelBuliderEntry()
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', 'Level01')
end
function CoreDebugLevelEnvironmentEntry()
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', '测试关卡')
end