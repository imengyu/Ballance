local GameManager = Ballance2.Sys.GameManager

function CoreDebugLevelBuliderEntry()
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', 'Level01')
end
function CoreDebugLevelEnvironmentEntry()
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', 'LevelTest')
end