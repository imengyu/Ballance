local GameManager = Ballance2.Sys.GameManager

function CoreDebugLevelBuliderEntry()
  GameManager.GameMediator:NotifySingleEvent('CoreStartLoadLevel', '01');
end